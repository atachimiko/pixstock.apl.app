using System;
using System.Collections.Generic;
using Hyperion.Pf.Workflow;
using pixstock.apl.app.core.Infra;
using SimpleInjector;
using pixstock.apl.app.Workflow;

namespace pixstock.apl.app.core.Service
{
    /// <summary>
    ///
    /// </summary>
    public class WorkflowService : IMessagingServiceExtention
    {
        public ServiceType ServiceType => ServiceType.Workflow;

        readonly HarmonicManager mHarmonic;

        readonly Perspective mPixstockPerspective;

        public Container Container { get; set; }

        public WorkflowService()
        {
            mHarmonic = new HarmonicManager();

            var dictPerspective = new Dictionary<string, string>();
            dictPerspective.Add("MainFrame", "PixstockMainContent");
            mPixstockPerspective = new Perspective("PIXSTOCK", ArbitrationMode.AWAB, dictPerspective, mHarmonic);
        }

        public void Execute(string intentMessage, object parameter)
        {
            Console.WriteLine("[WorkflowService][Execute]");
            Console.WriteLine(" intentMessage = " + intentMessage);
            Console.WriteLine(" parameter = " + parameter);

            if (mPixstockPerspective.Status == PerspectiveStatus.Active)
            {
                foreach (var content in mPixstockPerspective.Contents)
                {
                    if (content is IPixstockContent)
                    {
                        ((IPixstockContent)content).FireWorkflowEvent(Container, intentMessage, parameter);

                        var screenManager = Container.GetInstance<IScreenManager>();
                        screenManager.UpdateScreenTransitionView();
                    }
                }
            }
        }

        public void InitializeExtention()
        {
            List<IContentBuilder> contentBuilders = new List<IContentBuilder>();
            contentBuilders.Add(new MyContentBuilder() { Container = this.Container });

            // Harmonicマネージャの初期化
            //    ・Contentの登録も行う
            //    ・Perspectiveの登録も行う
            mHarmonic.Verify(contentBuilders);
            mHarmonic.RegisterPerspective(mPixstockPerspective);
            mHarmonic.StartPerspective("PIXSTOCK"); // 開発中のみ、ここでPerspectiveを開始する
        }
    }

    class MyContentBuilder : IContentBuilder
    {
        public Container Container { get; set; }

        public Content Build()
        {
            return new PixstockMainContent(Container);
        }
    }
}
