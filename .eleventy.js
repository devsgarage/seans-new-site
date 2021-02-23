const moment = require('moment');
const fs = require("fs");
moment.locale('en');
 
module.exports = function (eleventyConfig) {
 console.log('This ran');
  eleventyConfig.addFilter('dateIso', date => {
    return moment(date).toISOString();
  });
 
  eleventyConfig.addFilter('dateReadable', date => {
    return moment(date).utc().format('LL'); // E.g. May 31, 2019
  });

  eleventyConfig.addShortcode('excerpt', article => extractExcerpt(article));

  eleventyConfig.addNunjucksShortcode('getMyDate', (date1, date2) => getFormattedDate(date1, date2));

  eleventyConfig.addPassthroughCopy('css');
  eleventyConfig.addPassthroughCopy('assets');
  eleventyConfig.addCollection('', function(collection){
    return collection.getAll().sort(function(a, b) {
      return b.date - a.date;
    });
  });
  eleventyConfig.setBrowserSyncConfig({
    notify: true,
    callbacks: {
      ready: function(err, bs) {   
        bs.addMiddleware("*", (req, res) => {          
          const content_404 = fs.readFileSync('_site/404.html');
          // Provides the 404 content without redirect.
          //res.write("<html><head></head><body><h1>Page not found</h1></body></html>");
          res.write(content_404);
          // Add 404 http status code in request header.
          res.writeHead(404, { "Content-Type": "text/html" });
           res.writeHead(404);
          res.end();
          // res.writeHead(302, {
          //     location: "404.html"
          // });
          // res.end("Redirecting!");
        });
      }
    } 
  });
};

function extractExcerpt(article) {
    if (!article.hasOwnProperty('body')) {
      console.warn('Failed to extract excerpt: Document has no property "templateContent".');
      return null;
    }
   
    let excerpt = null;
    const content = article.body;
   
    // The start and end separators to try and match to extract the excerpt
    const separatorsList = [
      { start: '<!-- Excerpt Start -->', end: '<!-- Excerpt End -->' },
      { start: '<p>', end: '</p>' }
    ];
   
    separatorsList.some(separators => {
      const startPosition = content.indexOf(separators.start);
      const endPosition = content.indexOf(separators.end);
   
      if (startPosition !== -1 && endPosition !== -1) {
        excerpt = content.substring(startPosition + separators.start.length, endPosition).trim();
        return true; // Exit out of array loop on first match
      }
    });
   
    return excerpt;
  }

function getFormattedDate (date1, date2){  
  if (date1 === date2)
  return moment(date1).utc().format('LL');
  else
  return moment(date1).utc().format('MMMM DD') + ' - ' + moment(date2).utc().format('DD, YYYY');
}