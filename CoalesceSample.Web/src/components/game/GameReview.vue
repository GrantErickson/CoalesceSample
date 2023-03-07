<template>
  <card-base v-if="!review.isDeleted" :right-slots-list="rightSlots">
    <span slot="left">
      <v-card-text>
        <v-row>
          <v-card-title class="pb-0">
            {{ review.reviewerName }}
          </v-card-title>
          <v-card-text>
            {{ formattedDate }}
          </v-card-text>
        </v-row>
        <v-row>
          <v-card-title>
            <star-rating class="mr-4" :rating="review.rating" />
          </v-card-title>
        </v-row>
      </v-card-text>
    </span>
    <span slot="main">
      <v-card-text>
        <v-card-title>{{ review.reviewTitle }}</v-card-title>
        <v-card-text>{{ review.reviewBody }}</v-card-text>
      </v-card-text>
    </span>
  </card-base>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Game, Review } from "@/models.g";
import StarRating from "@/components/StarRating.vue";
import { ReviewServiceViewModel } from "@/viewmodels.g";
import CardBase from "@/components/templates/CardBase.vue";
import DeleteGameReview from "@/components/DeleteGameReview.vue";

@Component({
  components: { CardBase, StarRating },
})
export default class GameReview extends Vue {
  @Prop({ required: true })
  review!: Review;

  @Prop({ required: true })
  game!: Game;

  reviewService = new ReviewServiceViewModel();

  get formattedDate() {
    const date = new Date(this.review.reviewDate!);
    const time = date.toLocaleTimeString();
    return this.review.reviewDate?.toLocaleDateString() + "\n" + time;
  }

  get rightSlots() {
    return [
      {
        component: DeleteGameReview,
        props: {
          ReviewSync: this.review,
          GameSync: this.game,
        },
      },
    ];
  }
}
</script>
