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
        contentModels: ["about", "blog_post", "speaking_engagement"]
      }
    }
  ]
};
