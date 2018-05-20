using System;
using pixstock.apl.app.core.Infra;
using SimpleInjector;

namespace pixstock.apl.app.core.Service
{
    public class ServerMessageService : IMessagingServiceExtention
    {
        public ServiceType ServiceType => ServiceType.Server;

        public Container Container { get; set; }

        public void Execute(string intentMessage, object parameter)
        {
            Console.WriteLine("[ServerMessageService][Execute]");
        }

        public void InitializeExtention()
        {
            // EMPTY
        }
    }
}
