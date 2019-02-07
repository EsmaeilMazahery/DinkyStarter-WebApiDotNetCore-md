using Dinky.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dinky.Domain.ViewModels
{
    public interface INotificationViewModel
    {
        NotificationType NotificationType { get; set; }
    }

    public class NotificationViewModel<T> : INotificationViewModel
    {
        public NotificationType NotificationType { get; set; }
        public T Payload { get; set; }
    }

    public class MessageRelatedPayload
    {
        public string Username { get; set; }
        public string Message { get; set; }
    }
}
