using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hyperion.Pf.Workflow;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Infrastructure;
using Appccelerate.StateMachine.Machine;
using Appccelerate.StateMachine.Persistence;
using Appccelerate.StateMachine.Reports;
using Hyperion.Pf.Workflow.StateMachine;

using Pixstock.Core;
namespace Pixstock.Applus.Foundations.ContentBrowser.Transitions {
public partial class CategoryTreeTransitionWorkflow : FrameStateMachine<States, Events>, IAsyncPassiveStateMachine {
	public static string Name = "Pixstock.ContentBrowser.CategoryTreeTransitionWorkflow";
public void Setup() {
DefineHierarchyOn(States.ROOT)
.WithHistoryType(HistoryType.None)
.WithInitialSubState(States.HomePage)
;
DefineHierarchyOn(States.HomePage)
.WithHistoryType(HistoryType.None)
.WithInitialSubState(States.HomePageBase)
.WithSubState(States.ThumbnailListPage)
;
DefineHierarchyOn(States.ThumbnailListPage)
.WithHistoryType(HistoryType.None)
.WithInitialSubState(States.ThumbnailListPageBase)
.WithSubState(States.PreviewPage)
;
In(States.INIT)
.On(Events.TRNS_TOPSCREEN)
.Goto(States.HomePage);
In(States.ROOT)
.On(Events.TRNS_EXIT)
.Goto(States.INIT);
In(States.HomePageBase)
.On(Events.TRNS_ThumbnailListPage)
.Goto(States.ThumbnailListPage);
In(States.ThumbnailListPage)
.On(Events.TRNS_BACK)
.Goto(States.HomePage);
In(States.ThumbnailListPageBase)
.On(Events.TRNS_PreviewPage)
.Goto(States.PreviewPage);
In(States.HomePage)
.ExecuteOnEntry(__FTC_Event_HomePage_Entry);
In(States.HomePage)
.ExecuteOnExit(__FTC_Event_HomePage_Exit);
In(States.HomePageBase)
.ExecuteOnEntry(HomePageBase_Entry);
In(States.HomePageBase)
.ExecuteOnExit(HomePageBase_Exit);
In(States.ThumbnailListPage)
.ExecuteOnEntry(ThumbnailListPage_Entry);
In(States.ThumbnailListPage)
.ExecuteOnExit(ThumbnailListPage_Exit);
In(States.PreviewPage)
.ExecuteOnEntry(__FTC_Event_PreviewPage_Entry);
In(States.PreviewPage)
.ExecuteOnExit(__FTC_Event_PreviewPage_Exit);
	Initialize(States.INIT);
}
public virtual async Task __FTC_Event_HomePage_Entry() {
ICollection<int> ribbonMenuEventId = new List<int>{  };
	ShowFrame("HomePage",ribbonMenuEventId);
}
public virtual async Task __FTC_Event_HomePage_Exit() {
ICollection<int> ribbonMenuEventId = new List<int>{  };
	HideFrame("HomePage", ribbonMenuEventId);
}
public virtual async Task HomePageBase_Entry() {
	await OnHomePageBase_Entry();
}
public virtual async Task HomePageBase_Exit() {
	await OnHomePageBase_Exit();
}
public virtual async Task ThumbnailListPage_Entry() {
	await OnThumbnailListPage_Entry();
ICollection<int> ribbonMenuEventId = new List<int>{  };
	ShowFrame("ThumbnailListPage",ribbonMenuEventId);
}
public virtual async Task ThumbnailListPage_Exit() {
	await OnThumbnailListPage_Exit();
ICollection<int> ribbonMenuEventId = new List<int>{  };
	HideFrame("ThumbnailListPage", ribbonMenuEventId);
}
public virtual async Task __FTC_Event_PreviewPage_Entry() {
ICollection<int> ribbonMenuEventId = new List<int>{  };
	ShowFrame("PreviewPage",ribbonMenuEventId);
}
public virtual async Task __FTC_Event_PreviewPage_Exit() {
ICollection<int> ribbonMenuEventId = new List<int>{  };
	HideFrame("PreviewPage", ribbonMenuEventId);
}
}
}
