<script setup lang="ts">
import { RouterView } from 'vue-router'
import { authorizationManagerInjectionKey } from './authorization/authorization-manager'
import { inject } from 'vue'

const authManager = inject(authorizationManagerInjectionKey)!
</script>

<template>
  <b-nav tabs>
    <b-nav-item :to="{ name: 'home' }">Home</b-nav-item>
    <b-nav-item v-if="authManager.isLogedIn.value" @click="authManager.logout()"
      >Log Out</b-nav-item
    >
    <b-nav-item v-else :to="{ name: 'login' }">Log In</b-nav-item>
    <b-nav-item :to="{ name: 'settings' }">Settings</b-nav-item>
  </b-nav>
  <RouterView />
</template>
