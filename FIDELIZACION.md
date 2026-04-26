# Programa de Millas y Fidelización — AirTicketSystem

## ¿Qué es y para qué sirve?

El programa de fidelización recompensa a los clientes por volar con la aerolínea.
Cada vez que un vuelo aterriza, los pasajeros que tenían reservas confirmadas acumulan
millas en su cuenta. Esas millas pueden descontarse del valor de futuras reservas,
funcionando como dinero dentro del sistema.

---

## Regla de acumulación

> **1 milla por cada $1.000 COP del valor de la reserva.**

| Valor de reserva | Millas ganadas |
|---|---|
| $50.000 COP | 50 millas |
| $250.000 COP | 250 millas |
| $1.200.000 COP | 1.200 millas |

La acumulación ocurre **automáticamente** cuando el administrador registra el
aterrizaje de un vuelo (`RegisterLandingFlightUseCase`). No requiere ninguna
acción adicional del cliente.

---

## Regla de redención (conversión de millas a dinero)

> **1 milla = $1 COP.**

Esto es la inversa de la acumulación: si ganaste 1 milla por cada $1.000,
al gastarlas obtienes $1 de descuento por milla. El cliente puede redimir
millas de forma **parcial o total** al momento de pagar una reserva.

**Ejemplo de pago combinado:**

| Concepto | Valor |
|---|---|
| Valor total de la reserva | $80.000 COP |
| Millas a redimir | 30.000 millas |
| Descuento por millas | $30.000 COP |
| **Saldo a pagar en dinero** | **$50.000 COP** |

---

## Niveles del programa

El nivel se calcula sobre el **total histórico acumulado** (no sobre el saldo
disponible). Redimir millas no baja el nivel.

| Nivel | Millas históricas acumuladas |
|---|---|
| BRONCE | 0 — 4.999 |
| PLATA | 5.000 — 19.999 |
| ORO | 20.000 — 49.999 |
| PLATINO | 50.000 o más |

---

## Modelo de datos

```
clientes (tabla existente)
    │  1:1
    ▼
cuentas_millas
    │  id, cliente_id (único), saldo_actual,
    │  miles_acumuladas_total, nivel, fecha_inscripcion
    │
    │  1:N
    ▼
miles_movimientos
    │  id, cuenta_id, reserva_id (nullable),
    │  tipo (ACUMULACION | REDENCION),
    │  millas, fecha, descripcion
    
pagos (tabla existente, modificada)
    │  + miles_usadas (nullable)
    │    → registra cuántas millas se usaron en ese pago
```

**Dos campos separados en `cuentas_millas`:**

- `saldo_actual` — millas disponibles para gastar. Sube al acumular, baja al redimir.
- `miles_acumuladas_total` — total histórico de por vida. **Solo sube, nunca baja.**
  Se usa exclusivamente para calcular el nivel.

---

## Flujos principales

### Alta de cliente → cuenta creada automáticamente

```
RegisterClientUseCase
    └─ CreateClientUseCase
        ├─ INSERT clientes
        └─ INSERT cuentas_millas  (saldo=0, nivel=BRONCE)
```

### Vuelo aterriza → millas acumuladas automáticamente

```
RegisterLandingFlightUseCase
    ├─ flight.RegistrarAterrizaje()  →  UPDATE vuelos (estado=ATERRIZADO)
    └─ AcumularMilesPorVueloUseCase
         Para cada reserva CONFIRMADA del vuelo:
           ├─ millas = max(1, (int)(valorReserva / 1.000))
           ├─ cuenta.AcumularMiles(millas)
           │    ├─ saldo_actual += millas
           │    ├─ miles_acumuladas_total += millas
           │    └─ nivel = recalcular según total
           ├─ INSERT miles_movimientos  (tipo=ACUMULACION)
           └─ UPDATE cuentas_millas
```

**Idempotencia:** si el proceso se ejecuta dos veces para el mismo vuelo,
la segunda pasada detecta que ya existe un movimiento `ACUMULACION` para cada
reserva y la omite. Ningún cliente recibe millas dobles.

### Cliente redime millas al pagar

```
MilesMenu → opción "Redimir millas en una reserva"
    │
    ├─ RegistrarRedencionUseCase
    │    ├─ cuenta.RedimirMiles(millas)   ← valida saldo suficiente
    │    ├─ saldo_actual -= millas
    │    ├─ miles_acumuladas_total  (sin cambio)
    │    └─ INSERT miles_movimientos  (tipo=REDENCION)
    │
    └─ CreatePaymentUseCase(monto=dineroRestante, milesUsadas=millas)
         └─ INSERT pagos  (miles_usadas = millas redimidas)
```

---

## Reportes LINQ implementados (8.8 — 8.12)

| # | Reporte | Operadores LINQ |
|---|---|---|
| 8.8 | Clientes con más millas acumuladas | `OrderByDescending` · `Take` |
| 8.9 | Clientes que más redimen | `Where` · `GroupBy` · `Sum` · `OrderByDescending` |
| 8.10 | Aerolíneas con mayor fidelización | Joins en memoria · `GroupBy` · `Sum` |
| 8.11 | Rutas con mayor acumulación | Joins en memoria · `GroupBy` · `Sum` |
| 8.12 | Ranking de viajeros frecuentes | `OrderByDescending` · `ThenByDescending` · `Take` |

Los reportes 8.10 y 8.11 encadenan cuatro fuentes de datos:
`miles_movimientos → reservas → vuelos → rutas → aerolíneas`,
usando diccionarios de lookup para joins O(1) en memoria.

---

## Arquitectura hexagonal — dónde vive cada pieza

```
Domain (lógica pura, sin dependencias externas)
  MilesCuenta.cs          Reglas: AcumularMiles(), RedimirMiles(), nivel
  MilesMovimiento.cs      Inmutable: PorVueloCompletado(), PorRedencionEnReserva()
  NivelMilesCuenta.cs     Calcula nivel según total histórico
  SaldoMilesCuenta.cs     Valida que el saldo no sea negativo

Application (orquestación, coordina aggregates y repositorios)
  AcumularMilesPorVueloUseCase   Coordina: IFlightRepo + IBookingRepo +
                                           IMilesCuentaRepo + IMilesMovimientoRepo
  RegistrarAcumulacionUseCase    Coordina: IMilesCuentaRepo + IMilesMovimientoRepo
  RegistrarRedencionUseCase      Coordina: IMilesCuentaRepo + IMilesMovimientoRepo

Infrastructure (persistencia)
  MilesCuentaRepository          CRUD sobre tabla cuentas_millas
  MilesMovimientoRepository      INSERT-only sobre tabla miles_movimientos
```

**Regla que nunca se viola:** el Domain no importa nada de Infrastructure.
Los repositorios son interfaces en el Domain; las implementaciones concretas
están en Infrastructure. La capa Application habla con el Domain a través
de esas interfaces.
