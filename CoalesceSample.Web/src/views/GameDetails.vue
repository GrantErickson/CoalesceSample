<template>
  <v-container>
    <c-loader-status
      v-slot
      :loaders="{
        'no-loading-content no-secondary-progress no-initial-content no-error-content':
          [gameService.getGameDetails],
      }"
    >
      <v-row>
        <v-col cols="3">
          <v-card>
            <v-card-text class="pa-0 ma-0">
              <c-loader-status
                v-slot
                :loaders="{
                  'no-loading-content no-secondary-progress no-initial-content no-error-content':
                    [gameService.getGameDetails],
                }"
              >
                <v-img :key="gameImage" :src="gameImage" aspect-ratio="1" />
              </c-loader-status>
            </v-card-text>
            <v-card-actions v-if="$isAdmin">
              <v-spacer />
              <v-btn
                class="ma-1"
                color="primary"
                @click="toggleUpdateImageDialog"
              >
                Change Image
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-col>
        <v-col cols="9">
          <v-card class="fill-height">
            <v-card flat class="float-right">
              <like-button :game="game" />
              <v-sheet flat class="float-right mt-8 mx-6">
                <v-btn
                  v-if="$isAdmin"
                  fab
                  x-small
                  @click.native.stop="toggleShowEditGame"
                >
                  <v-icon color="red"> fa-pencil </v-icon>
                </v-btn>
              </v-sheet>
            </v-card>
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
                      class="px-2 ma-0"
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
          </v-card>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12">
          <v-card class="fill-height">
            <v-card-title>
              Reviews
              <v-tooltip bottom>
                <template #activator="{ on, attrs }">
                  <v-btn
                    v-bind="attrs"
                    fab
                    x-small
                    class="ml-4"
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
                v-model="page"
                class="ml-6"
                :length="pages"
                prev-icon="fa-chevron-left"
                next-icon="fa-chevron-right"
              />
              <span class="pa-0 ma-1 text-h6">Reviews Per Page:</span>
              <v-card flat width="50">
                <v-text-field
                  v-model="reviewsPerPage"
                  placeholder=""
                  :rules="[isNumeric, greaterThanZero]"
                />
              </v-card>
              <v-menu right offset-x :close-on-content-click="false">
                <template #activator="{ on, attrs }">
                  <v-btn
                    v-bind="attrs"
                    color="primary"
                    class="ml-2"
                    x-small
                    fab
                    v-on="on"
                  >
                    <v-icon>fa-filter</v-icon>
                  </v-btn>
                </template>
                <v-sheet width="400" class="ma-3 pt-4">
                  <v-row dense>
                    <v-btn
                      fab
                      x-small
                      class="mr-3"
                      @click="
                        sliderRangeArray = [0, 5];
                        updateReviewList();
                      "
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
                      @mouseup="updateReviewList"
                    />
                  </v-row>
                </v-sheet>
              </v-menu>
              <v-spacer />
              <v-sheet>
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
                'no-secondary-progress no-initial-content': [
                  reviewService.getReviews,
                ],
              }"
            >
              <game-review-list
                :key="'revList' + game.numberOfRatings"
                :game.sync="game"
                :reviews.sync="reviewsList"
              />
            </c-loader-status>
          </v-card>
        </v-col>
      </v-row>

      <edit-tags-dialog v-model="showEditTags" :game.sync="game" />
      <add-review-dialog v-model="showAddReview" :game.sync="game" />
      <update-image-dialog v-model="showUpdateImage" :game.sync="game" />
      <edit-game-dialog v-model="showEditGame" :game="game" />
    </c-loader-status>
  </v-container>
</template>

<script lang="ts">
import { Component, Prop, Provide, Vue, Watch } from "vue-property-decorator";
import { Game, GameTag, Review } from "@/models.g";
import { GameServiceViewModel, ReviewServiceViewModel } from "@/viewmodels.g";
import GameReviewList from "@/components/game/GameReviewList.vue";
import StarRating from "@/components/StarRating.vue";
import LikeButton from "@/components/LikeButton.vue";
import AddReviewDialog from "@/components/dialogs/AddReviewDialog.vue";
import UpdateImageDialog from "@/components/dialogs/UpdateImageDialog.vue";
import EditTagsDialog from "@/components/dialogs/EditTagsDialog.vue";
import EditGameDialog from "@/components/dialogs/EditGameDialog.vue";

@Component({
  components: {
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
  @Provide("REVIEW_SERVICE")
  reviewService: ReviewServiceViewModel = new ReviewServiceViewModel();

  showUpdateImage = false;

  showAddReview = false;
  showEditTags = false;
  showEditGame = false;

  gameDetails: Game | null = null;

  reviewsList: Review[] = [];
  reviewsPerPage = 10;
  page = 1;
  pages = 1;
  rangeTickLabels = ["0", "", "1", "", "2", "", "3", "", "4", "", "5"];
  sliderRangeArray: number[] = [0, 5];

  async created() {
    await this.gameService.getGameDetails(this.gameId);
    await this.gameService.getGameImage(this.gameId);
    await this.updateReviewList();
  }

  get game(): Game {
    if (this.gameDetails === null) {
      return this.gameService.getGameDetails.result as Game;
    } else {
      return this.gameDetails as Game;
    }
  }

  set game(value: Game) {
    this.gameDetails = value;
  }

  get numberOfRatings() {
    return this.game?.numberOfRatings;
  }

  get gameTags() {
    return this.game?.gameTags ?? [];
  }

  set gameTags(value: GameTag[]) {
    if (this.game) {
      this.game.gameTags = value;
    }
  }

  @Watch("reviewsPerPage")
  @Watch("page")
  @Watch("game.reviews")
  async updateReviewList() {
    if (this.reviewsPerPage > 0) {
      if (this.reviewsPerPage) {
        await this.reviewService.getReviews(
          this.gameId,
          this.page,
          this.reviewsPerPage,
          this.sliderRangeArray[0],
          this.sliderRangeArray[1]
        );
        this.pages = Math.max(
          1,
          Math.ceil((this.game?.numberOfRatings ?? 1) / this.reviewsPerPage)
        );

        this.page = Math.max(1, this.page);
        this.page = Math.min(this.page, this.pages);
        this.reviewsList = this.reviewService.getReviews.result ?? [];
      }
    }
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

  toggleShowEditGame() {
    if (this.$isAdmin) {
      this.showEditGame = !this.showEditGame;
    }
    return this.showEditGame;
  }

  get gameImage() {
    if (this.gameService.getGameImage.wasSuccessful) {
      this.game.image!.base64Image = this.gameService.getGameImage.result;
    }
    return this.gameService.getGameImage.result ?? "";
  }

  isNumeric(value: string) {
    return /^\d+$/.test(value);
  }
  greaterThanZero(value: string) {
    return parseInt(value) > 0;
  }
}
</script>
