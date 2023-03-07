<template>
  <v-btn v-if="$isAdmin" fab x-small @click.native.stop="toggleShowEditGame">
    <v-icon color="red"> fa-pencil </v-icon>
    <v-dialog :value="showEditGame" width="800" @input="close">
      <v-card>
        <c-admin-editor
          :key="'editGame' + syncedGame.gameId"
          :model="syncedGame"
          :model-id="syncedGame.gameId"
        />
      </v-card>
    </v-dialog>
  </v-btn>
</template>

<script lang="ts">
import { Component, PropSync, Vue } from "vue-property-decorator";
import { Game } from "@/models.g";
import { GameViewModel } from "@/viewmodels.g";

@Component
export default class EditGameDialog extends Vue {
  @PropSync("game", { required: true })
  syncedGame!: Game;

  showEditGame = false;

  toggleShowEditGame() {
    this.showEditGame = !this.showEditGame;
  }

  close() {
    this.showEditGame = false;
    const gameViewModel = new GameViewModel(this.syncedGame);
    console.log(this.syncedGame);
    gameViewModel.$save();
  }
}
</script>
