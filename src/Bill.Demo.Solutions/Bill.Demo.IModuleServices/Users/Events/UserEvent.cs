using Surging.Core.CPlatform.EventBus.Events;

namespace Bill.Demo.IModuleServices.Users.Events
{
    public class UserEvent : IntegrationEvent
    {
        public string UserId { get; set; }

        public string  Name{ get; set; }

        public string Age { get; set; }
    }
}