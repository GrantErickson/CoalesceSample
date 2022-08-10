<template>
  <v-card width="100%">
    <v-card-title>{{ review.reviewTitle }}</v-card-title>
    <v-row>
      <v-col cols="3">
        <v-card-title>
          <v-row>
            {{ review.reviewerName }}
          </v-row>
          <v-row>
            <v-rating></v-rating>
          </v-row>
        </v-card-title>
      </v-col>
      <v-col cols="8">
        <v-card-text>{{ review.reviewBody }}</v-card-text>
      </v-col>
    </v-row>
  </v-card>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Review } from "@/models.g";

@Component({
  components: {},
})
export default class GameReview extends Vue {
  @Prop({ required: true })
  review!: Review;
}
</script>
