using EventBus.Messages.Events;
using MassTransit;
using System;

namespace Test
{
    public class Repo : IRepo
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public Repo(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public void msg()
        {
            UserRegistratedEvent userRegistrationEvent = new UserRegistratedEvent { Message = "fwet Repodan salam" };
            _publishEndpoint.Publish(userRegistrationEvent);
           
        }
    }
}
