import Vue from "vue";
import Router from "vue-router";
import { CAdminTablePage, CAdminEditorPage } from "coalesce-vue-vuetify/lib";

Vue.use(Router);

export default new Router({
  mode: "history",
  routes: [
    {
      path: "/",
      name: "home",
      component: () => import("./views/Home.vue"),
    },
    {
      path: "/gamelist",
      name: "game-list",
      component: () => import("./views/Games.vue"),
      props: { title: "Example Games List" },
    },
    {
      path: "/gamedetails/:gameId",
      name: "game-details",
      component: () => import("./views/GameDetails.vue"),
      props: true,
    },
    {
      path: "/login",
      name: "login",
      component: () => import("./views/Login.vue"),
    },

    // Coalesce admin routes
    {
      path: "/admin/:type",
      name: "coalesce-admin-list",
      component: CAdminTablePage,
      props: (r) => ({
        type: r.params.type,
      }),
    },
    {
      path: "/admin/:type/edit/:id?",
      name: "coalesce-admin-item",
      component: CAdminEditorPage,
      props: (r) => ({
        type: r.params.type,
        id: r.params.id,
      }),
    },
  ],
});
