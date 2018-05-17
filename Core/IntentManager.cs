using System;
using System.Threading;
using System.Threading.Tasks;
using pixstock.apl.app.core.Infra;

namespace pixstock.apl.app.core
{
    public class IntentManager : IIntentManager
    {
        readonly IBackgroundTaskQueue mBackgroundTaskQueue;

        /// <summary>
        ///コンストラクタ
        /// </summary>
        /// <param name="queue"></param>
        public IntentManager(IBackgroundTaskQueue queue)
        {
            this.mBackgroundTaskQueue = queue;
        }

        public void AddIntent(string intentName)
        {
            mBackgroundTaskQueue.QueueBackgroundWorkItem(ExecuteItem);

            async Task ExecuteItem(CancellationToken token)
            {
                Console.WriteLine("[IntentManafer] IntentShori " + intentName);
                await Task.Delay(TimeSpan.FromSeconds(5), token);
            }
        }
    }
}
