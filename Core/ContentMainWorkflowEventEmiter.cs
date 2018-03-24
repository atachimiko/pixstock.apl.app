using System;
using ElectronNET.API;

namespace pixstock.apl.app.core
{
    public class ContentMainWorkflowEventEmiter
    {
        public void Initialize()
        {
            Electron.IpcMain.OnSync("EAV_GETCATEGORY", (args) =>
            {
                Console.WriteLine("[ContentMainWorkflowEventEmiter][EAV_GETCATEGORY] : IN " + args);

                return 9999;
            });
        }
    }
}