using System;
using pixstock.apl.app.core.Infra;

namespace pixstock.apl.app.core.IpcApi.Handler {
    /// <summary>
    /// ログ出力用のIPCハンドラ
    /// </summary>
    public class LogHandler : IRequestHandler
    {
        public void Handle(object param)
        {
            Console.WriteLine("Execute LogHandler.Handle");
        }
    }
}
