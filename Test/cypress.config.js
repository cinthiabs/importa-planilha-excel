const { defineConfig } = require("cypress");

module.exports = defineConfig({
  e2e: {
    baseUrl:'https://localhost:7040/',
    setupNodeEvents(on, config) {
    },
  },
});
