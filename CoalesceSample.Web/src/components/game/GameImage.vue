<template>
  <v-container>
    <v-card-text class="pa-0 ma-0">
      <v-img
        v-if="syncedGame.image?.content?.length > 0"
        :key="imageUrl"
        :src="imageUrl"
        aspect-ratio="1"
      />
    </v-card-text>
    <v-footer
      v-if="$isAdmin && !readOnly"
      class="justify-center pt-2"
      color="white"
      padless
    >
      <v-btn class="ma-1" color="primary" @click="showEdit = true">
        Change Image
      </v-btn>
    </v-footer>
    <v-dialog v-model="showEdit" width="800">
      <v-card>
        <v-card-title> Update Image </v-card-title>
        <v-card-text>
          <v-file-input v-model="newImage" label="New Image" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn color="primary" @click="updateImage"> Update Image</v-btn>
          <v-btn color="primary" class="px-3" text @click="close">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script lang="ts">
import { Component, Prop, PropSync, Vue } from "vue-property-decorator";
import { GameServiceViewModel } from "@/viewmodels.g";
import { Game } from "@/models.g";
import CardBase from "@/components/templates/CardBase.vue";
@Component({
  components: { CardBase },
})
export default class GameImage extends Vue {
  @PropSync("game", { required: true })
  syncedGame!: Game;

  @Prop({ required: false, default: true })
  readOnly!: boolean;

  gameService = new GameServiceViewModel();

  showEdit = false;

  newImage: File | null = null;

  close() {
    this.showEdit = false;
  }

  async updateImage() {
    if (this.newImage !== null) {
      await this.gameService.uploadGameImage(
        this.syncedGame.gameId,
        this.newImage
      );
      if (this.gameService.uploadGameImage.wasSuccessful) {
        this.syncedGame.image = this.gameService.uploadGameImage.result!;
      }
    }
    this.close();
  }

  get imageUrl() {
    if (this.syncedGame.image?.content) {
      return (
        "data:application/octet-stream;base64," + this.syncedGame.image?.content
      );
    } else {
      return "";
    }
  }
}
</script>
