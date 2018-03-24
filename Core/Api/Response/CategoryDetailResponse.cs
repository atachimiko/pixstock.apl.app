using pixstock.apl.app.Models;

namespace pixstock.apl.app.core.Api.Response
{
    public class CategoryDetailResponse
    {
        public Category Category { get; set; }

        public Content[] Content { get; set; }
    }
}