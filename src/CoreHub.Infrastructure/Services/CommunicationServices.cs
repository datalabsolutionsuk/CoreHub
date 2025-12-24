using CoreHub.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace CoreHub.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            _configuration["Email:FromName"], 
            _configuration["Email:FromAddress"]));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        if (isHtml)
            bodyBuilder.HtmlBody = body;
        else
            bodyBuilder.TextBody = body;

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(
            _configuration["Email:SmtpServer"],
            int.Parse(_configuration["Email:SmtpPort"] ?? "587"),
            SecureSocketOptions.StartTls);

        if (!string.IsNullOrEmpty(_configuration["Email:Username"]))
        {
            await client.AuthenticateAsync(
                _configuration["Email:Username"],
                _configuration["Email:Password"]);
        }

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendTemplatedEmailAsync(string to, string templateKey, Dictionary<string, string> parameters)
    {
        // Load template from DB or file
        var subject = "Template Subject"; // Load from template
        var body = "Template Body"; // Load and replace {{parameters}}
        
        foreach (var param in parameters)
        {
            body = body.Replace($"{{{{{param.Key}}}}}", param.Value);
        }

        await SendEmailAsync(to, subject, body, isHtml: true);
    }

    public async Task SendAppointmentReminderAsync(Guid appointmentId)
    {
        // Load appointment, generate reminder email
        await Task.CompletedTask;
    }
}

public class SmsService : ISmsService
{
    private readonly IConfiguration _configuration;

    public SmsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendSmsAsync(string to, string message)
    {
        // Twilio implementation
        var accountSid = _configuration["Twilio:AccountSid"];
        var authToken = _configuration["Twilio:AuthToken"];
        var fromNumber = _configuration["Twilio:FromNumber"];

        // TwilioClient.Init(accountSid, authToken);
        // await MessageResource.CreateAsync(...)
        
        await Task.CompletedTask;
    }

    public async Task SendAppointmentReminderSmsAsync(Guid appointmentId)
    {
        // Load appointment, send SMS reminder
        await Task.CompletedTask;
    }
}

public class FileStorageService : IFileStorageService
{
    private readonly IConfiguration _configuration;

    public FileStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, string containerName = "documents")
    {
        // Azure Blob Storage implementation
        // Or local file system fallback
        var blobReference = $"{containerName}/{Guid.NewGuid()}_{fileName}";
        
        // Save to storage
        // Return blob reference
        
        return await Task.FromResult(blobReference);
    }

    public async Task<Stream> DownloadAsync(string blobReference)
    {
        // Download from storage
        return await Task.FromResult(Stream.Null);
    }

    public async Task DeleteAsync(string blobReference)
    {
        // Delete from storage
        await Task.CompletedTask;
    }

    public async Task<string> GetUrlAsync(string blobReference, TimeSpan expiry)
    {
        // Generate SAS URL for Azure Blob
        // Or signed URL for other storage
        return await Task.FromResult($"https://storage.example.com/{blobReference}");
    }
}
