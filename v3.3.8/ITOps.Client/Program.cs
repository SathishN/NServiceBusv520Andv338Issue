using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ITOps.Messages.Commands;
using NServiceBus;

namespace ITOps.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            SetLoggingLibrary.Log4Net(log4net.Config.XmlConfigurator.Configure);

            var bus = 
                Configure.With(new [] { typeof(NotificationCommand).Assembly })
                   .DefineEndpointName("ITOps.Notification.Client")
                   .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.Contains("Messages.Messages"))
                   .DefiningEventsAs(t => t.Namespace != null && t.Namespace.Contains("Messages.Events"))
                   .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.Contains("Messages.Commands"))
                   .Log4Net()
                   .StructureMapBuilder()
                   .MsmqTransport()
                   .XmlSerializer()
                   .IsTransactional(true)
                   .UnicastBus()
                   .SendOnly();

            Console.WriteLine("Type any button to send an notification , q to Quit.");

            while (Console.ReadLine() != "q")
            {
                var notification = new NotificationCommand() {Message = "Billing complete"};
                bus.Send(notification);
                Console.WriteLine("Notification sent.");

                Console.WriteLine("Type any button to send an notification , q to Quit.");
            }
        }
    }
}
