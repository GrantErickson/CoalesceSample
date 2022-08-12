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
          <v-card height="500" class="fill-height">
            <v-img
              :key="gameImage"
              :src="gameImage"
              aspect-ratio="1"
              contain
            ></v-img>
            <v-file-input v-model="newImage" label="New Image"> </v-file-input>
            <v-btn color="primary" @click="update"> Upload Image </v-btn>
          </v-card>
        </v-col>
        <v-col cols="9">
          <v-card class="fill-height">
            <v-card-title>
              {{ game.name }}
            </v-card-title>
            <v-card-subtitle class="py-0 pb-4">
              {{ game.genre.name }}
            </v-card-subtitle>
            <v-card-text class="black--text">
              {{ game.description }}
            </v-card-text>
            <v-card-actions>
              <v-chip-group>
                <v-tooltip v-for="tag in tags" :key="tag.tag.name" bottom>
                  <template #activator="{ on, attrs }">
                    <v-chip
                      class="pa-2 ma-0"
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
                <v-btn fab x-small>
                  <v-icon>fa-plus</v-icon>
                </v-btn>
              </v-chip-group>
            </v-card-actions>
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
                    :ripple="isLoggedIn"
                    :color="isLoggedIn ? 'primary' : 'grey'"
                    v-on="on"
                    @click="toggleAddReview"
                  >
                    <v-icon>fa-plus</v-icon>
                  </v-btn>
                </template>
                <span v-if="!isLoggedIn">
                  You must be logged in to add a review.
                </span>
                <span v-else> Add a review. </span>
              </v-tooltip>
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
                  gameService.getReviews,
                ],
              }"
            >
              <game-review-list
                :key="'revList' + game.numberOfRatings"
                :reviews="game.reviews"
              ></game-review-list>
            </c-loader-status>
          </v-card>
        </v-col>
      </v-row>

      <v-dialog v-model="showAddReview" width="800">
        <v-card>
          <v-card-title> Write a review for: {{ game.name }}</v-card-title>

          <v-card-text class="align-center justify-center pb-4">
            <v-text-field
              v-model="reviewTitle"
              label="Review Title"
              :counter="50"
              maxlength="50"
              required
            ></v-text-field>

            <v-card-title class="text-h8">
              <span>Rating:</span>
              <star-rating
                :rating="reviewRating"
                :is-read-only="false"
                :is-dense="false"
              ></star-rating>
            </v-card-title>

            <v-textarea
              v-model="reviewBody"
              label="Review Contents"
              :counter="800"
              maxlength="800"
              required
            ></v-textarea>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn color="primary" @click="clearReview">Cancel</v-btn>
            <v-btn color="primary" @click="addReview">Submit</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </c-loader-status>
  </v-container>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { Game } from "@/models.g";
import {
  GameServiceViewModel,
  LoginServiceViewModel,
  ReviewServiceViewModel,
} from "@/viewmodels.g";
import GameReviewList from "@/components/game/GameReviewList.vue";
import StarRating from "@/components/StarRating.vue";

@Component({
  components: { GameReviewList, StarRating },
})
export default class GameDetails extends Vue {
  @Prop({ required: true })
  gameId!: number;

  gameService: GameServiceViewModel = new GameServiceViewModel();
  reviewService = new ReviewServiceViewModel();
  loginService = new LoginServiceViewModel();

  newImage: File | null = null;
  gameImage = "";

  isLoggedIn = false;

  showAddReview = false;
  reviewTitle = "";
  reviewRating = 0;
  reviewBody = "";

  update() {
    console.log("update");
    if (this.newImage !== null) {
      console.log("running");
      this.gameService.uploadGameImage(this.gameId, this.newImage);
    }
    this.getGameImage();
  }
  async created() {
    await this.gameService.getGameDetails(this.gameId);
    await this.loginService.isLoggedIn();
    this.isLoggedIn = this.loginService.isLoggedIn.wasSuccessful ?? false;

    await this.getGameImage();
  }

  get game(): Game | null {
    return this.gameService.getGameDetails.result;
  }

  get numberOfRatings() {
    return this.game?.numberOfRatings;
  }

  get tags() {
    return this.game?.gameTags;
  }

  async toggleAddReview() {
    if (this.isLoggedIn) {
      this.showAddReview = !this.showAddReview;
    }
  }

  async getGameImage() {
    if (this.game == null) {
      return;
    }
    try {
      await this.gameService.getGameImage(this.gameId);
      if (this.gameService.getGameImage.wasSuccessful) {
        this.gameImage = this.gameService.getGameImage.result!;
      }
    } catch (e) {
      this.gameImage = "";
    }
  }

  clearReview() {
    this.reviewTitle = "";
    this.reviewRating = 0;
    this.reviewBody = "";
    this.showAddReview = false;
  }

  async addReview() {
    if (this.game === null) {
      return;
    }
    await this.reviewService.addReview(
      this.gameId,
      this.reviewTitle,
      this.reviewBody,
      this.reviewRating
    );
    if (this.reviewService.addReview.wasSuccessful) {
      this.game.reviews!.push(this.reviewService.addReview.result!);
      this.game.numberOfRatings!++;
      this.game.averageRating =
        (this.game.averageRating! + this.reviewRating) / 2;
      this.clearReview();
    }
    this.clearReview();
  }
}
</script>
