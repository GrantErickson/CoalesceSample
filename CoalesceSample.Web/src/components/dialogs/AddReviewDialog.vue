<template>
  <v-dialog :value="value" width="800" @input="close">
    <v-card>
      <v-card-title> Write a review for: {{ game.name }}</v-card-title>

      <v-card-text class="align-center justify-center pb-4">
        <v-text-field
          v-model="reviewTitle"
          label="Review Title"
          :counter="50"
          maxlength="50"
          required
        />

        <v-card-title class="text-h8">
          <span>Rating:</span>
          <star-rating
            :rating.sync="reviewRating"
            :is-read-only="false"
            :is-dense="false"
          />
        </v-card-title>

        <v-textarea
          v-model="reviewBody"
          label="Review Contents"
          :counter="800"
          maxlength="800"
          required
        />
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn
          color="primary"
          :disabled="!reviewTitle || !reviewBody"
          @click="addReview"
        >
          Submit
        </v-btn>
        <v-btn text @click="clearReview"> Close </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Game } from "@/models.g";
import { ReviewServiceViewModel } from "@/viewmodels.g";
import StarRating from "@/components/StarRating.vue";
@Component({
  components: {
    StarRating,
  },
})
export default class AddReviewDialog extends Vue {
  @Prop({ required: true })
  value!: boolean;

  @Prop({ required: true })
  game!: Game;

  reviewService = new ReviewServiceViewModel();

  reviewTitle = "";
  reviewRating = 0;
  reviewBody = "";

  close() {
    this.$emit("input", false);
  }

  clearReview() {
    this.reviewTitle = "";
    this.reviewRating = 0;
    this.reviewBody = "";
    this.close();
  }

  async addReview() {
    await this.reviewService.addReview(
      this.game.gameId,
      this.reviewTitle,
      this.reviewBody,
      this.reviewRating
    );
    if (this.reviewService.addReview.wasSuccessful) {
      this.game.reviews!.push(this.reviewService.addReview.result!);
      this.game.numberOfRatings!++;
      this.game.totalRating! += this.reviewRating;
      this.game.averageRating =
        this.game.totalRating! / this.game.numberOfRatings!;
      this.clearReview();
      this.$userReviews.push(this.reviewService.addReview.result!.reviewId!);
      this.$emit("update:game", this.game);
    }
  }
}
</script>
