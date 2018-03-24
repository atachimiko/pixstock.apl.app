using System;
using System.Collections;
using System.Collections.Generic;
using ElectronNET.API;
using Newtonsoft.Json;
using pixstock.apl.app.core.Api.Response;
using pixstock.apl.app.Models;

namespace pixstock.apl.app.core
{
    public class ContentMainWorkflowEventEmiter
    {
        public void Initialize()
        {
            Electron.IpcMain.OnSync("EAV_GETCATEGORY", (args) =>
            {
                Console.WriteLine("[ContentMainWorkflowEventEmiter][EAV_GETCATEGORY] : IN " + args);

                var response = new CategoryDetailResponse();
                var category = new Category() { Id = 1, Name = "CategoryName" };
                var content1 = new List<Content>();

                for (int i = 1; i < 100; i++)
                {
                    content1.Add(new Content { Id = i, Name = "Content" + i });
                }

                response.Category = category;
                response.Content = content1.ToArray();

                return JsonConvert.SerializeObject(response);
            });
        }

        public void Dispose()
        {
            Electron.IpcMain.RemoveAllListeners("EAV_GETCATEGORY");
        }
    }
}