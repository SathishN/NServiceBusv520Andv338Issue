using System.Reflection;
using ITOps.Messages;
using ITOps.Messages.Commands;
using StructureMap;

namespace ITOps.Handlers 
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/profiles-for-nservicebus-host
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
	    public void Init()
	    {
            SetLoggingLibrary.Log4Net(log4net.Config.XmlConfigurator.Configure);

            ObjectFactory.Configure(cfg => { });

            Configure.With(new Assembly[] { typeof(NotificationCommand).Assembly, typeof(ConsoleNotificationHandler).Assembly })
                .DefineEndpointName("ITOps.Notification")
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.Contains("Messages.Messages"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.Contains("Messages.Events"))
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.Contains("Messages.Commands"))
                .Log4Net()
                .StructureMapBuilder()
                .XmlSerializer()
                .DisableTimeoutManager()
                .DisableSecondLevelRetries();


	    }
    }
}