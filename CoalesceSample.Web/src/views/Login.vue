<template>
  <v-container>
    <v-card flat color="grey lighten-5">
      <c-loader-status
        v-slot
        :loaders="{
          'no-secondary-progress no-initial-content no-error-content': [
            loginService.isLoggedIn,
          ],
        }"
      >
        <v-card class="mb-4">
          <v-card-title> You are already signed in. </v-card-title>
        </v-card>
      </c-loader-status>
      <v-row>
        <v-col>
          <v-card>
            <v-tabs v-model="tab" align-with-title>
              <v-tabs-slider color="black"></v-tabs-slider>
              <v-tab> Login </v-tab>
              <v-tab> Register </v-tab>
            </v-tabs>
            <v-tabs-items v-model="tab">
              <v-tab-item>
                <v-card-title> Login </v-card-title>
                <v-card-text>
                  <v-text-field
                    v-model="email"
                    label="Email"
                    required
                    autofocus
                  />
                  <v-text-field
                    v-model="password"
                    label="Password"
                    type="password"
                    required
                  />
                </v-card-text>
                <v-card-actions>
                  <v-radio-group v-model="signInType" row>
                    <v-radio label="JWT" value="jwt" />
                    <v-radio label="Cookie" value="cookie" />
                  </v-radio-group>
                  <v-spacer />
                  <v-btn
                    color="primary"
                    :disabled="!isLoggedIn"
                    @click="logout"
                  >
                    Logout
                  </v-btn>
                  <v-btn
                    color="primary"
                    type="submit"
                    :disabled="(!email || !password) && isLoggedIn"
                    @click="login"
                  >
                    Login
                  </v-btn>
                </v-card-actions>
              </v-tab-item>
              <v-tab-item>
                <v-card-title> Register </v-card-title>
                <v-card width="500" flat color="grey lighten-5">
                  <v-card-text>
                    <v-row>
                      <v-text-field
                        v-model="name"
                        label="Name"
                        required
                        autofocus
                      ></v-text-field>
                    </v-row>
                    <v-row>
                      <v-text-field
                        v-model="email"
                        label="Email"
                        required
                      ></v-text-field>
                    </v-row>
                    <v-row>
                      <v-text-field
                        v-model="password"
                        label="Password"
                        type="password"
                        required
                      ></v-text-field>
                      <v-text-field
                        v-model="password2"
                        label="Confirm Password"
                        type="password"
                        required
                      ></v-text-field>
                    </v-row>
                  </v-card-text>
                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn
                      color="primary"
                      type="submit"
                      :disabled="
                        !name || !email || !password || password !== password2
                      "
                      @click="register"
                    >
                      Register
                    </v-btn>
                  </v-card-actions>
                </v-card>
              </v-tab-item>
            </v-tabs-items>
          </v-card>
        </v-col>
        <v-col>
          <v-card class="fill-height">
            <v-card-title> Login with JWT or Cookies </v-card-title>
            <v-card-text>
              Choose to log in using a JWT or a cookie. Either authentication
              type is supported across this site.
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { LoginServiceViewModel } from "@/viewmodels.g";

@Component({})
export default class Login extends Vue {
  loginService = new LoginServiceViewModel();
  isLoggedIn = false;
  tab = 0;

  name = "";
  email = "";
  password = "";
  password2 = "";

  signInType = "jwt";

  async created() {
    await this.loginService.isLoggedIn();
    this.isLoggedIn = this.loginService.isLoggedIn.wasSuccessful ?? false;
  }

  async login() {
    if (this.signInType === "jwt") {
      console.log("login");
      await this.loginService.getToken(this.email, this.password);
    } else {
      await this.loginService.login(this.email, this.password);
    }
    window.location.reload();
  }
  async register() {
    await this.loginService.createAccount(this.name, this.email, this.password);
    window.location.reload();
  }
  async logout() {
    await this.loginService.logout();
    window.location.reload();
  }
}
</script>
