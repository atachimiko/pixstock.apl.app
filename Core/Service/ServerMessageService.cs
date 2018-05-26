using System;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using pixstock.apl.app.core.Dao;
using pixstock.apl.app.core.Infra;
using pixstock.apl.app.core.IpcApi.Response;
using pixstock.apl.app.json.ServerMessage;
using SimpleInjector;

namespace pixstock.apl.app.core.Service
{
    public class ServerMessageService : IMessagingServiceExtention
    {
        public ServiceType ServiceType => ServiceType.Server;

        public Container Container { get; set; }

        public void Execute(string intentMessage, object parameter)
        {
            try
            {
                Console.WriteLine("[ServerMessageService][Execute] " + intentMessage);
                Console.WriteLine("[ServerMessageService][Execute] parameter = " + parameter);
                var memCache = Container.GetInstance<IMemoryCache>();
                var intentManager = Container.GetInstance<IIntentManager>();

                if (intentMessage == "GETCATEGORY")
                {
                    var param = JsonConvert.DeserializeObject<GetCategoryParam>(parameter.ToString());

                    var dao_cat = new CategoryDao();
                    var category = dao_cat.LoadCategory(param.CategoryId, param.OffsetSubCategory, param.LimitOffsetSubCategory);

                    var response = new CategoryDetailResponse();
                    response.Category = category;
                    response.SubCategory = category.LinkSubCategoryList.ToArray();
                    response.Content = category.LinkContentList.ToArray();

                    memCache.Set("ResponseCategory", response);

                    intentManager.AddIntent(ServiceType.Workflow, "RESPONSE_GETCATEGORY", null);
                }
                else if (intentMessage == "GETCATEGORYCONTENT")
                {
                    var categoryId = long.Parse(parameter.ToString());

                    var dao_cat = new CategoryDao();
                    var category = dao_cat.LoadCategory(categoryId, 0, CategoryDao.MAXLIMIT);

                    var response = new CategoryDetailResponse();
                    response.Content = category.LinkContentList.ToArray();

                    memCache.Set("ResponseCategoryContent", response);

                    intentManager.AddIntent(ServiceType.Workflow, "RESPONSE_GETCATEGORYCONTENT", null);
                }
                else if (intentMessage == "GETCONTENT")
                {
                    var contentId = long.Parse(parameter.ToString());

                    var dao_content = new ContentDao();
                    var content = dao_content.LoadContent(contentId);

                    var response = new ContentDetailResponse()
                    {
                        Content = content,
                        Category = content.LinkCategory
                    };

                    memCache.Set("ResponsePreviewContent", response);

                    intentManager.AddIntent(ServiceType.Workflow, "RESPONSE_GETCONTENT", null);
                }
                else
                {
                    Console.WriteLine("Unknown MessageName " + intentMessage);
                }
            }
            catch (Exception expr)
            {
                Console.WriteLine(expr.Message);
            }
        }

        public void InitializeExtention()
        {
            // EMPTY
        }
    }
}
