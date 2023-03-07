<template>
  <v-card>
    <v-card-text>
      <v-row dense>
        <v-text-field
          v-model="localSearchText"
          class="pb-3"
          hide-details
          placeholder="Search"
        />
      </v-row>
      <v-row dense class="text-center" align="center">
        <v-sheet width="500">
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
        </v-sheet>
        <v-menu bottom offset-y :close-on-content-click="false">
          <template #activator="{ on, attrs }">
            <v-btn
              v-bind="attrs"
              color="primary"
              class="ml-2"
              outlined
              text
              v-on="on"
            >
              <v-icon>fa-star</v-icon>
              Ratings
            </v-btn>
          </template>
          <v-sheet width="400" class="overflow-hidden">
            <v-row dense class="px-3 pt-8">
              <v-btn
                fab
                x-small
                class="mr-3"
                @click="sliderRangeArray = [0, 5]"
              >
                <v-icon>fa-refresh</v-icon>
              </v-btn>
              <v-range-slider
                v-model="sliderRangeArray"
                :max="5"
                :min="0"
                :step="0.5"
                :ticks="true"
                :thumb-label="true"
                :thumb-size="24"
                thumb-color="primary"
                track-color="secondary"
                :tick-size="4"
                :tick-labels="rangeTickLabels"
              />
            </v-row>
          </v-sheet>
        </v-menu>
        <span class="pa-0 ma-1 text-h6">Games Per Page:</span>
        <v-text-field
          v-model="localGamesPerPage"
          hide-details
          placeholder="Games Per Page"
          :rules="[isNumeric, greaterThanZero]"
        />
      </v-row>
    </v-card-text>
    <v-card-actions class="pb-4 pt-0">
      <v-row class="justify-center">
        <v-pagination
          v-model="games.$page"
          class="pa-2"
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
export default class SearchAndFilterGames extends Vue {
  @Prop({ required: true, default: "" })
  filterGameTags!: string;
  @Prop({ required: true, default: "" })
  filterRatingUpper!: number;
  @Prop({ required: true, default: "" })
  filterRatingLower!: number;
  @Prop({ required: true, default: 10 })
  gamesPerPage!: number;
  @Prop({ required: true, default: "" })
  searchText!: string;

  @Inject("GAMESLIST")
  games!: GameListViewModel;

  @Inject("TAGSLIST")
  tags!: TagListViewModel;

  rangeTickLabels = ["0", "", "1", "", "2", "", "3", "", "4", "", "5"];

  filterGameTagsArray: number[] = [];
  sliderRangeArray: number[] = [0, 5];

  @Watch("filterGameTags")
  updateTagsArray() {
    if (this.filterGameTags?.length > 0) {
      this.filterGameTagsArray = this.filterGameTags
        .split(",")
        .map((x) => parseInt(x));
    }
  }

  @Watch("sliderRangeArray")
  updateSliderRangeArray() {
    this.$emit("update:filterRatingLower", this.sliderRangeArray[0]);
    this.$emit("update:filterRatingUpper", this.sliderRangeArray[1]);
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

  get localSearchText() {
    return this.searchText;
  }
  set localSearchText(value: string) {
    this.$emit("update:searchText", value);
  }

  isNumeric(value: string) {
    return /^\d+$/.test(value);
  }

  greaterThanZero(value: string) {
    return parseInt(value) > 0;
  }
}
</script>
