using System.Collections.Generic;
using Newtonsoft.Json;

namespace pixstock.apl.app.Models
{
    public class Category
    {
        public long Id { get; set; }

        public string Name { get; set; }

        /**
         * リンクしているサブカテゴリ情報一覧
         * このプロパティは、フロントエンドへのシリアライズ対象外です。
         */
        [JsonIgnore]
        public List<Category> LinkSubCategoryList { get; set; }

        /**
         * リンクしているコンテント情報一覧
         * このプロパティは、フロントエンドへのシリアライズ対象外です。
         */
        [JsonIgnore]
        public List<Content> LinkContentList { get; set; }

        /**
         * リンクしているサブカテゴリが存在するか示すフラグです。
         */
        public bool HasLinkSubCategoryFlag { get; set; }
    }
}
