<template>
  <v-card>
    <v-card-text>
      <v-list>
        <v-list-item v-for="(game, i) in games" :key="i" class="py-3">
          <game-card :game="game"></game-card>
        </v-list-item>
      </v-list>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { GameServiceViewModel } from "@/viewmodels.g";
import { Game } from "@/models.g";
import GameCard from "@/components/game/GameCard.vue";

@Component({
  components: {
    GameCard,
  },
})
export default class GameCardList extends Vue {
  @Prop({ required: true })
  games!: Game[];
  gameService = new GameServiceViewModel();
}
</script>

<style scoped></style>
