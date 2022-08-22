import Vue from "vue";

import { ApplicationUserServiceViewModel } from "@/viewmodels.g";

declare module "vue/types/vue" {
  export interface Vue {
    $userService: ApplicationUserServiceViewModel;
    readonly $isLoggedIn: boolean;
    readonly $userRoles: string[];

    $isInRole(role: string): boolean;
  }
}
const applicationUserService = (Vue.prototype.$userService =
  new ApplicationUserServiceViewModel());

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

const interval = 1000 * 60 * 2;
setInterval(() => {
  applicationUserService.getRoles().catch().then();
  console.log("Interval: ", applicationUserService.getRoles.result);
}, interval); // Refresh every 2 minutes.

export default applicationUserService;
