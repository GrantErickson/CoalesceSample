<template>
  <v-btn v-if="isReviewOwnerOrAdmin" x-small @click="confirmDelete = true">
    <v-icon small color="red">fa-trash-can</v-icon>
    <v-dialog v-model="confirmDelete" width="500">
      <v-card>
        <v-card-title>
          Are you sure you want to delete this review?
        </v-card-title>
        <v-card-actions>
          <v-spacer />
          <v-btn color="primary" @click="deleteReview">Yes</v-btn>
          <v-btn text @click="confirmDelete = false">No</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-btn>
</template>

<script lang="ts">
import { Component, PropSync, Vue } from "vue-property-decorator";
import { Game, Review } from "@/models.g";
import { ReviewServiceViewModel } from "@/viewmodels.g";

@Component
export default class DeleteGameReview extends Vue {
  @PropSync("ReviewSync", { required: true })
  review!: Review;
  @PropSync("GameSync", { required: true })
  game!: Game;

  reviewService = new ReviewServiceViewModel();

  confirmDelete = false;

  get isReviewOwnerOrAdmin() {
    return this.$isAdmin || this.$userReviews.includes(this.review.reviewId!);
  }

  async deleteReview() {
    await this.reviewService.deleteReview(this.review.reviewId);
    if (this.reviewService.deleteReview.wasSuccessful) {
      this.game.reviews = this.game.reviews!.filter(
        (r) => r.reviewId !== this.review.reviewId
      );
      this.game.numberOfRatings!--;
      this.game.totalRating! -= this.review.rating!;
      this.game.averageRating! =
        this.game.totalRating! / this.game.numberOfRatings!;
      this.$emit("update:game", this.game);
    }
    this.confirmDelete = false;
  }
}
</script>
