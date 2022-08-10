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
          <v-card height="250" class="fill-height">
            <c-loader-status
              v-slot
              :loaders="{
                'no-secondary-progress no-initial-content': [
                  gameService.getGameImage,
                ],
              }"
            >
              <v-img :src="gameImage"></v-img>
            </c-loader-status>
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
                <v-tooltip v-for="tag in tags" :key="tag.name" bottom>
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
                <v-icon>fa-plus</v-icon>
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
              <v-btn fab x-small class="ml-4" @click="toggleAddReview">
                <v-icon>fa-plus</v-icon>
              </v-btn>
              <v-spacer />
              <v-sheet>
                <v-row class="align-center">
                  <span> Total Reviews: {{ numberOfRatings }} </span>
                  <span class="pl-8 pr-2"> Average rating: </span>
                  <v-rating
                    :key="game.averageRating"
                    v-model="game.averageRating"
                    dense
                    class="mr-4"
                    half-increments
                    readonly
                  ></v-rating>
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
                :key="gameReviews.length"
                :reviews="gameReviews"
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
              <v-rating v-model="reviewRating" :hover="false" half-increments>
              </v-rating>
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
import { Component, Prop, Vue } from "vue-property-decorator";
import { Game, Review } from "@/models.g";
import { GameServiceViewModel, ReviewServiceViewModel } from "@/viewmodels.g";
import GameReviewList from "@/components/game/GameReviewList.vue";

@Component({
  components: { GameReviewList },
})
export default class GameDetails extends Vue {
  @Prop({ required: true })
  gameId!: number;

  game!: Game;
  gameReviews!: Review[];
  gameService = new GameServiceViewModel();
  reviewService = new ReviewServiceViewModel();

  showAddReview = false;
  reviewTitle = "";
  reviewRating = 0;
  reviewBody = "";

  async created() {
    await this.gameService.getGameDetails(this.gameId);
    if (
      this.gameService.getGameDetails.wasSuccessful &&
      this.gameService.getGameDetails.result
    ) {
      this.game = this.gameService.getGameDetails.result;
      this.gameReviews = this.gameService.getGameDetails.result.reviews ?? [];
      await this.gameService.getGameImage(this.game.gameId).catch((x) => {
        console.table(x);
      });
    }
  }

  get numberOfRatings() {
    return this.game.numberOfRatings;
  }

  get tags() {
    return this.game.gameTags;
  }

  get gameImage() {
    return this.gameService.getGameImage.result;
  }

  toggleAddReview() {
    this.showAddReview = !this.showAddReview;
  }

  clearReview() {
    this.reviewTitle = "";
    this.reviewRating = 0;
    this.reviewBody = "";
    this.showAddReview = false;
  }

  async addReview() {
    await this.reviewService.addReview(
      this.game.gameId,
      this.reviewTitle,
      this.reviewBody,
      this.reviewRating
    );
    await this.reviewService.getReviews(this.gameId);
    this.gameReviews = this.reviewService.getReviews.result ?? [];
    this.game.reviews = this.gameReviews;
    this.clearReview();
  }
}
</script>
