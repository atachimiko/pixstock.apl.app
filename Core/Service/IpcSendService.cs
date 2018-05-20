using System;
using System.Linq;
using ElectronNET.API;
using Newtonsoft.Json;
using pixstock.apl.app.core.Infra;
using SimpleInjector;

namespace pixstock.apl.app.core.Service
{
    public class IpcSendService : IMessagingServiceExtention
    {
        public ServiceType ServiceType => ServiceType.FrontendIpc;

        public Container Container { get; set; }

        public void Execute(string message, object parameter)
        {
            Console.WriteLine("[IpcSendService][Execute]");
            Console.WriteLine("    Parameter=" + message);

            if (message == "UpdateView")
            {
                // IPCメッセージを作成する
                var ipcMessage = new IpcMessage();
                object obj = new
                {
                    UpdateList = parameter
                };
                ipcMessage.Body = JsonConvert.SerializeObject(obj, Formatting.Indented);
                Console.WriteLine("    Body(JSON)=" + ipcMessage.Body);

                var mainWindow = Electron.WindowManager.BrowserWindows.First();
                Electron.IpcMain.Send(mainWindow, "IPC_UPDATEVIEW", ipcMessage);
            }
        }

        public void InitializeExtention()
        {
            // EMPTY
        }
    }
}
