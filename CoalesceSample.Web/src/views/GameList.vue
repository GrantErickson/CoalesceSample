<template>
  <c-loader-status
    v-slot
    :loaders="{
      'no-secondary-progress no-initial-content no-error-content': [
        gameService.getGames,
      ],
    }"
  >
    <v-card v-if="gameService.getGames.wasSuccessful">
      <v-card-title> {{ title }}</v-card-title>
      <v-card-text>
        <v-list>
          <v-list-item v-for="(game, i) in games" :key="i">
            <v-list-item-content>
              <v-list-item-title>
                {{ game.name }}
              </v-list-item-title>
              <v-list-item-subtitle>
                {{ game.description }}
              </v-list-item-subtitle>
              <v-divider />
            </v-list-item-content>
          </v-list-item>
        </v-list>
      </v-card-text>
    </v-card>

    <v-card v-else>
      <v-card-title> {{ title }}</v-card-title>
      <v-card-text> There are currently no games.</v-card-text>
    </v-card>
  </c-loader-status>
</template>

<script lang="ts">
import { Vue, Component, Prop } from "vue-property-decorator";
import { GameServiceViewModel } from "@/viewmodels.g";

@Component
export default class GameList extends Vue {
  @Prop({ required: true })
  title!: string;

  gameService = new GameServiceViewModel();

  async created() {
    await this.gameService.getGames();
  }

  get games() {
    return this.gameService.getGames.result;
  }
}
</script>
