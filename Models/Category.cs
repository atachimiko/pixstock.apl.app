using System.Collections.Generic;
using Newtonsoft.Json;

namespace pixstock.apl.app.Models
{
    public class Category
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public List<Category> LinkSubCategoryList{get;set;}

        [JsonIgnore]
        public List<Content> LinkContentList { get; set; }
    }
}