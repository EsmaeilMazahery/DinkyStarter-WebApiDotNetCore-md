using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Dinky.WebApi.Notifications
{
    [Authorize]
    public class NotificationsHub : Hub { }
}