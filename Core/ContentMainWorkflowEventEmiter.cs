using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ElectronNET.API;
using Newtonsoft.Json;
using NLog;
using pixstock.apl.app.core.IpcApi.Response;
using pixstock.apl.app.Models;
using Pixstock.Base.AppIf.Sdk;
using RestSharp;

namespace pixstock.apl.app.core
{
    public class ContentMainWorkflowEventEmiter
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static string BASEURL = "http://localhost:5080/aapi";

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            Electron.IpcMain.OnSync("EAV_GETCATEGORY", OnEAV_GETCATEGORY);

            // TODO: ↓別ハンドラ化する
            Electron.IpcMain.OnSync("EAV_GETCONTENT", (args) =>
            {
                Console.WriteLine("[ContentMainWorkflowEventEmiter][EAV_GETCONTENT] : IN " + args);
                var contentId = long.Parse(args.ToString());
                var content = new Content { Id = contentId, Name = "Content" + contentId };

                var response = new ContentDetailResponse();
                response.Content = content;

                return JsonConvert.SerializeObject(response);
            });
        }

        public void Dispose()
        {
            Electron.IpcMain.RemoveAllListeners("EAV_GETCATEGORY");
            Electron.IpcMain.RemoveAllListeners("EAV_GETCONTENT");
        }

        private string OnEAV_GETCATEGORY(object args)
        {
            _logger.Info("IN", args);
            long categoryId = long.Parse(args.ToString());

            // TODO: ↓ DAO化する
            string requestUrl = BASEURL;
            var client = new RestClient(requestUrl);
            var request = new RestRequest("category/{id}", Method.GET);
            request.AddUrlSegment("id", categoryId);

            var rest_response = client.Execute<ResponseAapi<Category>>(request);
            if (!rest_response.IsSuccessful)
            {
                Console.WriteLine("ErrorCode=" + rest_response.StatusCode);
                Console.WriteLine("ErrorException=" + rest_response.ErrorException);
                Console.WriteLine("ErrorMessage=" + rest_response.ErrorMessage);
                Console.WriteLine("ContentError=" + rest_response.Data.Error);
                return null;
            }

            var category = rest_response.Data.Value;

            // リンク情報から、コンテント情報を取得する
            var contentList = new List<Content>();
            var link_la = rest_response.Data.Link["la"] as List<object>;
            foreach (var content_id in link_la.Select(p => (long)p))
            {
                //Console.WriteLine("Request LinkType=la = " + category_id + ":" + content_id);
                var request_link_la = new RestRequest("category/{id}/la/{content_id}", Method.GET);
                request_link_la.AddUrlSegment("id", categoryId);
                request_link_la.AddUrlSegment("content_id", content_id);

                var response_link_la = client.Execute<ResponseAapi<Content>>(request_link_la);
                if (response_link_la.IsSuccessful)
                {
                    var content = response_link_la.Data.Value;
                    _logger.Info("Link[la]のコンテント読み込み=" + content);

                    // サムネイルが存在する場合は、サムネイルのURLを設定
                    if (!string.IsNullOrEmpty(content.ThumbnailKey))
                    {
                        _logger.Info("コンテントサムネイルの読み込み=" + content.ThumbnailKey);
                        content.ThumbnailImageSrcUrl = BASEURL + "/thumbnail/" + content.ThumbnailKey;
                    }

                    // コンテントのURLを設定
                    content.PreviewFileUrl = BASEURL + "/artifact/" + content.Id + "/preview";

                    contentList.Add(content);
                }
            }
            // ↑ここまで。

            // IPCレスポンス作成
            var response = new CategoryDetailResponse();
            response.Category = category;
            response.Content = contentList.ToArray();
            return JsonConvert.SerializeObject(response);
        }
    }
}