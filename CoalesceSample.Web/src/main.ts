import Vue from "vue";
import App from "./App.vue";
import router from "./router";

// Import global CSS and Fonts:
import "typeface-roboto";
import "@fortawesome/fontawesome-free/css/all.css";
import "coalesce-vue-vuetify/dist/coalesce-vue-vuetify.css";
import "@/site.scss";

import Vuetify, { VInput, VTextField } from "vuetify/lib";

import { AxiosClient as CoalesceAxiosClient } from "coalesce-vue";
import CoalesceVuetify from "coalesce-vue-vuetify/lib";

import $metadata from "@/metadata.g";
// viewmodels.g has side effects - it populates the global lookup on ViewModel and ListViewModel.
// This global lookup allows the admin page components to function.
import "@/viewmodels.g";
import applicationUserService from "@/services/UserService";

// SETUP: vuetify
Vue.use(Vuetify);
const vuetify = new Vuetify({
  icons: {
    iconfont: "fa", // 'mdi' || 'mdiSvg' || 'md' || 'fa' || 'fa4'
  },
  customProperties: true,
  theme: {
    options: {
      customProperties: true,
    },
    themes: {
      light: {
        // primary: "#9ccc6f",
        // secondary: "#4d97bc",
        // accent: "#e98f07",
        error: "#df323b", // This is the default error color with darken-1
      },
    },
  },
});

// Global defaults for vuetify components. Change as desired.
(VInput as any).options.props.dense.default = true;
(VTextField as any).options.props.dense.default = true;
(VTextField as any).options.props.outlined.default = true;

// SETUP: coalesce-vue
CoalesceAxiosClient.defaults.baseURL = "/api";
CoalesceAxiosClient.defaults.withCredentials = true;
CoalesceAxiosClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers!["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// SETUP: coalesce-vue-vuetify
Vue.use(CoalesceVuetify, {
  metadata: $metadata,
});

Vue.config.productionTip = false;
router.beforeEach(async (to, from, next) => {
  await applicationUserService.getRoles();
  await applicationUserService.getUserReviews();
  next();
});

applicationUserService.getRoles().then(() => {
  new Vue({
    router,
    vuetify,
    render: (h) => h(App),
  }).$mount("#app");
});
