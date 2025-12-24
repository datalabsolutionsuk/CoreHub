# CoreHub

**CoreHub** is a production-ready therapy outcomes tracking system built with .NET 9, Blazor Server, and clean architecture principles. It clones and improves upon systems like CORE Net, providing comprehensive client management, outcomes measurement, progress tracking, and clinical workflow support.

---

## 🎯 Features

### Core Functionality
- **Client Management**: Full caseload management with demographics, status tracking, and data quality scoring
- **Outcome Measures**: CORE-10, CORE-OM, PHQ-9, GAD-7, and custom questionnaires with automated scoring
- **Progress Visualization**: Time-series charts, scatter plots (first vs. last score), item-level analysis
- **Clinical Flags**: Automated risk detection, off-track alerts, needs-closing reminders
- **Sessions & Appointments**: Calendar views, reminders (email/SMS), ICS exports, DNA tracking
- **Case Notes**: Structured clinical notes with categories, search, and locking
- **Letters & Templates**: Merge fields, PDF generation, batch printing
- **Reports & KPIs**: Improvement rates, caseload stats, discharge outcomes, activity reports
- **Data Quality**: Configurable required fields, thermometer visualization (0-5 scale)

### Technical Features
- **Multi-Tenancy**: Row-level security with TenantId on all tables
- **Authentication**: ASP.NET Core Identity + JWT, TOTP 2FA, optional WebAuthn
- **Authorization**: Role-based (Admin, Manager, Practitioner, ReadOnly) + policy-based
- **Real-time**: SignalR for live flag updates and appointment notifications
- **Background Jobs**: Hangfire for scheduled reminders, report emails, maintenance tasks
- **Audit Trail**: Full PII change logging with old/new values
- **Soft Delete**: All entities support soft delete for GDPR compliance
- **API & UI**: RESTful API + Blazor Server UI with responsive design
- **Exports**: Excel (ClosedXML), PDF (QuestPDF), ICS (Ical.NET)
- **Observability**: Serilog, OpenTelemetry, HealthChecks, Seq integration

---

## 🏗️ Architecture

### Solution Structure
```
CoreHub/
├── src/
│   ├── CoreHub.Domain/          # Entities, ValueObjects, Enums, Events
│   ├── CoreHub.Application/     # DTOs, MediatR handlers, Validators, Interfaces
│   ├── CoreHub.Infrastructure/  # EF Core, Identity, Services (Email, SMS, Export)
│   ├── CoreHub.Web/            # Blazor Server UI, API Controllers, SignalR Hubs
│   └── CoreHub.Seed/           # CLI tool for demo data
├── tests/
│   └── CoreHub.Tests/          # xUnit, FluentAssertions, WebApplicationFactory
├── docker-compose.yml
├── Dockerfile
└── .github/workflows/build.yml
```

### Technology Stack
- **.NET 9** with C# 13
- **Blazor Server** for interactive UI
- **EF Core 9** with SQL Server (PostgreSQL-compatible mappings)
- **ASP.NET Core Identity** for authentication
- **MediatR** for CQRS pattern
- **FluentValidation** for input validation
- **Mapster** for object mapping
- **Hangfire** for background jobs
- **SignalR** for real-time updates
- **Serilog + Seq** for logging
- **Blazorise** with Bootstrap 5 for UI components
- **ClosedXML, QuestPDF, Ical.NET** for exports

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB, Express, or Docker)
- (Optional) Docker Desktop for containerized deployment

### Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/corehub.git
   cd corehub
   ```

2. **Update connection string**
   Edit `src/CoreHub.Web/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CoreHubDb;Trusted_Connection=true;..."
   }
   ```

3. **Apply migrations**
   ```bash
   cd src/CoreHub.Infrastructure
   dotnet ef database update --startup-project ../CoreHub.Web
   ```

4. **Seed demo data**
   ```bash
   cd ../CoreHub.Seed
   dotnet run
   ```

5. **Run the application**
   ```bash
   cd ../CoreHub.Web
   dotnet run
   ```

6. **Open browser**
   Navigate to `https://localhost:5001`

### Docker Deployment

```bash
docker-compose up -d
```

Access the application at `http://localhost:5000`

---

## 👤 Demo Credentials

After running the seed tool:

| Role        | Email                  | Password       |
|-------------|------------------------|----------------|
| Admin       | admin@demo.com         | Admin123!      |
| Manager     | manager@demo.com       | Manager123!    |
| Practitioner| therapist1@demo.com    | Therapist123!  |

---

## 📊 Database Migrations

### Create a new migration
```bash
cd src/CoreHub.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../CoreHub.Web
```

### Update database
```bash
dotnet ef database update --startup-project ../CoreHub.Web
```

### Generate SQL script
```bash
dotnet ef migrations script --startup-project ../CoreHub.Web -o migration.sql
```

---

## 🔌 API Endpoints

### Authentication
- `POST /api/auth/login` - Login with email/password
- `POST /api/auth/refresh` - Refresh JWT token
- `POST /api/auth/2fa/setup` - Setup TOTP 2FA
- `POST /api/auth/2fa/verify` - Verify 2FA code

### Clients
- `GET /api/clients` - List clients (filtered, paged)
- `GET /api/clients/{id}` - Get client details
- `POST /api/clients` - Create client
- `PUT /api/clients/{id}` - Update client
- `DELETE /api/clients/{id}` - Soft delete client
- `POST /api/clients/{id}/close` - Close/discharge client
- `GET /api/clients/{id}/dq` - Get data quality score

### Measures & Forms
- `GET /api/measures` - List available measures
- `GET /api/measures/{id}` - Get measure details with items
- `POST /api/measures/forms` - Create new form (session or remote)
- `POST /api/measures/forms/{id}/link` - Generate remote access link
- `GET /api/measures/forms/token/{token}` - Get form by token (public)
- `POST /api/measures/forms/{id}/submit` - Submit completed form

### Flags
- `GET /api/flags` - List flags (filtered by type, status, program)
- `POST /api/flags` - Raise a flag manually
- `POST /api/flags/{id}/clear` - Clear a flag with note

### Appointments
- `GET /api/appointments` - List appointments (calendar view)
- `POST /api/appointments` - Book appointment
- `PUT /api/appointments/{id}` - Update appointment
- `POST /api/appointments/{id}/cancel` - Cancel appointment

### Reports
- `POST /api/reports/kpi` - Generate KPI report
- `POST /api/reports/scatter` - Get scatter plot data
- `GET /api/reports/progress?clientId=...&measureId=...` - Progress chart
- `POST /api/reports/export` - Export to Excel/PDF

---

## 🎨 UI Pages

- **Dashboard** - KPI cards, recent flags, upcoming appointments
- **Clients** - Searchable table with DQ thermometer, status badges, quick actions
- **Client Profile** - Tabs: Overview, Measures, Sessions, Notes, Flags, Letters, Attachments
- **Forms** - Measure runner for client/practitioner mode, item-level charts
- **Flags** - Board view with filters, real-time updates via SignalR
- **Appointments** - Calendar (day/week), drag-drop, conflict detection
- **Reports** - KPI dashboard, scatter plot with drill-down, export buttons
- **Admin** - Users, roles, teams, programs, measures library, templates, settings, audit logs

---

## 🧪 Testing

### Run all tests
```bash
dotnet test
```

### Run with coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=opencover
```

### Integration tests
Uses `WebApplicationFactory` with in-memory database. See `tests/CoreHub.Tests/Integration/`.

---

## 🔒 Security

### Implemented
- ✅ ASP.NET Core Identity with lockout policies
- ✅ JWT authentication for API
- ✅ TOTP 2FA ready (setup in Identity UI)
- ✅ Role-based + policy-based authorization
- ✅ Row-level security via TenantId filters
- ✅ Soft delete for GDPR compliance
- ✅ Audit logging for all PII changes
- ✅ HTTPS enforcement, HSTS headers
- ✅ Anti-XSRF tokens on Blazor forms
- ✅ Input validation with FluentValidation
- ✅ SQL injection protection (EF Core parameterized queries)

### Recommended Additions
- [ ] Column-level encryption for sensitive fields (SSN, etc.)
- [ ] Rate limiting (AspNetCoreRateLimit)
- [ ] CSP headers (NWebsec)
- [ ] FIDO2/WebAuthn support
- [ ] Virus scanning for file uploads (ClamAV integration)
- [ ] Penetration testing
- [ ] GDPR data export/purge workflows

---

## 🔧 Configuration

### Email Settings
Edit `appsettings.json`:
```json
"Email": {
  "SmtpServer": "smtp.example.com",
  "SmtpPort": "587",
  "FromAddress": "noreply@corehub.com",
  "Username": "your-username",
  "Password": "your-password"
}
```

### SMS (Twilio)
```json
"Twilio": {
  "AccountSid": "your-account-sid",
  "AuthToken": "your-auth-token",
  "FromNumber": "+1234567890"
}
```

### Azure Blob Storage (Optional)
```json
"AzureStorage": {
  "ConnectionString": "DefaultEndpointsProtocol=https;...",
  "ContainerName": "corehub-documents"
}
```

---

## 📈 Roadmap

- [x] Core client and session management
- [x] Outcome measures with scoring
- [x] Flags and risk detection
- [x] Appointments and reminders
- [x] Reports and exports
- [ ] Client portal (view appointments, complete forms)
- [ ] SSO/OIDC integration
- [ ] FHIR API facade
- [ ] Mobile app (Xamarin/MAUI)
- [ ] Advanced analytics (ML-based risk prediction)
- [ ] Multi-language support (i18n)

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Code Style
- Follow Microsoft C# coding conventions
- Use `dotnet format` before committing
- Write unit tests for new features
- Update documentation

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- **CORE System** - Original inspiration for outcomes tracking
- **PHQ/GAD** - Standardized clinical measures
- **.NET Community** - For amazing libraries and tools
- **Bogus** - For realistic demo data generation

---

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/yourusername/corehub/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/corehub/discussions)
- **Email**: support@corehub.com

---

## 🔐 Threat Model Notes

### Assets
- **Client PII**: Names, DOB, contact info, clinical notes
- **Clinical Data**: Measure scores, risk assessments, diagnoses
- **User Credentials**: Passwords, 2FA secrets, API keys
- **System Configuration**: Tenant settings, flag rules, templates

### Threats
1. **Unauthorized Access**: Mitigated by Identity + JWT + 2FA
2. **Data Breach**: Mitigated by encryption, audit logs, soft delete
3. **SQL Injection**: Mitigated by EF Core parameterized queries
4. **XSS**: Mitigated by Blazor automatic encoding
5. **CSRF**: Mitigated by anti-forgery tokens
6. **Session Hijacking**: Mitigated by secure cookies, HTTPS
7. **Insider Threats**: Mitigated by audit logs, role separation

### Compliance
- **GDPR**: Consent flags, right-to-be-forgotten, data retention policies
- **HIPAA**: (if applicable) PHI encryption, audit trails, access controls
- **ISO 27001**: Recommended for production deployment

---

**Built with ❤️ using .NET 9 and Blazor Server**
