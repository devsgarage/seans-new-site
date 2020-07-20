module.exports = {
  plugins: [
    {
      module: require("@kentico/sourcebit-source-kontent"),
      options: {
        projectId: "6658cbe8-5978-0046-f871-adf81a715540",
        languageCodenames: ["default"]
      }
    },
    {
      module: require("c:/sandbox/sourcebit-target-eleventy"),
      options: {
        writeFile: function (entry, utils) {
          // This function is invoked for each entry and its return value determines
          // whether the entry will be written to a file. When an object with `content`,
          // `format` and `path` properties is returned, a file will be written with
          // those parameters. If a falsy value is returned, no file will be created.
          const { __metadata: meta, ...fields } = entry;

          if (!meta) return;

          const { createdAt = "", modelName, projectId, source } = meta;
        }
      }
    }
  ]
};
