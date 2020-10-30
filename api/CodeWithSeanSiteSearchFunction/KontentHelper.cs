using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeWithSeanSiteSearchFunction
{
    public class KontentHelper
    {
        private readonly IDeliveryClient deliveryClient;

        public KontentHelper(IConfiguration configuration)
        {
            var projectId = configuration["KontentProjectID"];
            this.deliveryClient = DeliveryClientBuilder.WithProjectId(projectId).Build();
        }

        public async Task<ContentItem> GetArticleByUrlSlug(string urlSlug)
        {
            var parameters = new List<IQueryParameter>
            {
                new EqualsFilter("system.type", "article"),
                new EqualsFilter("elements.url_pattern", urlSlug),
                new LimitParameter(1)
            };

            var response = await deliveryClient.GetItemsAsync(parameters);
            return response.Items.FirstOrDefault();
        }

        public async Task<IEnumerable<Blog>> GetArticlesForSearch()
        {
            var parameters = new List<IQueryParameter>
            {
                new EqualsFilter("system.type", "blog_post"),
                new OrderParameter("elements.created_date", SortOrder.Descending)
            };

            var response = await deliveryClient.GetItemsAsync(parameters);

            return response.Items.Select(GetArticle);
        }

        public async Task<IEnumerable<Blog>> GetArticlesForSearch(IEnumerable<string> ids)
        {
            var parameters = new List<IQueryParameter>
            {
                new EqualsFilter("system.type", "article"),
                new InFilter("system.id", string.Join(',',ids)),
                new LimitParameter(1)
            };

            var response = await deliveryClient.GetItemsAsync(parameters);
            return response.Items.Select(GetArticle);
        }

        private Blog GetArticle(ContentItem item)
        {
            if (item == null)
            {
                return new Blog();
            }

            return new Blog
            {
                Body = item.GetString("body"),
                ID = item.System.Id,
                CreatedDate = item.GetDateTime("created_date"),
                Summary = item.GetString("summary"),
                Title = item.GetString("title"),
                UrlSlug = item.GetString("slug"),
                TeaserImageUrl = item.GetAssets("teaser_image").FirstOrDefault()?.Url //tem.GetString("teaser_image")
            };
        }
    }
}