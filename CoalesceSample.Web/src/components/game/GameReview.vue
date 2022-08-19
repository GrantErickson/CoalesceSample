<template>
  <v-card width="100%" class="pa-3">
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
  </v-card>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Review } from "@/models.g";
import StarRating from "@/components/StarRating.vue";

@Component({
  components: { StarRating },
})
export default class GameReview extends Vue {
  @Prop({ required: true })
  review!: Review;
}
</script>
