<template>
  <card-base
    :right-slots-list="rightSlots"
    @click.native.stop="gameDetails(game.gameId)"
  >
    <span slot="left">
      <v-card class="fill-height">
        <c-loader-status
          v-slot
          :loaders="{
            'no-secondary-progress no-error-content': [
              gameService.getGameImage,
            ],
          }"
        >
          <game-image :game="game" />
        </c-loader-status>
      </v-card>
    </span>
    <span slot="main">
      <v-card-title>
        {{ game.name }}
      </v-card-title>
      <v-card-subtitle> {{ game.genre.name }} </v-card-subtitle>
      <v-card-text class="black--text">
        {{ game.description }}
        <v-row dense class="pt-4">
          <v-chip-group v-for="tag in tags" :key="tag.tag.name">
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <v-chip
                  class="pa-2 mr-1 my-0"
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
    </span>
  </card-base>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Game } from "@/models.g";
import { GameServiceViewModel } from "@/viewmodels.g";
import LikeButton from "@/components/LikeButton.vue";
import StarRating from "@/components/StarRating.vue";
import EditGameDialog from "@/components/dialogs/EditGameDialog.vue";
import GameImage from "@/components/game/GameImage.vue";
import CardBase from "@/components/templates/CardBase.vue";
import IComponent from "@/services/i-component";

@Component({
  components: { CardBase, GameImage, EditGameDialog, StarRating, LikeButton },
})
export default class GameCard extends Vue {
  @Prop({ required: true })
  game!: Game;

  gameService: GameServiceViewModel = new GameServiceViewModel();

  async created() {
    await this.gameService.getGameImage(this.game.gameId);
    this.game.image = this.gameService.getGameImage.result;
  }

  get image() {
    if (this.gameService.getGameImage.wasSuccessful) {
      return this.gameService.getGameImage.result;
    } else {
      return "";
    }
  }

  get tags() {
    return this.game.gameTags;
  }

  get rightSlots(): IComponent[] {
    return [
      {
        component: LikeButton,
        props: { game: this.game },
      },
      {
        component: StarRating,
        props: { game: this.game, rating: this.game.averageRating },
      },
      {
        component: EditGameDialog,
        props: { game: this.game },
      },
    ];
  }

  async gameDetails(gameId: string) {
    await this.$router.push({
      name: "game-details",
      params: { gameId: gameId },
    });
  }
}
</script>
