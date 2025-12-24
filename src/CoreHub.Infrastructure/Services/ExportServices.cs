using CoreHub.Application.Interfaces;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data;

namespace CoreHub.Infrastructure.Services;

public class ExportService : IExportService
{
    public async Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        // Get properties
        var properties = typeof(T).GetProperties();
        
        // Headers
        for (int i = 0; i < properties.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = properties[i].Name;
            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
        }

        // Data
        int row = 2;
        foreach (var item in data)
        {
            for (int col = 0; col < properties.Length; col++)
            {
                var value = properties[col].GetValue(item);
                worksheet.Cell(row, col + 1).Value = value?.ToString() ?? "";
            }
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return await Task.FromResult(stream.ToArray());
    }

    public async Task<byte[]> ExportToPdfAsync(string html)
    {
        // QuestPDF implementation
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .Text("CoreHub Report")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Text(html); // Simple text - enhance with HTML parsing

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        });

        return await Task.FromResult(document.GeneratePdf());
    }
}

public class LetterService : ILetterService
{
    public async Task<string> RenderLetterAsync(Guid templateId, Guid clientId, Dictionary<string, string>? additionalData = null)
    {
        // Load template
        // Load client data
        // Merge fields
        var rendered = "<html><body>Letter content with {{ClientName}}</body></html>";
        
        return await Task.FromResult(rendered);
    }

    public async Task<byte[]> GeneratePdfAsync(Guid letterId)
    {
        // Load letter, render HTML, convert to PDF
        var html = await RenderLetterAsync(Guid.Empty, Guid.Empty);
        
        // Use QuestPDF or similar
        return await Task.FromResult(Array.Empty<byte>());
    }
}

public class CalendarService : ICalendarService
{
    public async Task<string> GenerateIcsAsync(Guid appointmentId)
    {
        // Use Ical.Net
        var calendar = new Ical.Net.Calendar();
        
        var evt = new Ical.Net.CalendarComponents.CalendarEvent
        {
            Start = new Ical.Net.DataTypes.CalDateTime(DateTime.Now),
            End = new Ical.Net.DataTypes.CalDateTime(DateTime.Now.AddHours(1)),
            Summary = "Appointment",
            Description = "Therapy appointment"
        };
        
        calendar.Events.Add(evt);
        
        var serializer = new Ical.Net.Serialization.CalendarSerializer();
        return await Task.FromResult(serializer.SerializeToString(calendar));
    }

    public async Task SendCalendarInviteAsync(Guid appointmentId)
    {
        var ics = await GenerateIcsAsync(appointmentId);
        
        // Send via email with ICS attachment
        await Task.CompletedTask;
    }
}
