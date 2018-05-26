using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using pixstock.apl.app.core.Cache;
using pixstock.apl.app.core.Infra;
using pixstock.apl.app.core.IpcApi.Response;
using pixstock.apl.app.Models;
using SimpleInjector;

namespace Pixstock.Applus.Foundations.ContentBrowser.Transitions
{
    public partial class CategoryTreeTransitionWorkflow
    {
        readonly Container mContainer;

        public CategoryTreeTransitionWorkflow(Container container)
        {
            this.mContainer = container;
        }

        async Task OnHomePageBase_Entry()
        {

        }

        async Task OnHomePageBase_Exit()
        {

        }

        async Task OnThumbnailListPage_Entry()
        {

        }

        async Task OnThumbnailListPage_Exit()
        {

        }

        async Task OnREQUEST_GetCategory(object param)
        {
        }

        async Task OnACT_ContinueCategoryList(object param)
        {
        }

        async Task OnRESPONSE_GETCATEGORY(object param)
        {
            Console.WriteLine("[CategoryTreeTransitionWorkflow][OnRESPONSE_GETCATEGORY]");
            await Task.Delay(1);

            var intentManager = mContainer.GetInstance<IIntentManager>();
            var memCache = mContainer.GetInstance<IMemoryCache>();
            CategoryDetailResponse response;
            if (memCache.TryGetValue("ResponseCategory", out response))
            {
                memCache.Set("CategoryList", new CategoryListParam
                {
                    CategoryList = response.SubCategory
                });

                memCache.Set("ContentList", new ContentListParam
                {
                    ContentList = response.Content
                });

                intentManager.AddIntent(ServiceType.FrontendIpc, "UpdateProp", "CategoryList");
                intentManager.AddIntent(ServiceType.FrontendIpc, "UpdateProp", "ContentList");
            }
            else
            {
                Console.WriteLine("    MemCache TryGet Failer");
            }
        }

        async Task OnRESPONSE_GETCATEGORYCONTENT(object param)
        {
            Console.WriteLine("[CategoryTreeTransitionWorkflow][OnRESPONSE_GETCATEGORYCONTENT]");
            var intentManager = mContainer.GetInstance<IIntentManager>();
            var memCache = mContainer.GetInstance<IMemoryCache>();
            CategoryDetailResponse response;
            if (memCache.TryGetValue("ResponseCategoryContent", out response))
            {
                memCache.Set("ContentList", new ContentListParam
                {
                    ContentList = response.Content
                });

                intentManager.AddIntent(ServiceType.FrontendIpc, "UpdateProp", "ContentList");
            }
            else
            {
                Console.WriteLine("    MemCache TryGet Failer");
            }
        }

        async Task OnCategorySelectBtnClick(object param)
        {
            Console.WriteLine("[CategoryTreeTransitionWorkflow][OnCategorySelectBtnClick]");

            var intentManager = mContainer.GetInstance<IIntentManager>();
            var memCache = mContainer.GetInstance<IMemoryCache>();
            await Task.Run(() =>
            {
                CategoryDetailResponse response;
                if (memCache.TryGetValue("ResponseCategoryContent", out response))
                {
                    memCache.Set("ContentList", new ContentListParam
                    {
                        ContentList = response.Content
                    });

                    intentManager.AddIntent(ServiceType.FrontendIpc, "UpdateProp", "ContentList");
                }
                else
                {
                    Console.WriteLine("    MemCache TryGet Failer");
                }
            });
        }

        async Task OnACT_DISPLAY_PREVIEWCURRENTLIST(object param)
        {
            try
            {
                Console.WriteLine("[CategoryTreeTransitionWorkflow][OnACT_DISPLAY_PREVIEWCURRENTLIST]");
                Console.WriteLine("     param=" + param);
                var intentManager = mContainer.GetInstance<IIntentManager>();
                var memCache = mContainer.GetInstance<IMemoryCache>();

                ContentListParam objContentList;
                if (memCache.TryGetValue("ContentList", out objContentList))
                {
                    // コンテント一覧の項目位置(param)にあるコンテント情報を読み込む
                    var contentPosition = long.Parse(param.ToString());
                    var content = objContentList.ContentList[contentPosition];
                    intentManager.AddIntent(ServiceType.Server, "GETCONTENT", content.Id);
                }
                else
                {
                    throw new ApplicationException("ContentListプロパティを取得できませんでした");
                }
            }
            catch (Exception expr)
            {
                Console.WriteLine(expr.Message);
            }
        }

        async Task OnRESPONSE_GETCONTENT(object param)
        {
            Console.WriteLine("[CategoryTreeTransitionWorkflow][OnRESPONSE_GETCONTENT]");
            var intentManager = mContainer.GetInstance<IIntentManager>();
            var memCache = mContainer.GetInstance<IMemoryCache>();
            ContentDetailResponse response;
            if (memCache.TryGetValue("ResponsePreviewContent", out response))
            {
                memCache.Set("PreviewContent", response);

                intentManager.AddIntent(ServiceType.FrontendIpc, "UpdateProp", "PreviewContent");
            }
            else
            {
                Console.WriteLine("    MemCache TryGet Failer");
            }
        }

    }
}
