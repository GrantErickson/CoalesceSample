<template>
  <v-dialog :value="value" width="800" @input="close">
    <v-card>
      <v-card-title> Update Image</v-card-title>
      <v-card-text>
        <v-file-input v-model="newImage" label="New Image" />
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn color="primary" @click="updateImage"> Update Image</v-btn>
        <v-btn color="primary" class="px-3" text @click="close"> Close </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { GameServiceViewModel } from "@/viewmodels.g";
import { Game } from "@/models.g";
@Component
export default class UpdateImageDialog extends Vue {
  @Prop({ required: true })
  value!: boolean;

  @Prop({ required: true })
  game!: Game;

  gameService = new GameServiceViewModel();

  newImage: File | null = null;

  close() {
    this.$emit("input", false);
  }

  async updateImage() {
    if (this.newImage !== null) {
      await this.gameService.uploadGameImage(this.game.gameId, this.newImage);
      if (this.gameService.uploadGameImage.wasSuccessful) {
        this.game.image!.base64Image = await this.file2Base64(this.newImage);
      }
    }
    this.close();
  }

  //https://stackoverflow.com/a/65586375
  file2Base64(file: File): Promise<string> {
    return new Promise<string>((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result?.toString() || "");
      reader.onerror = (error) => reject(error);
    });
  }
}
</script>
