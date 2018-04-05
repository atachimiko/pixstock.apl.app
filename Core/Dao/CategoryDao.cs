using System;
using System.Linq;
using System.Collections.Generic;
using Pixstock.Base.AppIf.Sdk;
using RestSharp;
using pixstock.apl.app.Models;
using NLog;

namespace pixstock.apl.app.core.Dao
{
    /// <summary>
    /// 
    /// </summary>
    public class CategoryDao
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        const string BASEURL = "http://localhost:5080/aapi";

        readonly RestClient mClient;

        public CategoryDao()
        {
            mClient = new RestClient(BASEURL);
        }

        /// <summary>
        /// カテゴリ情報を読み込みます
        /// </summary>
        /// <param name="categoryId">カテゴリID</param>
        /// <returns>カテゴリ情報</returns>
        public Category LoadCategory(long categoryId)
        {

            var request = new RestRequest("category/{id}", Method.GET);
            request.AddUrlSegment("id", categoryId);
            request.AddQueryParameter("lla_order", "NAME_ASC");

            var response = mClient.Execute<ResponseAapi<Category>>(request);
            if (!response.IsSuccessful)
            {
                Console.WriteLine("ErrorCode=" + response.StatusCode);
                Console.WriteLine("ErrorException=" + response.ErrorException);
                Console.WriteLine("ErrorMessage=" + response.ErrorMessage);
                Console.WriteLine("ContentError=" + response.Data.Error);
                return null;
            }

            var category = response.Data.Value;
            category.LinkSubCategoryList = LinkGetSubCategory(categoryId, 0, response);
            category.LinkContentList = LinkGetContentList(categoryId, 0, response);
            return category;
        }

        private List<Content> LinkGetContentList(long categoryId, long offset, IRestResponse<ResponseAapi<Category>> response)
        {

            // リンク情報から、コンテント情報を取得する
            var contentList = new List<Content>();
            var link_la = response.Data.Link["la"] as List<object>;
            foreach (var content_id in link_la.Select(p => (long)p))
            {
                //Console.WriteLine("Request LinkType=la = " + category_id + ":" + content_id);
                var request_link_la = new RestRequest("category/{id}/la/{content_id}", Method.GET);
                request_link_la.AddUrlSegment("id", categoryId);
                request_link_la.AddUrlSegment("content_id", content_id);
                request_link_la.AddQueryParameter("offset", offset.ToString());

                var response_link_la = mClient.Execute<ResponseAapi<Content>>(request_link_la);
                if (response_link_la.IsSuccessful)
                {
                    //Console.WriteLine("Link[la]のコンテント読み込み=" + response_link_la.Data.Value);
                    var content = response_link_la.Data.Value;
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

            return contentList;
        }

        private List<Category> LinkGetSubCategory(long categoryId, long offset, IRestResponse<ResponseAapi<Category>> response)
        {
            // リンク情報から、カテゴリ情報を取得する
            List<Category> categoryList = new List<Category>();
            var link_la = response.Data.Link["cc"] as List<object>;
            foreach (var category_id in link_la.Select(p => (long)p))
            {
                var request_link_la = new RestRequest("category/{id}/cc/{category_id}", Method.GET);
                request_link_la.AddUrlSegment("id", categoryId);
                request_link_la.AddUrlSegment("category_id", category_id);
                request_link_la.AddQueryParameter("offset", offset.ToString());

                var response_link_la = mClient.Execute<ResponseAapi<Category>>(request_link_la);
                if (response_link_la.IsSuccessful)
                {
                    //Console.WriteLine("Link[la]のコンテント読み込み=" + response_link_la.Data.Value);
                    categoryList.Add(response_link_la.Data.Value);
                }
            }

            return categoryList;
        }
    }
}