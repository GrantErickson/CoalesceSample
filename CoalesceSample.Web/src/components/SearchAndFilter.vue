<template>
  <v-card>
    <v-card-text>
      <v-row>
        <v-text-field
          v-model="localGamesPerPage"
          placeholder="Games Per Page"
        />
        <v-autocomplete
          v-model="localFilterTags"
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
      </v-row>
    </v-card-text>
    <v-card-actions class="pb-4 pt-0">
      <v-row class="justify-center">
        <v-pagination
          v-model="games.$page"
          :length="games.$pageCount"
          prev-icon="fa-chevron-left"
          next-icon="fa-chevron-right"
        />
      </v-row>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
import { Component, Inject, Prop, Vue, Watch } from "vue-property-decorator";
import { GameListViewModel, TagListViewModel } from "@/viewmodels.g";

@Component({
  components: {},
})
export default class SearchAndFilter extends Vue {
  //@Prop({ required: true })
  //gameIds!: number[];
  @Prop({ required: true, default: "" })
  filterGameTags!: string;
  @Prop({ required: true, default: 10 })
  gamesPerPage!: number;

  filterGameTagsArray: number[] = [];

  @Inject("GAMESLIST")
  games!: GameListViewModel;
  tags = new TagListViewModel();

  showEditTags = false;

  async created() {
    this.tags.$pageSize = 1000;
    await this.tags.$load();

    //await this.update();
  }

  @Watch("filterGameTags")
  updateArray() {
    if (this.filterGameTags?.length > 0) {
      this.filterGameTagsArray = this.filterGameTags
        .split(",")
        .map((x) => parseInt(x));
    }
  }

  get localFilterTags() {
    return this.filterGameTagsArray;
  }

  set localFilterTags(value: number[]) {
    this.filterGameTagsArray = value;
    let string =
      this.filterGameTagsArray
        .filter((tagId) => tagId !== null && tagId !== undefined)
        .map((tagId) => {
          return tagId;
        })
        .join(",") ?? "";

    this.$emit("update:filterGameTags", string);
  }

  get localGamesPerPage() {
    return this.gamesPerPage;
  }
  set localGamesPerPage(value: number) {
    this.$emit("update:gamesPerPage", value);
  }

  toggleShowEditTags() {
    this.showEditTags = !this.showEditTags;
  }
}
</script>
