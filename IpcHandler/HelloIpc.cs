using System;
using pixstock.apl.app.core.Infra;
using pixstock.apl.app.core.IpcApi.Handler;

namespace pixstock.apl.app.core.IpcHandler
{
    public class HelloIpc : IIpcExtention
    {
        public string IpcMessageName => "HELLO";

        public Type RequestHandler => typeof(LogHandler);
    }
}
