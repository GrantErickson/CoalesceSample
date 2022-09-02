import Vue from "vue";

import { ApplicationUserServiceViewModel } from "@/viewmodels.g";

declare module "vue/types/vue" {
  export interface Vue {
    $userService: ApplicationUserServiceViewModel;
    $userReviews: string[];
    readonly $isAdmin: boolean;
    readonly $isLoggedIn: boolean;
    readonly $userRoles: string[];

    $isInRole(role: string): boolean;
  }
}

const applicationUserService = (Vue.prototype.$userService =
  new ApplicationUserServiceViewModel());

Object.defineProperty(Vue.prototype, "$userReviews", {
  get() {
    return applicationUserService.getUserReviews.result;
  },
  set(value: string[]) {
    this.$userReviews = value;
  },
});

Object.defineProperty(Vue.prototype, "$isAdmin", {
  get() {
    return (
      (applicationUserService.getRoles.wasSuccessful &&
        applicationUserService.getRoles.result?.includes("SuperAdmin")) ??
      false
    );
  },
});

Object.defineProperty(Vue.prototype, "$isLoggedIn", {
  get() {
    return (
      (applicationUserService.getRoles.wasSuccessful &&
        applicationUserService.getRoles.result?.includes("User")) ??
      false
    );
  },
});

Object.defineProperty(Vue.prototype, "$userRoles", {
  get() {
    return applicationUserService.getRoles.result ?? [];
  },
});

export const isInRole = (Vue.prototype.$isInRole = (role: string) => {
  return (
    applicationUserService.getRoles.result?.filter((r) => r === role) ?? false
  );
});

const interval = 1000 * 60 * 0.333; // Refresh every 2 minutes.
setInterval(() => {
  console.log(" --------------------- Interval");
  applicationUserService
    .isLoggedIn()
    .catch((e) => console.log(e))
    .then((data) => {
      console.log("**********IsLoggedInResult", data);
    });
  applicationUserService.getRoles().catch().then();
  applicationUserService.getUserReviews().catch().then();
  console.log("Interval --------------------- ");
}, interval);

export default applicationUserService;
