using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using pixstock.apl.app.core.Infra;
using SimpleInjector;

namespace Pixstock.Applus.Foundations.ContentBrowser.Transitions
{
    public partial class CategoryTreeTransitionWorkflow
    {
        readonly Container mContainer;

        public CategoryTreeTransitionWorkflow(Container container)
        {
            this.mContainer = container;
        }

        async Task OnHomePageBase_Entry()
        {

        }

        async Task OnHomePageBase_Exit()
        {

        }

        async Task OnThumbnailListPage_Entry()
        {

        }

        async Task OnThumbnailListPage_Exit()
        {

        }
    }
}
