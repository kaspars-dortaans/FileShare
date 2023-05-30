<script setup lang="ts">
import { RouterView } from 'vue-router'
import { authorizationManagerInjectionKey } from './authorization/authorization-manager'
import { computed, inject } from 'vue'
import { Role } from '@/common/enums/role'

const authManager = inject(authorizationManagerInjectionKey)!
const showAdminPages = computed(() => authManager.userInfo.value?.role == Role.Admin)
</script>

<template>
  <b-nav tabs>
    <b-nav-item :to="{ name: 'home' }">Home</b-nav-item>
    <template v-if="authManager.isLogedIn.value">
      <template v-if="showAdminPages">
        <b-nav-item :to="{ name: 'settings' }">Settings</b-nav-item>
      </template>
      <b-nav-item :to="{ name: 'files' }">Files</b-nav-item>
      <b-nav-item @click="authManager.logout()">
        Log Out
      </b-nav-item>
    </template>
    <b-nav-item v-else :to="{ name: 'login' }">Log In</b-nav-item>
  </b-nav>
  <RouterView />
</template>
