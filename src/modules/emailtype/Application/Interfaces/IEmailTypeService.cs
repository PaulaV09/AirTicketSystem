// src/modules/emailtype/Application/Interfaces/IEmailTypeService.cs
using AirTicketSystem.modules.emailtype.Infrastructure.entity;

namespace AirTicketSystem.modules.emailtype.Application.Interfaces;

public interface IEmailTypeService
{
    Task<EmailTypeEntity> CreateAsync(string nombre);
    Task<EmailTypeEntity?> GetByIdAsync(int id);
    Task<IEnumerable<EmailTypeEntity>> GetAllAsync();
    Task<EmailTypeEntity> UpdateAsync(int id, string nombre);
    Task DeleteAsync(int id);
}