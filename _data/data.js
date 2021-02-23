
const Sourcebit = require('sourcebit');
const SourcebitConfig = require('../sourcebit');

async function getAllData (){    
    const data = await Sourcebit.fetch(SourcebitConfig);
    const dataByModelType = {};
    
    data.objects.forEach(object => {
        dataByModelType[object.__metadata.modelName] = dataByModelType[object.__metadata.modelName] || [];
        dataByModelType[object.__metadata.modelName].push(object);
    });
    dataByModelType.blog_post = dataByModelType.blog_post.sort(function(a, b){return new Date(a.created_date) - new Date(b.created_date)});
    //console.log(dataByModelType.blog_post);
    return dataByModelType;
}

module.exports = getAllData;
        