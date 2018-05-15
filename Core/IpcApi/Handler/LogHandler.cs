using NLog;
using pixstock.apl.app.core.Infra;

namespace pixstock.apl.app.core.IpcApi.Handler {
    public class LogHandler : IRequestHandler
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public void Handle(object param)
        {
            _logger.Info("Execute LogHandler.Handle");
        }
    }
}
