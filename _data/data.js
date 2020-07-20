
const Sourcebit = require('sourcebit');
const SourcebitConfig = require('../sourcebit');

async function getAllData (){
    const data = await Sourcebit.fetch(SourcebitConfig);
    const dataByModelType = {};

    data.objects.forEach(object => {
        dataByModelType[object.__metadata.modelName] = dataByModelType[object.__metadata.modelName] || [];
        dataByModelType[object.__metadata.modelName].push(object);
    });

    console.log(dataByModelType);
    return dataByModelType;
}

module.exports = getAllData;
        