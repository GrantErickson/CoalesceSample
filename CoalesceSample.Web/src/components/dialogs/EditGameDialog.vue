<template>
  <v-dialog :value="value" width="800" @input="close">
    <v-card>
      <c-admin-editor :model="game" :model-id="game.gameId" />
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Game } from "@/models.g";

@Component
export default class EditGameDialog extends Vue {
  @Prop({ required: true })
  value!: boolean;

  @Prop({ required: true })
  game!: Game;
  created() {
    console.log(this.game);
  }
  close() {
    this.$emit("input", false);
  }
}
</script>
