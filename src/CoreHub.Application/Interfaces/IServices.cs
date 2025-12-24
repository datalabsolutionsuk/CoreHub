using CoreHub.Domain.Entities;
using CoreHub.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CoreHub.Application.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid? TenantId { get; }
    string? Email { get; }
    string UserName { get; }
    bool IsInRole(string role);
    bool HasPermission(string permission);
}

public interface IApplicationDbContext
{
    // Exposed DbSets - actual implementation in Infrastructure
    DbSet<Client> Clients { get; }
    DbSet<Measure> Measures { get; }
    DbSet<AdministeredForm> AdministeredForms { get; }
    DbSet<Session> Sessions { get; }
    DbSet<ClientFlag> ClientFlags { get; }
    DbSet<Appointment> Appointments { get; }
    DbSet<CaseNote> CaseNotes { get; }
    DbSet<User> Users { get; }
    DbSet<Program> Programs { get; }
    DbSet<Tenant> Tenants { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IScoringService
{
    Task<ScoreResult> CalculateScoreAsync(Guid measureId, Dictionary<Guid, int> answers);
    Task<bool> DetectRiskAsync(Guid measureId, Dictionary<Guid, int> answers);
}

public interface IFlagRulesEngine
{
    Task EvaluateClientAsync(Guid clientId);
    Task<bool> ShouldRaiseFlagAsync(string ruleType, Guid clientId);
}

public interface IDataQualityCalculator
{
    Task<int> CalculateScoreAsync(Guid clientId, Guid? programId = null);
    Task<Dictionary<string, bool>> GetMissingFieldsAsync(Guid clientId);
}

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    Task SendTemplatedEmailAsync(string to, string templateKey, Dictionary<string, string> parameters);
    Task SendAppointmentReminderAsync(Guid appointmentId);
}

public interface ISmsService
{
    Task SendSmsAsync(string to, string message);
    Task SendAppointmentReminderSmsAsync(Guid appointmentId);
}

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream stream, string fileName, string containerName = "documents");
    Task<Stream> DownloadAsync(string blobReference);
    Task DeleteAsync(string blobReference);
    Task<string> GetUrlAsync(string blobReference, TimeSpan expiry);
}

public interface IExportService
{
    Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName);
    Task<byte[]> ExportToPdfAsync(string html);
}

public interface ILetterService
{
    Task<string> RenderLetterAsync(Guid templateId, Guid clientId, Dictionary<string, string>? additionalData = null);
    Task<byte[]> GeneratePdfAsync(Guid letterId);
}

public interface ICalendarService
{
    Task<string> GenerateIcsAsync(Guid appointmentId);
    Task SendCalendarInviteAsync(Guid appointmentId);
}

public record ScoreResult(
    decimal TotalScore,
    Dictionary<string, decimal> SubscaleScores,
    List<string> InterpretationFlags
);
