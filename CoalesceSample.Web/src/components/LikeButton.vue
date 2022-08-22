<template>
  <v-card-title>
    <span :key="'likes' + game.likes">Likes: {{ game.likes }}</span>
    <v-btn class="mx-3" fab x-small @click.native.stop="toggleLike">
      <v-icon
        :color="hasLiked ? 'primary' : 'secondary'"
        class="fa fa-thumbs-up"
      />
    </v-btn>
  </v-card-title>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { GameServiceViewModel } from "@/viewmodels.g";
import { Game } from "@/models.g";

@Component({
  components: {},
})
export default class LikeButton extends Vue {
  @Prop({ required: true })
  game!: Game;

  gameService: GameServiceViewModel = new GameServiceViewModel();

  hasLiked = false;

  created() {
    this.hasLiked =
      localStorage.getItem("liked-game-" + this.game.gameId) === "true" ??
      false;
  }

  async toggleLike() {
    if (!this.hasLiked) {
      await this.gameService.addLike(this.game.gameId);
      if (this.gameService.addLike.wasSuccessful) {
        localStorage.setItem("liked-game-" + this.game.gameId, "true");
        this.hasLiked = true;
        if (this.game && this.game.likes) {
          this.game.likes++;
        }
      }
    } else {
      await this.gameService.removeLike(this.game.gameId);
      if (this.gameService.removeLike.wasSuccessful) {
        localStorage.setItem("liked-game-" + this.game.gameId, "false");
        this.hasLiked = false;
        if (this.game && this.game.likes) {
          this.game.likes--;
        }
      }
    }
  }
}
</script>
