using System;
using System.Collections.Generic;
using System.Linq;
using pixstock.apl.app.core.Infra;
using pixstock.apl.app.core.IpcApi;
using SimpleInjector;

namespace pixstock.apl.app.core
{
    public class ServiceDistoributionManager : IServiceDistoributor
    {
        const string SERVICEEXTENTION_NAMESPACE = "pixstock.apl.app.core.Service";

        readonly Dictionary<ServiceType, IMessagingServiceExtention> mServiceInstanceDict = new Dictionary<ServiceType, IMessagingServiceExtention>();

        readonly Container mContext;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ServiceDistoributionManager(Container context)
        {
            this.mContext = context;
        }

        public void Initialize()
        {
            var repositoryAssembly = typeof(ServiceDistoributionManager).Assembly;

            // IPCメッセージ処理プラグイン登録
            // IIpcExtentionインターフェースを実装するクラスのみ登録可能とする
            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace == SERVICEEXTENTION_NAMESPACE
                where type.GetInterfaces().Contains(typeof(IMessagingServiceExtention))
                select new { Service = type.GetInterfaces().Single(), Implementation = type };

            List<Type> implementationList = new List<Type>();
            foreach (var reg in registrations)
            {
                Console.WriteLine("[ServiceDistoributionManager][Initialize] Register " + reg.Implementation.Name);

                var instance = Activator.CreateInstance(reg.Implementation) as IMessagingServiceExtention;
                instance.Container = mContext;
                mServiceInstanceDict.Add(instance.ServiceType, instance);

                instance.InitializeExtention();
            }
        }

        public void ExecuteService(ServiceType service, string intentName, object parameter)
        {
            var serviceObj = mServiceInstanceDict[service];
            serviceObj.Execute(intentName, parameter);
        }
    }
}
