using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace CodeWithSeanSiteSearchFunction
{
    public class Blog
    {
        [Key]
        public string ID { get; set; }
        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnMicrosoft), IsRetrievable(false)]
        public string Title { get; set; }
        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnMicrosoft), IsRetrievable(false)]
        public string Body { get; set; }
        [IsSearchable]
        public string Summary { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UrlSlug { get; set; }
        public string TeaserImageUrl { get; set; }
    }
}
