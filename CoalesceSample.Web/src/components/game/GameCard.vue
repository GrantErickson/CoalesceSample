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
        <v-sheet flat class="float-right">
          <v-card-title>
            {{ game.likes }}
            <v-icon color="secondary" class="fa fa-thumbs-up px-2" />
          </v-card-title>
        </v-sheet>
        <v-card-title>
          {{ game.name }}
        </v-card-title>
        <v-card-subtitle>
          {{ game.genre.name }}
        </v-card-subtitle>
        <v-card-text class="black--text">
          {{ game.description }}

          <v-row dense class="pt-4">
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
        </v-card-text>
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

  gameService: GameServiceViewModel = new GameServiceViewModel();

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

  async gameDetails(gameId: number) {
    //await this.gameService.getGameDetails(gameId);
    // if (
    //   this.gameService.getGameDetails.wasSuccessful &&
    //   this.gameService.getGameDetails.result
    // ) {
    await this.$router.push({
      name: "game-details",
      params: { gameId: gameId.toString() },
    });
    //  }
  }
}
</script>
