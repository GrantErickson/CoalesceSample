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

Object.defineProperty(Vue.prototype, "$isLoggedIn", {
  get() {
    return applicationUserService.hasRole.wasSuccessful ?? false;
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

setInterval(async () => {
  if (document.hidden) {
    // Don't refresh info if the window is minimized or the tab is in the background.
    return;
  }
  // eslint-disable-next-line @typescript-eslint/no-empty-function
  const loggedIn = await applicationUserService.hasRole("user").catch(() => {});
  if (loggedIn) {
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    applicationUserService.getRoles().catch(() => {});
  }
}, 1000 * 60 * 2); // Refresh every 2 minutes.

export default applicationUserService;
