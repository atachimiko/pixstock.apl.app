using System;
using Hyperion.Pf.Workflow;
using Hyperion.Pf.Workflow.StateMachine.Eventsas;
using pixstock.apl.app.core.Infra;
using Pixstock.Applus.Foundations.ContentBrowser.Transitions;
using Pixstock.Core;
using SimpleInjector;

namespace pixstock.apl.app.Workflow
{
    public class PixstockMainContent : Hyperion.Pf.Workflow.Content, IPixstockContent
    {
        readonly CategoryTreeTransitionWorkflow mWorkflow;

        readonly Container mContainer;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <returns></returns>
        public PixstockMainContent(Container container) : base("PixstockMainContent")
        {
            this.mContainer = container;
            this.mWorkflow = new CategoryTreeTransitionWorkflow(container);
            mWorkflow.InvokeShowFrame += OnInvokeShowFrame;
            mWorkflow.InvokeHideFrame += OnInvokeHideFrame;
        }

        /// <summary>
        /// イベント名（文字列）からイベントを実行する
        /// </summary>
        /// <param name="workflowEvent"></param>
        public void FireWorkflowEvent(Container context, string workflowEvent, object param)
        {
            try
            {
                var events = Events.ForName(workflowEvent);
                mWorkflow.Fire(events, param);
            }
            catch (Exception expr)
            {
                Console.WriteLine("イベントが見つかりません " + expr.Message);
            }
        }

        /// <summary>
        /// ワークフローのUIパーツ表示指示イベントのハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnInvokeShowFrame(object sender, InvokeShowFrameEventArgs args)
        {
            try
            {
                Console.WriteLine("[PixstockMainContent][OnInvokeShowFrame] - IN");
                Console.WriteLine("   mContainer=" + this.mContainer);
                Console.WriteLine("   ScreenManager=" + this.ScreenManager);
                // ScreenManagerを呼び出して、表示処理を行う
                this.ScreenManager.ShowScreen(args.FrameName);
            }
            catch (Exception expr)
            {
                Console.WriteLine("Exception " + expr.Message);
            }
        }

        /// <summary>
        /// ワークフローのUIパーツ非表示指示イベントのハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnInvokeHideFrame(object sender, InvokeHideFrameEventArgs args)
        {
            try
            {
                Console.WriteLine("[PixstockMainContent][OnInvokeHideFrame] - IN");
                // ScreenManagerを呼び出して、非表示処理を行う
                this.ScreenManager.HideScreen(args.FrameName);
            }
            catch (Exception expr)
            {
                Console.WriteLine("Exception " + expr.Message);
            }
        }

        IScreenManager ScreenManager => this.mContainer.GetInstance<IScreenManager>();

        public override void OnDestroy()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnDiscontinue()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnEnd()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnIdle()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnInitialize()
        {
            //throw new System.NotImplementedException();
            mWorkflow.Setup();
        }

        public override bool OnPreResume()
        {
            //throw new System.NotImplementedException();
            return true;
        }

        public override bool OnPreStop()
        {
            //throw new System.NotImplementedException();
            return true;
        }

        public override void OnRestart()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnResume()
        {
            mWorkflow.Start();
            CompleteStart();
        }

        public override void OnRun()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnStop()
        {
            mWorkflow.Stop();
            CompleteStop();
        }

        public override void OnSuspend()
        {
            //throw new System.NotImplementedException();
        }
    }

}
