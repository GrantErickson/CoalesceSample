<template>
  <v-row>
    <v-card
      :flat="noCard"
      :color="!noCard ? '' : '#FAFAFA'"
      :class="!noCard ? 'ma-2 pa-2 flex-fill' : 'ma-2 flex-fill'"
    >
      <v-row>
        <v-card flat class="ma-4 mr-2" width="20%">
          <slot name="left" />
        </v-card>
        <v-card flat class="ma-4 flex-fill">
          <div class="float-right ma-2" style="z-index: 1; position: relative">
            <div v-if="rightSlotsList?.length > 0 ?? false">
              <div
                v-for="(slot, i) in rightSlotsList"
                :key="'slot' + i + slot.component.name"
              >
                <v-row class="ma-2">
                  <v-spacer />
                  <component :is="slot.component" v-bind="slot.props" />
                </v-row>
              </div>
            </div>
          </div>
          <v-card v-if="noCard" class="fill-height" style="z-index: 0">
            <slot name="main" />
          </v-card>
          <div v-else>
            <slot name="main" />
          </div>
        </v-card>
      </v-row>
    </v-card>
  </v-row>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import IComponent from "@/services/i-component";

@Component({
  components: {},
})
export default class CardBase extends Vue {
  @Prop({ required: false, default: () => [] })
  rightSlotsList!: IComponent[];

  @Prop({ required: false, default: false })
  noCard!: boolean;

}
</script>
