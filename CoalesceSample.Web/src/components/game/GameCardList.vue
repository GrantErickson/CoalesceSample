<template>
  <v-card>
    <v-card-text>
      <v-list>
        <v-list-item v-for="game in games.$items" :key="game.name" class="py-3">
          <game-card :game="game"></game-card>
        </v-list-item>
      </v-list>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { Component, Inject, Vue } from "vue-property-decorator";

import GameCard from "@/components/game/GameCard.vue";
import { GameListViewModel } from "@/viewmodels.g";

@Component({
  components: {
    GameCard,
  },
})
export default class GameCardList extends Vue {
  @Inject("GAMESLIST")
  games!: GameListViewModel;
}
</script>
