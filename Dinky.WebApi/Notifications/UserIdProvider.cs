using Microsoft.AspNetCore.SignalR;

namespace Dinky.WebApi.Notifications
{
  public class UserIdProvider : IUserIdProvider
  {
    public string GetUserId(HubConnectionContext connection)
    {
      return connection.User.Identity.Name;
    }
  }
}