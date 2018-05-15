// using System.Linq;
// using ElectronNET.API;
// using NLog;
// using pixstock.apl.app.core.Infra;
// using SimpleInjector;

// namespace pixstock.apl.app.core.IpcApi
// {
//     public class IpcBridge
//     {
//         const string IPCEXTENTION_NAMESPACE = "pixstock.apl.app.core.IpcHandler";

//         private static Logger _logger = LogManager.GetCurrentClassLogger();

//         readonly Container mContainer;

//         /// <summary>
//         ///
//         /// </summary>
//         /// <param name="container"></param>
//         public IpcBridge(Container container)
//         {
//             this.mContainer = container;
//         }

//         /// <summary>
//         /// Ipcハンドラの初期化
//         /// </summary>
//         public RequestHandlerFactory Initialize()
//         {
//             _logger.Info("[IpcBridge][Initialize] - IN");
//             RequestHandlerFactory requestHandlerFactory = new RequestHandlerFactory(mContainer);
//             Container localContainer = new Container();

//             var repositoryAssembly = typeof(IpcBridge).Assembly;

//             var registrations =
//                 from type in repositoryAssembly.GetExportedTypes()
//                 where type.Namespace == IPCEXTENTION_NAMESPACE
//                 where type.GetInterfaces().Any()
//                 select new { Service = type.GetInterfaces().Single(), Implementation = type };

//             foreach (var reg in registrations)
//             {
//                 _logger.Info("[IpcBridge][Initialize] Register");

//                 // Interface => IIpcExtentionクラス
//                 localContainer.Register(reg.Service, reg.Implementation, Lifestyle.Transient);
//             }

//             localContainer.Verify();

//             foreach (var ext in localContainer.GetAllInstances<IIpcExtention>())
//             {
//                 requestHandlerFactory.Add(ext.IpcMessageName, ext.RequestHandler);
//                 Electron.IpcMain.On(ext.IpcMessageName, (param) =>
//                 {
//                     var factory = mContainer.GetInstance<IRequestHandlerFactory>();
//                     var handler = factory.CreateNew(ext.IpcMessageName);

//                     handler.Handle(param);
//                 });
//             }

//             return requestHandlerFactory;
//         }
//     }
// }
