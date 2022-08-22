<template>
  <c-loader-status
    v-slot
    :loaders="{
      'no-secondary-progress no-initial-content': [gamesList.$load],
    }"
  >
    <SearchAndFilter
      :filter-game-tags.sync="dataSource.filterTags"
      :games-per-page.sync="gamesList.$pageSize"
    />
    <game-card-list v-if="gamesList.$items?.length > 0" />
    <v-card v-else>
      <v-card-title> {{ title }}</v-card-title>
      <v-card-text> There are currently no games.</v-card-text>
    </v-card>
  </c-loader-status>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch, Inject } from "vue-property-decorator";
import { GameListViewModel } from "@/viewmodels.g";
import GameCardList from "@/components/game/GameCardList.vue";
import { Game } from "@/models.g";
import SearchAndFilter from "@/components/SearchAndFilter.vue";

@Component({
  components: {
    SearchAndFilter,
    GameCardList,
  },
})
export default class GameList extends Vue {
  @Prop({ required: true })
  title!: string;

  @Inject("GAMESLIST")
  gamesList!: GameListViewModel;

  dataSource = (this.gamesList.$dataSource =
    new Game.DataSources.GameDataSource());

  async created() {
    this.gamesList.$dataSource = this.dataSource;
    this.gamesList.$startAutoLoad(this);
    await this.gamesList.$load();
  }

  @Watch("gamesList.$items")
  log() {
    console.log(this.gamesList.$items);
  }
}
</script>
