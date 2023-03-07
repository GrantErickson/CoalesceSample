<template>
  <v-rating
    v-model="localRating"
    :hover="false"
    :readonly="isReadOnly"
    :dense="isDense"
    half-increments
    empty-icon="fa-regular fa-star"
    full-icon="fa-star"
    half-icon="fa-solid fa-star-half-stroke"
  />
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({
  components: {},
})
export default class StarRating extends Vue {
  @Prop({ required: true })
  rating!: number;

  @Prop({ required: false, default: true })
  isReadOnly!: boolean;

  @Prop({ required: false, default: true })
  isDense!: boolean;

  get localRating() {
    return this.rating;
  }

  set localRating(value: number) {
    this.$emit("update:rating", value);
  }
}
</script>
