<template>
  <c-loader-status
    v-slot
    :loaders="{
      'no-secondary-progress no-initial-content': [gamesList.$load],
    }"
  >
    <SearchAndFilterGames
      :search-text.sync="gamesList.$params.search"
      :filter-game-tags.sync="gamesList.$dataSource.filterTags"
      :filter-rating-upper.sync="gamesList.$dataSource.filterRatingsUpper"
      :filter-rating-lower.sync="gamesList.$dataSource.filterRatingsLower"
      :games-per-page.sync="gamesList.$pageSize"
    />
    <game-card-list v-if="gamesList.$items?.length > 0" />
    <v-card v-else>
      <v-card-title> {{ title }}</v-card-title>
      <v-card-text>
        There are currently no games matching your search parameters.
      </v-card-text>
    </v-card>
  </c-loader-status>
</template>

<script lang="ts">
import { Vue, Component, Prop, Inject } from "vue-property-decorator";
import { GameListViewModel } from "@/viewmodels.g";
import GameCardList from "@/components/game/GameCardList.vue";
import SearchAndFilterGames from "@/components/game/SearchAndFilterGames.vue";

@Component({
  components: {
    SearchAndFilterGames,
    GameCardList,
  },
})
export default class GameList extends Vue {
  @Prop({ required: true })
  title!: string;

  @Inject("GAMESLIST")
  gamesList!: GameListViewModel;

  async created() {
    this.gamesList.$startAutoLoad(this);
    await this.gamesList.$load();
  }
}
</script>
