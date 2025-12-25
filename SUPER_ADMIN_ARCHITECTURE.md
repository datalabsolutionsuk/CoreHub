# CoreHub - Super Admin Architecture

## Role Hierarchy

1. **SuperAdmin** (Highest - Full System Access)
   - Email: admin@corehub.com
   - Password: Admin@123
   - **Full CRUD access to everything**
   - Can view/edit/delete ALL data
   - Can manage users and roles
   - Can access audit logs
   - Can configure system settings
   - No restrictions whatsoever

2. **Admin**
   - Can view all cases and data
   - Can configure system settings
   - Limited user management

3. **Manager**
   - Can view team cases
   - Can manage team members

4. **Practitioner**
   - Can view own cases
   - Can manage own clients

5. **User** (Lowest)
   - Basic access only

## Authorization Policies

### Super Admin Exclusive Policies
- `SuperAdminOnly` - Only SuperAdmin role
- `CanManageUsers` - Create, edit, delete users
- `CanManageRoles` - Assign and remove roles
- `CanDeleteAnything` - Delete any data without restrictions
- `CanViewAuditLogs` - View system audit trails

### Shared Policies (SuperAdmin + Others)
- `AdminAccess` - SuperAdmin + Admin
- `CanViewAllCases` - SuperAdmin + Admin
- `CanViewTeamCases` - SuperAdmin + Admin + Manager
- `CanViewOwnCases` - SuperAdmin + Admin + Manager + Practitioner
- `CanConfigureSystem` - SuperAdmin + Admin

## Implementation Guide

### For Future Features:

When adding new features to CoreHub, **ALWAYS** ensure Super Admin has full access:

1. **UI Components** - Use `AuthorizeView` with role checks:
   ```razor
   <AuthorizeView Roles="@Roles.SuperAdmin">
       <Authorized>
           <!-- Super Admin sees all CRUD buttons -->
           <button>Add</button>
           <button>Edit</button>
           <button>Delete</button>
       </Authorized>
   </AuthorizeView>
   ```

2. **API Endpoints** - Add `[Authorize(Policy = Policies.SuperAdminOnly)]` or `[Authorize(Roles = Roles.SuperAdmin)]`

3. **Data Access** - SuperAdmin bypasses all filters:
   ```csharp
   if (currentUser.IsInRole(Roles.SuperAdmin))
   {
       // Return ALL data without filters
       return await _context.Data.ToListAsync();
   }
   else
   {
       // Apply user/team filters
       return await _context.Data.Where(d => d.UserId == currentUserId).ToListAsync();
   }
   ```

4. **Navigation** - Show admin-only menu items:
   ```razor
   <AuthorizeView Roles="@Roles.SuperAdmin">
       <NavLink href="/admin/users">User Management</NavLink>
       <NavLink href="/admin/roles">Role Management</NavLink>
       <NavLink href="/admin/audit">Audit Logs</NavLink>
   </AuthorizeView>
   ```

## Database Seeding

Super Admin is automatically created on first run:
- File: `ApplicationDbContextSeed.cs`
- Runs on application startup
- Creates all roles and assigns SuperAdmin to admin@corehub.com

## Constants Location

All roles and policies are defined in:
- `CoreHub.Web/Constants/AuthorizationConstants.cs`
- Use these constants instead of magic strings

## Testing

Login as Super Admin:
- Email: admin@corehub.com
- Password: Admin@123

The Super Admin should:
- ✅ See ALL clients from all users
- ✅ See ALL appointments
- ✅ Have Add/Edit/Delete buttons everywhere
- ✅ Access admin-only pages
- ✅ Bypass all data filters
- ✅ No "Access Denied" messages ever

## Future Enhancements

As we build more features, remember:
- Super Admin = **GOD MODE**
- No restrictions whatsoever
- Full visibility into everything
- Can modify/delete anything
- Always test new features as Super Admin
