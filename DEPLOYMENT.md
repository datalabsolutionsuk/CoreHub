# CoreHub - Production Deployment Checklist

## Pre-Deployment

### 1. Database Setup
- [ ] Provision production SQL Server instance
- [ ] Update connection string in `appsettings.Production.json`
- [ ] Run migrations: `dotnet ef database update --startup-project src/CoreHub.Web`
- [ ] Create database backups schedule
- [ ] Set up read replicas (optional, for reporting)

### 2. Security Configuration
- [ ] Generate strong JWT secret key (min 256 bits)
- [ ] Configure HTTPS certificate
- [ ] Enable HSTS with long max-age
- [ ] Set up CSP headers
- [ ] Configure CORS for specific origins only
- [ ] Enable rate limiting
- [ ] Review and harden authentication settings
- [ ] Set up 2FA for all admin accounts
- [ ] Configure password policies (complexity, rotation)

### 3. Email/SMS Setup
- [ ] Configure SMTP server credentials
- [ ] Set up Twilio account and credentials
- [ ] Test email delivery
- [ ] Test SMS delivery
- [ ] Configure email templates
- [ ] Set up SPF/DKIM records for email domain

### 4. File Storage
- [ ] Set up Azure Blob Storage account (or S3)
- [ ] Configure connection strings
- [ ] Test file upload/download
- [ ] Configure CDN (optional)
- [ ] Set up blob lifecycle policies

### 5. Monitoring & Logging
- [ ] Configure Seq or Application Insights
- [ ] Set up log retention policies
- [ ] Configure alerting rules
- [ ] Set up performance monitoring
- [ ] Configure health check endpoints
- [ ] Set up uptime monitoring (Pingdom, StatusCake)

### 6. Background Jobs (Hangfire)
- [ ] Configure Hangfire dashboard authorization
- [ ] Set up recurring job schedules
- [ ] Configure retry policies
- [ ] Monitor job execution

### 7. Environment Variables
Set the following in production:

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Server=...
Jwt__Key=your-production-secret-key
Jwt__Issuer=CoreHub
Jwt__Audience=CoreHubUsers
Email__SmtpServer=smtp.production.com
Email__Username=...
Email__Password=...
Twilio__AccountSid=...
Twilio__AuthToken=...
AzureStorage__ConnectionString=...
Serilog__SeqUrl=https://seq.production.com
```

## Deployment Steps

### Option 1: Docker Deployment

1. Build image:
   ```bash
   docker build -t corehub:1.0.0 .
   ```

2. Push to registry:
   ```bash
   docker tag corehub:1.0.0 yourregistry/corehub:1.0.0
   docker push yourregistry/corehub:1.0.0
   ```

3. Deploy to production:
   ```bash
   docker-compose -f docker-compose.production.yml up -d
   ```

### Option 2: Azure App Service

1. Publish:
   ```bash
   dotnet publish src/CoreHub.Web/CoreHub.Web.csproj -c Release -o ./publish
   ```

2. Deploy via Azure CLI:
   ```bash
   az webapp up --name corehub-prod --resource-group corehub-rg
   ```

### Option 3: Kubernetes

1. Apply manifests:
   ```bash
   kubectl apply -f k8s/
   ```

2. Verify deployment:
   ```bash
   kubectl get pods -n corehub
   kubectl logs -f deployment/corehub-web -n corehub
   ```

## Post-Deployment

### 1. Verification
- [ ] Verify application is running
- [ ] Check health endpoints: `/health`
- [ ] Test login with admin account
- [ ] Verify database connectivity
- [ ] Test email sending
- [ ] Test SMS sending
- [ ] Verify file upload works
- [ ] Check Hangfire dashboard
- [ ] Verify SignalR connections
- [ ] Test API endpoints

### 2. Data Migration
- [ ] Run seed tool to create initial tenant
- [ ] Import existing client data (if applicable)
- [ ] Import historical measures data
- [ ] Verify data integrity

### 3. User Onboarding
- [ ] Create admin accounts
- [ ] Create practitioner accounts
- [ ] Set up teams and roles
- [ ] Configure programs and subsites
- [ ] Import measure library
- [ ] Configure flag rules
- [ ] Set up letter templates
- [ ] Configure data quality requirements

### 4. Training
- [ ] Train admin users
- [ ] Train practitioners
- [ ] Provide user documentation
- [ ] Set up support channels

### 5. Monitoring Setup
- [ ] Configure application monitoring
- [ ] Set up error alerting
- [ ] Configure performance baselines
- [ ] Set up usage analytics

## Security Hardening

### Application Level
- [ ] Disable unnecessary endpoints
- [ ] Remove development tools from production
- [ ] Configure secure cookie settings
- [ ] Enable anti-forgery token validation
- [ ] Configure request size limits
- [ ] Set up IP whitelisting (if needed)

### Infrastructure Level
- [ ] Configure firewall rules
- [ ] Set up WAF (Web Application Firewall)
- [ ] Enable DDoS protection
- [ ] Configure VNet/private endpoints
- [ ] Set up backup encryption
- [ ] Configure Azure Key Vault for secrets

### Compliance
- [ ] Complete security assessment
- [ ] Perform penetration testing
- [ ] Document data flows
- [ ] Create privacy policy
- [ ] Set up GDPR workflows
- [ ] Configure audit log retention
- [ ] Create incident response plan

## Maintenance

### Daily
- [ ] Check application health
- [ ] Review error logs
- [ ] Monitor performance metrics

### Weekly
- [ ] Review security logs
- [ ] Check backup status
- [ ] Review failed jobs
- [ ] Monitor disk usage

### Monthly
- [ ] Review audit logs
- [ ] Update dependencies
- [ ] Review user access
- [ ] Check certificate expiration
- [ ] Review and optimize database

### Quarterly
- [ ] Security review
- [ ] Performance review
- [ ] Disaster recovery drill
- [ ] Update documentation

## Rollback Plan

If deployment fails:

1. Stop new deployment
2. Revert to previous container/version
3. Verify rollback successful
4. Investigate failure
5. Fix and redeploy

## Support Contacts

- **Technical Lead**: [email]
- **DevOps**: [email]
- **Security**: [email]
- **On-Call**: [phone]

---

**Last Updated**: December 2024
**Version**: 1.0.0
