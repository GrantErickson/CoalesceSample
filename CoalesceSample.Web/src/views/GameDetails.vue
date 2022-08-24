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
          <v-card>
            <v-card-text class="pa-0 ma-0">
              <c-loader-status
                v-slot
                :loaders="{
                  'no-loading-content no-secondary-progress no-initial-content no-error-content':
                    [gameService.getGameDetails],
                }"
              >
                <v-img :key="gameImage" :src="gameImage" aspect-ratio="1" />
              </c-loader-status>
            </v-card-text>
            <v-card-actions v-if="$isAdmin">
              <v-spacer />
              <v-btn
                class="ma-1"
                color="primary"
                @click="toggleUpdateImageDialog"
              >
                Change Image
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-col>
        <v-col cols="9">
          <v-card class="fill-height">
            <v-card flat class="float-right">
              <like-button :game="game" />
            </v-card>
            <v-card-title>
              {{ game.name }}
            </v-card-title>
            <v-card-subtitle class="pb-0">
              {{ game.genre.name }}
            </v-card-subtitle>
            <v-card-actions>
              <v-chip-group>
                <v-tooltip v-for="tag in gameTags" :key="tag.tag.name" bottom>
                  <template #activator="{ on, attrs }">
                    <v-chip
                      class="px-2 ma-0"
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
              </v-chip-group>
              <v-btn
                v-if="$isLoggedIn"
                class="ml-2"
                fab
                x-small
                @click="toggleShowEditTags"
              >
                <v-icon>fa-plus</v-icon>
              </v-btn>
            </v-card-actions>
            <v-card-text class="black--text pb-4">
              {{ game.description }}
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12">
          <v-card class="fill-height">
            <v-card-title>
              Reviews
              <v-tooltip bottom>
                <template #activator="{ on, attrs }">
                  <v-btn
                    v-bind="attrs"
                    fab
                    x-small
                    class="ml-4"
                    :ripple="$isLoggedIn"
                    :color="$isLoggedIn ? '' : 'grey'"
                    v-on="on"
                    @click="toggleAddReview"
                  >
                    <v-icon>fa-plus</v-icon>
                  </v-btn>
                </template>
                <span v-if="!$isLoggedIn">
                  You must be logged in to add a review.
                </span>
                <span v-else> Add a review. </span>
              </v-tooltip>
              <v-spacer />
              <v-sheet>
                <v-row class="align-center">
                  <span :key="'noRatings' + game.numberOfRatings">
                    Total Reviews: {{ numberOfRatings }}
                  </span>
                  <span class="pl-8 pr-2"> Average rating: </span>
                  <star-rating
                    :key="'avgRatings' + game.averageRating"
                    class="mr-4"
                    :rating="game.averageRating"
                  />
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
                :key="'revList' + game.numberOfRatings"
                :game.sync="game"
              />
            </c-loader-status>
          </v-card>
        </v-col>
      </v-row>

      <edit-tags-dialog v-model="showEditTags" :game.sync="game" />
      <add-review-dialog v-model="showAddReview" :game.sync="game" />
      <update-image-dialog v-model="showUpdateImage" :game.sync="game" />
    </c-loader-status>
  </v-container>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Game, GameTag } from "@/models.g";
import {
  ApplicationUserServiceViewModel,
  GameServiceViewModel,
} from "@/viewmodels.g";
import GameReviewList from "@/components/game/GameReviewList.vue";
import StarRating from "@/components/StarRating.vue";
import LikeButton from "@/components/LikeButton.vue";
import AddReviewDialog from "@/components/dialogs/AddReviewDialog.vue";
import UpdateImageDialog from "@/components/dialogs/UpdateImageDialog.vue";
import EditTagsDialog from "@/components/dialogs/EditTagsDialog.vue";

@Component({
  components: {
    AddReviewDialog,
    GameReviewList,
    StarRating,
    LikeButton,
    UpdateImageDialog,
    EditTagsDialog,
  },
})
export default class GameDetails extends Vue {
  @Prop({ required: true })
  gameId!: string;

  gameService: GameServiceViewModel = new GameServiceViewModel();

  showUpdateImage = false;

  showAddReview = false;
  showEditTags = false;

  gameDetails: Game | null = null;

  async created() {
    await this.gameService.getGameDetails(this.gameId);
    console.log("error");
    try {
      await this.gameService.getGameImage(this.gameId).catch();
    } catch (e) {
      console.log(e);
    }
    console.log("error2");
  }

  async userRoles() {
    let service = new ApplicationUserServiceViewModel();
    await service.getRoles();
    return service.getRoles.result;
  }

  get game(): Game {
    if (this.gameDetails === null) {
      return this.gameService.getGameDetails.result as Game;
    } else {
      return this.gameDetails as Game;
    }
  }

  set game(value: Game) {
    this.gameDetails = value;
  }

  get numberOfRatings() {
    return this.game?.numberOfRatings;
  }

  get gameTags() {
    return this.game?.gameTags ?? [];
  }

  set gameTags(value: GameTag[]) {
    if (this.game) {
      this.game.gameTags = value;
    }
  }

  toggleShowEditTags() {
    if (this.$isLoggedIn) {
      this.showEditTags = !this.showEditTags;
    }
    return this.showAddReview;
  }

  toggleAddReview(): boolean {
    if (this.$isLoggedIn) {
      this.showAddReview = !this.showAddReview;
    }
    return this.showAddReview;
  }

  toggleUpdateImageDialog(): boolean {
    if (this.$isAdmin) {
      this.showUpdateImage = !this.showUpdateImage;
    }
    return this.showAddReview;
  }

  get gameImage() {
    if (this.gameService.getGameImage.wasSuccessful) {
      this.game.image!.base64Image = this.gameService.getGameImage.result;
    }
    return this.gameService.getGameImage.result ?? "";
  }
}
</script>
