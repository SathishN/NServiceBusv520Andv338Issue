using System.Reflection;
using ITOps.Messages;
using ITOps.Messages.Commands;
using NServiceBus.Features;
using NServiceBus.Logging;
using StructureMap;
using NServiceBus;

namespace ITOps.Handlers 
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/profiles-for-nservicebus-host
	*/
    public class EndpointConfig : AsA_Server, IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration config)
        {
            LogManager.Use<NServiceBus.Log4Net.Log4NetFactory>();

            var container = new StructureMap.Container(cfg => { });

            config.AssembliesToScan(new [] {typeof (NotificationCommand).Assembly, typeof (ConsoleNotificationHandler).Assembly});

            config.EndpointName("ITOps.Notification");
            config.Conventions()
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.Contains("Messages.Messages"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.Contains("Messages.Events"))
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.Contains("Messages.Commands"));
            
            //configuration.UseContainer<NServiceBus.StructureMapBuilder>(cfg => cfg.ExistingContainer(container));
            config.UsePersistence<InMemoryPersistence>();
            config.UseSerialization<XmlSerializer>();
            config.DisableFeature<TimeoutManager>();
            config.DisableFeature<SecondLevelRetries>();
        }
    }
}