<template>
  <v-app id="vue-app">
    <v-navigation-drawer v-model="drawer" app clipped>
      <v-row class="py-5 justify-center">
        <v-card class="">
          <v-btn outlined to="/login" exact> Account </v-btn>
        </v-card>
      </v-row>
      <v-list>
        <v-list-item link to="/">
          <v-list-item-action>
            <v-icon>fas fa-home</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Home</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-item link to="/gamelist">
          <v-list-item-action>
            <v-icon>fas fa-dice</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Game List</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar app color="primary" dark dense clipped-left>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-toolbar-title>
        <router-link to="/" class="white--text" style="text-decoration: none">
          Coalesce Vue Template
        </router-link>
      </v-toolbar-title>
    </v-app-bar>

    <v-main>
      <transition
        name="router-transition"
        mode="out-in"
        appear
        @enter="routerViewOnEnter"
      >
        <!-- https://stackoverflow.com/questions/52847979/what-is-router-view-key-route-fullpath -->
        <router-view ref="routerView" :key="$route.path" />
      </transition>
    </v-main>
  </v-app>
</template>

<script lang="ts">
import Vue from "vue";
import { Component, Inject, Provide } from "vue-property-decorator";
import { GameListViewModel, TagListViewModel } from "@/viewmodels.g";
import { Game } from "@/models.g";

@Component({
  components: {},
})
export default class App extends Vue {
  drawer: boolean | null = null;
  routeComponent: Vue | null = null;

  @Provide("GAMESLIST")
  gamesList = new GameListViewModel();

  dataSource = (this.gamesList.$dataSource =
    new Game.DataSources.GameDataSource());

  @Provide("TAGSLIST")
  tags = new TagListViewModel();

  get routeMeta() {
    if (!this.$route || this.$route.name === null) return null;

    return this.$route.meta;
  }

  routerViewOnEnter() {
    this.routeComponent = this.$refs.routerView as Vue;
  }

  async created() {
    this.gamesList.$dataSource = this.dataSource;
    this.tags.$pageSize = 1000;
    await this.tags.$load();

    const baseTitle = document.title;
    this.$watch(
      () => (this.routeComponent as any)?.pageTitle,
      (n: string | null | undefined) => {
        if (n) {
          document.title = n + " - " + baseTitle;
        } else {
          document.title = baseTitle;
        }
      },
      { immediate: true }
    );
  }
}
</script>

<style lang="scss">
.router-transition-enter-active,
.router-transition-leave-active {
  // transition: 0.2s cubic-bezier(0.25, 0.8, 0.5, 1);
  transition: 0.1s ease-out;
}

.router-transition-move {
  transition: transform 0.4s;
}

.router-transition-enter,
.router-transition-leave-to {
  opacity: 0;
  // transform: translateY(5px);
}
</style>
