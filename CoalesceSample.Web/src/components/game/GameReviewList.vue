<template>
  <v-card>
    <v-card-text>
      <v-list>
        <v-list-item
          v-for="review in reviews"
          :key="review.reviewId"
          class="py-3"
        >
          <game-review :review="review"></game-review>
        </v-list-item>
      </v-list>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import GameReview from "@/components/game/GameReview.vue";
import { Review } from "@/models.g";

@Component({
  components: {
    GameReview,
  },
})
export default class GameReviewList extends Vue {
  @Prop({ required: true })
  reviews!: Review[];
}
</script>
