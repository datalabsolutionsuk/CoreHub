# CoreHub - Quick Start Guide

This guide will help you get CoreHub running locally in under 5 minutes.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB comes with Visual Studio, or use Docker)

## Step 1: Restore NuGet Packages

```bash
dotnet restore
```

## Step 2: Setup Database

The default configuration uses SQL Server LocalDB. If you have it installed:

```bash
cd src/CoreHub.Infrastructure
dotnet ef database update --startup-project ../CoreHub.Web
```

### Alternative: Use Docker for SQL Server

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 --name corehub-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

Then update connection string in `src/CoreHub.Web/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=CoreHubDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true"
}
```

And run migrations:

```bash
cd src/CoreHub.Infrastructure
dotnet ef database update --startup-project ../CoreHub.Web
```

## Step 3: Seed Demo Data

```bash
cd ../CoreHub.Seed
dotnet run
```

This creates:
- DemoCare tenant
- 3 demo users (admin, manager, therapist)
- 3 programs (Primary Care, Students, Workplace)
- CORE-10 measure with all items
- 50 demo clients with realistic data

## Step 4: Run the Application

```bash
cd ../CoreHub.Web
dotnet run
```

Or use `dotnet watch` for hot reload:

```bash
dotnet watch run
```

## Step 5: Access the Application

Open your browser and navigate to:

**https://localhost:5001**

### Login Credentials

| Role         | Email                | Password        |
|--------------|----------------------|-----------------|
| Admin        | admin@demo.com       | Admin123!       |
| Manager      | manager@demo.com     | Manager123!     |
| Practitioner | therapist1@demo.com  | Therapist123!   |

## What's Next?

### Explore the UI
- **Dashboard**: View KPIs and recent activity
- **Clients**: Browse the 50 demo clients
- **Flags**: See risk and off-track alerts
- **Appointments**: Create and manage appointments
- **Reports**: Generate KPI reports and scatter plots

### Try the API
Use the `api-requests.http` file with the REST Client extension in VS Code, or import into Postman.

1. Get a JWT token:
   ```http
   POST https://localhost:5001/api/auth/login
   Content-Type: application/json

   {
     "email": "admin@demo.com",
     "password": "Admin123!"
   }
   ```

2. Use the token in subsequent requests:
   ```http
   GET https://localhost:5001/api/clients
   Authorization: Bearer YOUR_TOKEN_HERE
   ```

### View Background Jobs
Access the Hangfire dashboard at:
**https://localhost:5001/hangfire**

### View Logs (if using Seq)
If you have Seq running locally:
**http://localhost:5341**

Or start Seq via Docker:
```bash
docker run -d --name seq -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest
```

## Troubleshooting

### Migration Fails
Make sure you're in the `CoreHub.Infrastructure` directory and using `--startup-project`:
```bash
cd src/CoreHub.Infrastructure
dotnet ef database update --startup-project ../CoreHub.Web
```

### Port Already in Use
Edit `src/CoreHub.Web/Properties/launchSettings.json` to use different ports.

### Cannot Connect to Database
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- For LocalDB, ensure it's installed with Visual Studio

### NuGet Restore Issues
Clear cache and restore:
```bash
dotnet nuget locals all --clear
dotnet restore
```

## Development Tips

### Hot Reload
```bash
cd src/CoreHub.Web
dotnet watch run
```

### Run Tests
```bash
dotnet test
```

### Add a Migration
```bash
cd src/CoreHub.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../CoreHub.Web
```

### Update Database
```bash
dotnet ef database update --startup-project ../CoreHub.Web
```

### View EF SQL
```bash
dotnet ef migrations script --startup-project ../CoreHub.Web
```

## Next Steps

1. Read the full [README.md](README.md) for detailed features
2. Review the [DEPLOYMENT.md](DEPLOYMENT.md) for production deployment
3. Explore the codebase starting with `src/CoreHub.Domain/Entities/`
4. Check out the API documentation in `api-requests.http`

Happy coding! 🚀
