<template>
  <c-loader-status
    v-slot
    class="ma-4"
    :loaders="{
      'no-secondary-progress no-initial-content no-error-content': [game.$load],
    }"
  >
    <card-base :no-card="true" :right-slots-list="rightSlots">
      <span slot="left">
        <c-loader-status
          v-slot
          :loaders="{
            'no-loading-content no-secondary-progress': [
              gameService.getGameImage,
            ],
          }"
        >
          <v-card :flat="!$isAdmin || !game.image">
            <game-image :game.sync="game" :read-only="false" />
          </v-card>
        </c-loader-status>
      </span>
      <span slot="main">
        <v-card-title>
          {{ game.name }}
        </v-card-title>
        <v-card-subtitle class="pb-0">
          {{ game.genre.name }}
        </v-card-subtitle>
        <v-card-actions>
          <v-chip-group>
            <v-tooltip v-for="tag in gameTags" :key="tag.tag.name" bottom>
              <template #activator="{ on, attrs }">
                <v-chip
                  class="px-2 ma-0 mx-1"
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
          <v-btn
            v-if="$isLoggedIn"
            class="ml-2"
            fab
            x-small
            @click="toggleShowEditTags"
          >
            <v-icon>fa-plus</v-icon>
          </v-btn>
        </v-card-actions>
        <v-card-text class="black--text pb-4">
          {{ game.description }}
        </v-card-text>
      </span>
    </card-base>
    <v-card class="fill-height flex-fill mt-6">
      <v-card-title>
        <v-row class="ma-1" align="center">
          Reviews
          <v-tooltip bottom>
            <template #activator="{ on, attrs }">
              <v-btn
                v-bind="attrs"
                fab
                x-small
                class="ml-3 mt-1"
                :ripple="$isLoggedIn"
                :color="$isLoggedIn ? '' : 'grey'"
                v-on="on"
                @click="toggleAddReview"
              >
                <v-icon>fa-plus</v-icon>
              </v-btn>
            </template>
            <span v-if="!$isLoggedIn">
              You must be logged in to add a review.
            </span>
            <span v-else> Add a review. </span>
          </v-tooltip>
          <v-pagination
            v-model="reviewsListVM.$page"
            class="ml-6"
            :length="reviewsListVM.$pageCount"
            prev-icon="fa-chevron-left"
            next-icon="fa-chevron-right"
          />
          <v-menu offset-x :close-on-content-click="false">
            <template #activator="{ on, attrs }">
              <v-btn
                v-bind="attrs"
                color="primary"
                class="ml-3 mt-1"
                x-small
                fab
                v-on="on"
              >
                <v-icon>fa-filter</v-icon>
              </v-btn>
            </template>
            <v-card flat width="400" class="px-3 pt-8 pb-1 mb-2">
              <v-card-text class="ma-0 pa-0">
                <v-row class="mx-2 mt-0">
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
                    @mouseup="updateReviewList"
                  />
                </v-row>
                <v-row>
                  <v-card flat width="3em" class="ml-3">
                    <v-text-field
                      v-model="reviewsPerPage"
                      placeholder=""
                      :rules="[isNumeric, greaterThanZero]"
                    />
                  </v-card>
                  <span class="pa-0 ma-1 text-h6">Reviews Per Page</span>
                </v-row>
                <v-row>
                  <v-menu
                    v-model="showingCalendar"
                    max-width="345"
                    top
                    nudge-top="75"
                    nudge-right="45"
                    :close-on-content-click="false"
                  >
                    <template #activator="{ on, attrs }">
                      <v-text-field
                        v-model="dateRangeString"
                        class="px-3"
                        label="Date Range"
                        prepend-icon="fa-calendar"
                        readonly
                        v-bind="attrs"
                        v-on="on"
                      />
                    </template>
                    <v-date-picker v-model="dates" width="345" no-title range />
                  </v-menu>
                </v-row>
              </v-card-text>
              <v-card-actions>
                <v-spacer />
                <v-btn outlined small class="mx-3" @click="resetFilters">
                  reset
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-menu>
        </v-row>
        <v-spacer />
        <v-sheet class="ml-3">
          <v-row class="align-center">
            <span :key="'noRatings' + game.numberOfRatings">
              Total Reviews: {{ numberOfRatings }}
            </span>
            <span class="pl-8 pr-2"> Average rating: </span>
            <star-rating
              :key="'avgRatings' + game.averageRating"
              class="mr-4"
              :rating="game.averageRating"
            />
          </v-row>
        </v-sheet>
      </v-card-title>
      <c-loader-status
        v-slot
        :loaders="{
          'no-secondary-progress no-initial-content': [reviewsListVM.$load],
        }"
      >
        <v-divider />
        <game-review-list
          :key="'revList' + game.numberOfRatings"
          :game.sync="game"
          :reviews.sync="reviewsListVM.$items"
        />
      </c-loader-status>
    </v-card>

    <edit-tags-dialog v-model="showEditTags" :game.sync="game" />
    <add-review-dialog v-model="showAddReview" :game.sync="game" />
  </c-loader-status>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { Review } from "@/models.g";
import {
  GameServiceViewModel,
  GameTagViewModel,
  GameViewModel,
  ImageViewModel,
  ReviewListViewModel,
} from "@/viewmodels.g";
import GameReviewList from "@/components/game/GameReviewList.vue";
import StarRating from "@/components/StarRating.vue";
import LikeButton from "@/components/LikeButton.vue";
import AddReviewDialog from "@/components/dialogs/AddReviewDialog.vue";
import UpdateImageDialog from "@/components/game/GameImage.vue";
import GameImage from "@/components/game/GameImage.vue";
import EditTagsDialog from "@/components/dialogs/EditTagsDialog.vue";
import EditGameDialog from "@/components/dialogs/EditGameDialog.vue";
import CardBase from "@/components/templates/CardBase.vue";
import IComponent from "@/services/i-component";

@Component({
  components: {
    CardBase,
    GameImage,
    AddReviewDialog,
    GameReviewList,
    StarRating,
    LikeButton,
    UpdateImageDialog,
    EditTagsDialog,
    EditGameDialog,
  },
})
export default class GameDetails extends Vue {
  @Prop({ required: true })
  gameId!: string;

  gameService: GameServiceViewModel = new GameServiceViewModel();

  showUpdateImage = false;

  showAddReview = false;
  showEditTags = false;

  game: GameViewModel = new GameViewModel();

  reviewsListVM: ReviewListViewModel = new ReviewListViewModel();
  reviewsDataSource = new Review.DataSources.ReviewDataSource();
  reviewsPerPage = 10;

  rangeTickLabels = ["0", "", "1", "", "2", "", "3", "", "4", "", "5"];
  sliderRangeArray: number[] = [0, 5];

  dates: Date[] = [];
  showingCalendar = false;

  ratings: number[] = [];

  async created() {
    this.reviewsListVM.$dataSource = this.reviewsDataSource;
    this.reviewsDataSource.filterGameId = this.gameId;
    this.reviewsDataSource.minRating = this.sliderRangeArray[0];
    this.reviewsDataSource.maxRating = this.sliderRangeArray[1];
    this.reviewsDataSource.firstDate = this.dates[0];
    this.reviewsDataSource.secondDate = this.dates[1];
    await this.reviewsListVM.$load();
    this.reviewsListVM.$startAutoLoad(this, { wait: 50 });

    await this.game.$load(this.gameId);
    await this.gameService.getGameImage(this.gameId);
    this.game.image = new ImageViewModel(this.gameService.getGameImage.result);
    this.ratings = this.game.reviews?.map((r) => r.rating ?? 0) || [];
  }

  get numberOfRatings() {
    return this.game.numberOfRatings;
  }

  get gameTags() {
    return this.game.gameTags ?? [];
  }

  set gameTags(value: GameTagViewModel[]) {
    if (this.game) {
      this.game.gameTags = value;
    }
  }

  get dateRangeString() {
    if (this.dates[0] && !this.dates[1]) {
      if (!this.showingCalendar) {
        this.dates[1] = this.dates[0];
        return `${this.dates[0]}`;
      }
      return `${this.dates[0]} to `;
    }
    if (!this.dates[0] || !this.dates[1]) {
      return "Any Day";
    }
    if (this.dates[0] && this.dates[1]) {
      if (this.dates[0] === this.dates[1]) {
        return `${this.dates[0]}`;
      }
      if (this.dates[0] > this.dates[1]) {
        this.dates.reverse();
      }
    }
    return `${this.dates[0]} to ${this.dates[1]}`;
  }

  get rightSlots(): IComponent[] {
    let likeButton = {
      component: LikeButton,
      props: { game: this.game },
    };
    let editButton = {
      component: EditGameDialog,
      props: { game: this.game },
    };
    return [likeButton, editButton];
  }

  @Watch("game.reviews")
  updateReviewList() {
    this.reviewsListVM.$load();
  }

  @Watch("reviewsPerPage")
  @Watch("dates")
  async updateReviewFilters() {
    this.reviewsListVM.$pageSize = this.reviewsPerPage;
    this.reviewsDataSource.minRating = this.sliderRangeArray[0];
    this.reviewsDataSource.maxRating = this.sliderRangeArray[1];
    this.reviewsDataSource.firstDate = this.dates[0];
    this.reviewsDataSource.secondDate = this.dates[1];
    if (this.reviewsPerPage > 0 && this.game) {
      if (
        (this.dates[0] && this.dates[1]) ||
        (!this.dates[0] && !this.dates[1])
      ) {
        this.reviewsListVM.$pageSize = this.reviewsPerPage;
        this.reviewsDataSource.minRating = this.sliderRangeArray[0];
        this.reviewsDataSource.maxRating = this.sliderRangeArray[1];
        this.reviewsDataSource.firstDate = this.dates[0];
        this.reviewsDataSource.secondDate = this.dates[1];
      }
    }
  }

  @Watch("game.image")
  imageUpdated() {
    //Remove error message if image is updated from empty
    if (this.game.image?.content) {
      this.gameService.getGameImage.wasSuccessful = true;
    }
  }

  resetFilters() {
    this.dates = [];
    this.sliderRangeArray = [0, 5];
    this.reviewsPerPage = 10;
    this.updateReviewList();
  }

  toggleShowEditTags() {
    if (this.$isLoggedIn) {
      this.showEditTags = !this.showEditTags;
    }
    return this.showAddReview;
  }

  toggleAddReview(): boolean {
    if (this.$isLoggedIn) {
      this.showAddReview = !this.showAddReview;
    }
    return this.showAddReview;
  }

  toggleUpdateImageDialog(): boolean {
    if (this.$isAdmin) {
      this.showUpdateImage = !this.showUpdateImage;
    }
    return this.showAddReview;
  }

  isNumeric(value: string) {
    return /^\d+$/.test(value);
  }

  greaterThanZero(value: string) {
    return parseInt(value) > 0;
  }
}
</script>
