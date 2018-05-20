using System;
using System.Threading;
using System.Threading.Tasks;
using pixstock.apl.app.core.Infra;

namespace pixstock.apl.app.core
{
    public class IntentManager : IIntentManager
    {
        readonly IBackgroundTaskQueue mBackgroundTaskQueue;

        readonly IServiceDistoributor mServiceDistoributor;

        /// <summary>
        ///コンストラクタ
        /// </summary>
        /// <param name="queue"></param>
        public IntentManager(IBackgroundTaskQueue queue, IServiceDistoributor distributor)
        {
            this.mBackgroundTaskQueue = queue;
            this.mServiceDistoributor = distributor;
        }

        public void AddIntent(ServiceType service, string intentName, object parameter)
        {
            mBackgroundTaskQueue.QueueBackgroundWorkItem(ExecuteItem);

            async Task ExecuteItem(CancellationToken token)
            {
                Console.WriteLine("[IntentManafer] IntentShori " + intentName);

                //await Task.Delay(TimeSpan.FromSeconds(5), token); //デバッグ用のウェイト

                // Distributorの呼び出し
                mServiceDistoributor.ExecuteService(service, intentName, parameter);
            }
        }
    }
}
