using Microsoft.AspNetCore.SignalR;

namespace CoreHub.Web.Hubs;

public class NotificationHub : Hub
{
    public async Task SendFlagNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveFlagNotification", message);
    }

    public async Task SendAppointmentUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveAppointmentUpdate", message);
    }

    public async Task JoinTenantGroup(string tenantId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"tenant_{tenantId}");
    }

    public async Task LeaveTenantGroup(string tenantId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"tenant_{tenantId}");
    }
}
