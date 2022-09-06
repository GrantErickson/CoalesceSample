<template>
  <v-dialog :value="value" width="800" @input="close">
    <v-card>
      <v-card-title> Edit Tags </v-card-title>
      <v-card-text>
        <c-loader-status
          v-slot
          :loaders="{
            'no-secondary-progress no-initial-content no-error-content': [
              tags.$load,
            ],
          }"
        >
          <v-autocomplete
            v-model="gameTagIds"
            :items="tags.$items"
            item-text="name"
            item-value="tagId"
            label="Tags"
            hide-details
            multiple
            chips
          >
            <template slot="item" slot-scope="{ item }">
              <v-chip
                :key="item.tagId"
                :value="item.tagId"
                color="primary"
                dark
                small
              >
                {{ item.name }}
              </v-chip>
            </template>
          </v-autocomplete>
        </c-loader-status>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn color="primary" @click="saveGameTags"> Save </v-btn>
        <v-btn text @click="close"> Close </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { GameServiceViewModel, TagListViewModel } from "@/viewmodels.g";
import { Game } from "@/models.g";

@Component
export default class EditTagsDialog extends Vue {
  @Prop({ required: true })
  value!: boolean;

  @Prop({ required: true })
  game!: Game;

  tags = new TagListViewModel();
  gameService = new GameServiceViewModel();

  gameTagIds: number[] = [];

  close() {
    this.$emit("input", false);
  }

  async created() {
    this.tags.$pageSize = 1000;
    await this.tags.$load();

    this.gameTagIds = this.game.gameTags?.map((tag) => tag.tagId!) ?? [];
  }

  async saveGameTags() {
    await this.gameService.setGameTags(this.game.gameId, this.gameTagIds);
    if (this.gameService.setGameTags.wasSuccessful) {
      console.log(this.game.gameTags);
      console.log(this.gameService.setGameTags.result);
      this.game.gameTags = this.gameService.setGameTags.result ?? [];
    }
    close();
  }
}
</script>
