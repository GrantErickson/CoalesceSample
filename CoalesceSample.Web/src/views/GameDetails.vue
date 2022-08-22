<template>
  <v-container>
    <c-loader-status
      v-slot
      :loaders="{
        'no-loading-content no-secondary-progress no-initial-content no-error-content':
          [gameList.$load],
      }"
    >
      <v-row>
        <v-col cols="3">
          <v-card>
            <v-card-text class="pa-0 ma-0">
              <v-img :key="gameImage" :src="gameImage" aspect-ratio="1" />
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
              <v-card-title>
                <span :key="'likes' + game.likes">Likes: {{ game.likes }}</span>
                <v-btn class="mx-3" fab x-small @click="toggleLike">
                  <v-icon
                    :color="hasLiked ? 'primary' : 'secondary'"
                    class="fa fa-thumbs-up"
                  />
                </v-btn>
              </v-card-title>
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
                v-if="$isAdmin"
                class="ml-2"
                fab
                x-small
                @click="showEditTags = true"
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
                :reviews="game.reviews"
              />
            </c-loader-status>
          </v-card>
        </v-col>
      </v-row>

      <v-dialog v-model="showEditTags" width="800">
        <v-card>
          <v-card-title> Edit Tags </v-card-title>
          <v-card-text>
            <c-loader-status
              v-slot
              :loaders="{
                'no-secondary-progress no-initial-content no-error-content': [
                  tags.$load,
                ],
              }"
            >
              <v-autocomplete
                v-model="gameTagIds"
                :items="tags.$items"
                item-text="name"
                item-value="tagId"
                label="Tags"
                hide-details
                multiple
                chips
              >
                <template slot="item" slot-scope="{ item }">
                  <v-chip
                    :key="item.tagId"
                    :value="item.tagId"
                    color="primary"
                    dark
                    small
                  >
                    {{ item.name }}
                  </v-chip>
                </template>
              </v-autocomplete>
            </c-loader-status>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn color="primary" @click="saveGameTags"> Save </v-btn>
            <v-btn color="grey" @click="showEditTags = false"> Cancel </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <v-dialog v-model="showUpdateImage" width="800">
        <v-card>
          <v-card-title> Update Image</v-card-title>
          <v-card-text>
            <v-file-input v-model="newImage" label="New Image" />
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn color="primary" @click="updateImage"> Update Image</v-btn>
            <v-btn
              color="primary"
              class="px-3"
              flat
              text
              @click="showUpdateImage = false"
            >
              Close
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <v-dialog v-model="showAddReview" width="800">
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
            <v-btn color="primary" @click="clearReview">Cancel</v-btn>
            <v-btn color="primary" @click="addReview">Submit</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </c-loader-status>
  </v-container>
</template>

<script lang="ts">
import { Component, Inject, Prop, Vue } from "vue-property-decorator";
import { Game, GameTag } from "@/models.g";
import {
  ApplicationUserServiceViewModel,
  GameListViewModel,
  GameServiceViewModel,
  LoginServiceViewModel,
  ReviewServiceViewModel,
  TagListViewModel,
} from "@/viewmodels.g";
import GameReviewList from "@/components/game/GameReviewList.vue";
import StarRating from "@/components/StarRating.vue";

@Component({
  components: { GameReviewList, StarRating },
})
export default class GameDetails extends Vue {
  @Prop({ required: true })
  gameId!: number;

  gameService: GameServiceViewModel = new GameServiceViewModel();
  reviewService = new ReviewServiceViewModel();
  loginService = new LoginServiceViewModel();

  showUpdateImage = false;
  newImage: File | null = null;
  gameImage = "";

  showEditTags = false;
  tags = new TagListViewModel();
  gameTagIds: number[] = [];

  showAddReview = false;
  reviewTitle = "";
  reviewRating = 0;
  reviewBody = "";

  hasLiked = false;

  @Inject("GAMESLIST")
  gameList!: GameListViewModel;

  async created() {
    if (this.gameList.$load.wasSuccessful == null) {
      await this.gameList.$load();
    }

    await this.getGameImage();

    this.tags.$pageSize = 1000;
    await this.tags.$load();
    this.gameTagIds = this.game?.gameTags!.map((tag) => tag.tagId!) ?? [];
    this.hasLiked =
      localStorage.getItem("liked-game-" + this.gameId) === "true" ?? false;
  }

  async userRoles() {
    let service = new ApplicationUserServiceViewModel();
    await service.getRoles();
    return service.getRoles.result;
  }

  get game(): Game | null {
    return this.gameList.$items.filter((g) => g.gameId === this.gameId)[0];
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

  async toggleLike() {
    if (!this.hasLiked) {
      await this.gameService.addLike(this.gameId);
      if (this.gameService.addLike.wasSuccessful) {
        localStorage.setItem("liked-game-" + this.gameId, "true");
        this.hasLiked = true;
        if (this.game && this.game.likes) {
          this.game.likes++;
        }
      }
    } else {
      await this.gameService.removeLike(this.gameId);
      if (this.gameService.removeLike.wasSuccessful) {
        localStorage.setItem("liked-game-" + this.gameId, "false");
        this.hasLiked = false;
        if (this.game && this.game.likes) {
          this.game.likes--;
        }
      }
    }
  }

  async saveGameTags() {
    this.gameService.setGameTags(this.gameId, this.gameTagIds);
    await this.gameService.getGameTags(this.gameId);
    if (this.game) {
      this.game.gameTags = this.gameService.getGameTags.result ?? [];
    }
  }

  toggleAddReview() {
    if (this.$isLoggedIn) {
      this.showAddReview = !this.showAddReview;
    }
  }

  toggleUpdateImageDialog() {
    this.showUpdateImage = !this.showUpdateImage;
  }

  async updateImage() {
    if (this.newImage !== null) {
      this.gameService.uploadGameImage(this.gameId, this.newImage);
    }
    this.showUpdateImage = false;
    await this.getGameImage();
  }

  async getGameImage() {
    if (this.game == null) {
      return;
    }
    try {
      await this.gameService.getGameImage(this.gameId);
      if (this.gameService.getGameImage.wasSuccessful) {
        this.gameImage = this.gameService.getGameImage.result!;
      }
    } catch (e) {
      this.gameImage = "";
    }
  }

  clearReview() {
    this.reviewTitle = "";
    this.reviewRating = 0;
    this.reviewBody = "";
    this.showAddReview = false;
  }

  async addReview() {
    if (this.game === null) {
      return;
    }
    await this.reviewService.addReview(
      this.gameId,
      this.reviewTitle,
      this.reviewBody,
      this.reviewRating
    );
    if (this.reviewService.addReview.wasSuccessful) {
      this.game.reviews!.push(this.reviewService.addReview.result!);
      this.game.numberOfRatings!++;
      this.game.averageRating =
        (this.game.averageRating! + this.reviewRating) / 2;
      this.clearReview();
    }
    this.clearReview();
  }
}
</script>
