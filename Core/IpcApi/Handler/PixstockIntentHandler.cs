using System;
using pixstock.apl.app.core.Infra;

namespace pixstock.apl.app.core.IpcApi.Handler
{
    /// <summary>
    /// PixstockのIntentメッセージとして処理するIPCハンドラ
    /// </summary>
    public class PixstockIntentHandler : IRequestHandler
    {
        readonly IIntentManager mIntentManager;

        public PixstockIntentHandler(IIntentManager intentManager)
        {
            this.mIntentManager = intentManager;
        }

        public void Handle(object param)
        {
            Console.WriteLine("Execute PixstockIntentHandler.Handle");

            //TODO: IPCメッセージから、PixstockのIntentメッセージを取り出す処理

            mIntentManager.AddIntent(param.ToString());
        }
    }
}
