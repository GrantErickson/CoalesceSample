<template>
  <v-card
    height="175"
    class="ma-2 pa-2 flex-fill"
    @click="gameDetails(game.gameId)"
  >
    <v-row>
      <v-col cols="2">
        <c-loader-status
          v-slot
          :loaders="{
            'no-secondary-progress': [gameService.getGameImage],
          }"
        >
          <v-card v-if="gameService.getGameImage.wasSuccessful">
            <v-img aspect-ratio="1" max-height="150" :src="image" />
          </v-card>
        </c-loader-status>
      </v-col>
      <v-col cols="10">
        <v-row>
          <v-card flat>
            <v-card-title>
              {{ game.name }}
            </v-card-title>
            <v-card-subtitle class="py-0 pb-4">
              {{ game.genre.name }}
            </v-card-subtitle>
            <v-card-text class="black--text">
              {{ game.description }}
            </v-card-text>
          </v-card>
        </v-row>
        <v-row dense>
          <v-chip-group v-for="tag in tags" :key="tag.tag.name">
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <v-chip
                  class="pa-2 ma-0"
                  color="primary"
                  small
                  dark
                  v-bind="attrs"
                  v-on="on"
                >
                  {{ tag.tag.name }}
                </v-chip>
              </template>
              <span>{{ tag.tag.description }}</span>
            </v-tooltip>
          </v-chip-group>
        </v-row>
      </v-col>
    </v-row>
  </v-card>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Game } from "@/models.g";
import { GameServiceViewModel } from "@/viewmodels.g";

@Component
export default class GameCard extends Vue {
  @Prop({ required: true })
  game!: Game;
  gameService = new GameServiceViewModel();

  created() {
    this.gameService.getGameImage(this.game.gameId);
  }

  get image() {
    console.log(this.gameService.getGameImage.wasSuccessful);
    if (this.gameService.getGameImage.wasSuccessful) {
      return this.gameService.getGameImage.result;
    } else {
      return "";
    }
  }

  get tags() {
    return this.game.gameTags;
  }

  gameDetails(gameId: number) {
    this.$router.push({
      name: "game-details",
      params: { gameId: gameId.toString() },
    });
  }
}
</script>
