<template>
  <c-loader-status
    v-slot
    class="ma-4"
    :loaders="{
      'no-secondary-progress no-initial-content': [
        $userService.getAllUsersInfo,
        $userService.getRoleList,
      ],
    }"
  >
    <v-container class="pb-4">
      <v-row dense class="align-center pb-4">
        Search users:
        <v-combobox
          v-model="searchUser"
          class="pl-2"
          :items="infoDto.map((item) => item.name + ' (' + item.email + ')')"
          :filter="filterUsers"
          multiple
          deletable-chips
          hide-details
          chips
          clearable
        />
      </v-row>
      <v-row dense class="align-center pb-4">
        Search Roles:
        <v-combobox
          v-model="searchRole"
          class="pl-2"
          full-width
          :items="allRoles"
          item-text="role"
          hide-details
          deletable-chips
          clearable
          multiple
          chips
        />
      </v-row>
    </v-container>
    <v-list class="ma-0 pa-0">
      <v-list-item
        v-for="user in displayedInfoDto"
        :key="user.email"
        class="ma-0 pa-0"
      >
        <card-base class="pa-3">
          <div slot="left">
            <v-row class="ma-3">
              <strong> {{ user.name }} </strong>
            </v-row>
            <v-row class="ma-3">
              {{ user.email }}
            </v-row>
          </div>
          <div slot="main" class="ml-12">
            <v-card flat>
              <v-card-title>Roles</v-card-title>
              <v-card-actions>
                <span v-for="role in allRoles" :key="role" class="ma-3">
                  <v-chip-group v-model="user.userRoles" show-arrows>
                    <v-btn
                      :key="'btnColor' + user.email + role"
                      rounded
                      large
                      :color="
                        user.userRoles.includes(role)
                          ? 'green'
                          : 'red lighten-3'
                      "
                      @click="toggleRole(user, role)"
                    >
                      {{ role }}
                    </v-btn>
                  </v-chip-group>
                </span>
              </v-card-actions>
            </v-card>
          </div>
        </card-base>
      </v-list-item>
    </v-list>
  </c-loader-status>
</template>

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import CardBase from "@/components/templates/CardBase.vue";
import { UserInfoDto } from "@/models.g";

@Component({
  components: { CardBase },
})
export default class UserManagement extends Vue {
  infoDto: UserInfoDto[] = [];
  displayedInfoDto: UserInfoDto[] = [];

  searchUser: string[] = [];
  searchRole: string[] = [];

  allRoles: string[] = [];

  async created() {
    console.log("UserManagement created");
    await this.$userService.getAllUsersInfo();
    await this.$userService.getRoleList();
    this.allRoles = this.$userService.getRoleList.result ?? [];
    this.infoDto = this.$userService.getAllUsersInfo.result ?? [];
    this.displayedInfoDto = this.infoDto;
  }

  @Watch("searchUser")
  @Watch("searchRole")
  onSearchChanged() {
    console.log("search changed");
    if (this.searchUser.length == 0) {
      this.displayedInfoDto = this.infoDto;
    } else {
      this.displayedInfoDto =
        this.infoDto.filter(
          (x) =>
            this.searchUser.filter((su) =>
              !su.includes("(") && !su.includes(")")
                ? x.name?.toLowerCase().includes(su.toLowerCase())
                : x.name?.toLowerCase().includes(su.split(" ")[0].toLowerCase())
            ).length > 0 ||
            this.searchUser.filter((su) =>
              !su.includes("(") && !su.includes(")")
                ? x.email?.toLowerCase().includes(su.toLowerCase())
                : x.email
                    ?.toLowerCase()
                    .includes(su.split(" ")[1].slice(1, -1).toLowerCase())
            ).length > 0
        ) ?? [];
    }
    if (this.searchRole.length > 0) {
      console.log(this.searchRole.length);
      if (this.searchRole.length > 0) {
        console.log("search role", this.searchRole);
        this.displayedInfoDto = this.displayedInfoDto.filter(
          (u) =>
            this.searchRole.filter((r) => u.userRoles?.includes(r)).length > 0
        );
      }
    }
  }

  filterUsers(item: string, queryText: string, itemText: string) {
    const hasValue = (val: string) => val != null;
    const startsWith = (val: string) =>
      val.toLowerCase().includes(queryText.toLowerCase());
    return [item, itemText].some(hasValue) && startsWith(itemText);
  }

  async toggleRole(user: UserInfoDto, role: string) {
    await this.$userService.toggleUserRole(
      user.email,
      role,
      user.userRoles?.includes(role) ?? false
    );
    if (
      this.$userService.toggleUserRole.wasSuccessful ||
      this.$userService.toggleUserRole.message?.includes("already")
    ) {
      if (user.userRoles?.includes(role)) {
        user.userRoles = user.userRoles?.filter((r) => r !== role);
      } else {
        user.userRoles?.push(role);
      }
    }
  }
}
</script>
