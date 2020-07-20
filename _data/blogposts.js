const KenticoContent = require('@kentico/kontent-delivery');
const deliveryClient = new KenticoContent.DeliveryClient({
    projectId: '6658cbe8-5978-0046-f871-adf81a715540'    
});

const Sourcebit = require('sourcebit');
const SourcebitConfig = require('../sourcebit');

async function getAllBlogPosts2 (){
    const data = await Sourcebit.fetch(SourcebitConfig)

    console.log(data)
  

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

async function getAllBlogPosts (){
  const data = await Sourcebit.fetch(SourcebitConfig)

  
      
  const blogpostsFormatted ={ 
      blogs: data.objects
        .filter((item)=> item.__metadata.modelName == 'blog_post')
        .map((item) => {
          return {        
            id: item.__metadata.id,
            date: item.__metadata.createdAt,
            title: item.title,
            slug: item.slug,
            summary: item.summary,
            body: item.body
          };
        })
      }

    console.log(blogpostsFormatted)
    
    return blogpostsFormatted;
}

/*

- blog_post
- author

{
  blog_post: [
    {...},
    {...}
  ],
  author: [
    {...}
  ]
}

*/

module.exports = getAllBlogPosts;