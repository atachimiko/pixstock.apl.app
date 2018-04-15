using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ElectronNET.API;
using Newtonsoft.Json;
using NLog;
using pixstock.apl.app.core.Dao;
using pixstock.apl.app.core.IpcApi.Response;
using pixstock.apl.app.Models;
using Pixstock.Base.AppIf.Sdk;
using RestSharp;

namespace pixstock.apl.app.core
{
    public class ContentMainWorkflowEventEmiter
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        //static string BASEURL = "http://localhost:5080/aapi";

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            Electron.IpcMain.OnSync("EAV_GETCATEGORY", OnEAV_GETCATEGORY);
            Electron.IpcMain.OnSync("EAV_GETCONTENT", OnEAV_GETCONTENT);
        }

        public void Dispose()
        {
            Electron.IpcMain.RemoveAllListeners("EAV_GETCATEGORY");
            Electron.IpcMain.RemoveAllListeners("EAV_GETCONTENT");
        }

        private string OnEAV_GETCATEGORY(object args)
        {
            long categoryId = long.Parse(args.ToString());

            var dao_cat = new CategoryDao();
            var category = dao_cat.LoadCategory(categoryId);

            // IPCレスポンス作成
            var response = new CategoryDetailResponse();
            response.Category = category;
            response.SubCategory = category.LinkSubCategoryList.ToArray();
            response.Content = category.LinkContentList.ToArray();
            return JsonConvert.SerializeObject(response);
        }

        private string OnEAV_GETCONTENT(object args)
        {
            var contentId = long.Parse(args.ToString());
            var content = new Content { Id = contentId, Name = "Content" + contentId }; // DEBUG:開発中につきダミーデータを返す

            var response = new ContentDetailResponse();
            response.Content = content;
            return JsonConvert.SerializeObject(response);
        }
    }
}