const KenticoContent = require('@kentico/kontent-delivery');
const deliveryClient = new KenticoContent.DeliveryClient({
    projectId: '6658cbe8-5978-0046-f871-adf81a715540'
});
async function getAllBlogPosts (){
    var results = await deliveryClient.items()
        .type('blog_post')
        .toPromise(); 
        
    const blogpostsFormatted = results.items.map((item) => {
        return {        
          id: item.system.id,
          date: item.created_date.value,
          title: item.title.value,
          slug: item.slug.value,
          summary: item.summary.value,
          body: item.body.value
        };
      });
      
      return blogpostsFormatted;
}

module.exports = getAllBlogPosts;