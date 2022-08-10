<template>
  <c-loader-status
    v-slot
    :loaders="{
      'no-secondary-progress no-initial-content': [gameService.getGames],
    }"
  >
    <game-card-list v-if="games.length > 0" :games="games"></game-card-list>
    <v-card v-else>
      <v-card-title> {{ title }}</v-card-title>
      <v-card-text> There are currently no games.</v-card-text>
    </v-card>
  </c-loader-status>
</template>

<script lang="ts">
import { Vue, Component, Prop } from "vue-property-decorator";
import { GameServiceViewModel } from "@/viewmodels.g";
import GameCardList from "@/components/game/GameCardList.vue";
import { Game } from "@/models.g";

@Component({
  components: {
    GameCardList,
  },
})
export default class GameList extends Vue {
  @Prop({ required: true })
  title!: string;

  gameService = new GameServiceViewModel();

  async created() {
    await this.gameService.getGames();
  }

  get games(): Game[] {
    return this.gameService.getGames.result ?? [];
  }
}
</script>
