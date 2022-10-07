<template>
  <c-loader-status
    v-slot
    class="ma-4"
    :loaders="{
      'no-secondary-progress no-initial-content': [gamesList.$load],
    }"
  >
    <SearchAndFilterGames
      class="mb-2"
      :search-text.sync="gamesList.$params.search"
      :filter-game-tags.sync="gamesList.$dataSource.filterTags"
      :filter-rating-upper.sync="gamesList.$dataSource.filterRatingsUpper"
      :filter-rating-lower.sync="gamesList.$dataSource.filterRatingsLower"
      :games-per-page.sync="gamesList.$pageSize"
    />
    <v-card v-if="gamesList.$items?.length > 0">
      <v-card-text>
        <v-list>
          <v-list-item
            v-for="game in gamesList.$items"
            :key="game.gameId"
            class="py-3"
          >
            <game-card :game="game" />
          </v-list-item>
        </v-list>
      </v-card-text>
    </v-card>
    <v-card v-else>
      <v-card-title> Game List </v-card-title>
      <v-card-text>
        There are currently no games matching your search parameters.
      </v-card-text>
    </v-card>
  </c-loader-status>
</template>

<script lang="ts">
import { Vue, Component, Inject } from "vue-property-decorator";
import GameCard from "@/components/game/GameCard.vue";
import { GameListViewModel } from "@/viewmodels.g";
import SearchAndFilterGames from "@/components/game/SearchAndFilterGames.vue";

@Component({
  components: {
    SearchAndFilterGames,
    GameCard,
  },
})
export default class GameList extends Vue {
  @Inject("GAMESLIST")
  gamesList!: GameListViewModel;

  async created() {
    this.gamesList.$startAutoLoad(this, { wait: 200 });
    await this.gamesList.$load();
  }
}
</script>
