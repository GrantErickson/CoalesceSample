<template>
  <v-card v-if="!review.isDeleted" width="100%" class="pa-3">
    <v-card flat class="float-right">
      <v-btn v-if="isReviewOwnerOrAdmin" x-small @click="confirmDelete = true">
        <v-icon small color="red">fa-trash-can</v-icon>
      </v-btn>
    </v-card>
    <v-row>
      <v-col cols="2" sm="3">
        <v-card-text>
          <v-row>
            <v-card-title>
              {{ review.reviewerName }}
            </v-card-title>
          </v-row>
          <v-row>
            <v-card-title>
              <star-rating class="mr-4" :rating="review.rating" />
            </v-card-title>
          </v-row>
        </v-card-text>
      </v-col>
      <v-col cols="8">
        <v-card-title>{{ review.reviewTitle }}</v-card-title>
        <v-card-text>{{ review.reviewBody }}</v-card-text>
      </v-col>
    </v-row>
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
  </v-card>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Game, Review } from "@/models.g";
import StarRating from "@/components/StarRating.vue";
import { ReviewServiceViewModel } from "@/viewmodels.g";

@Component({
  components: { StarRating },
})
export default class GameReview extends Vue {
  @Prop({ required: true })
  review!: Review;

  @Prop({ required: true })
  game!: Game;

  confirmDelete = false;

  reviewService = new ReviewServiceViewModel();

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
