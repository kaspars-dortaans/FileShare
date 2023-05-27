<template>
  <div class="login-page">
    <div class="login-form">
      <h4>Log In</h4>
      <b-alert v-model="showErrorMessage" variant="danger">{{ errorMessage }}</b-alert>
      <b-form-input v-model="username" placeholder="Username"></b-form-input>
      <b-form-input type="password" v-model="password" placeholder="Password"></b-form-input>
      <b-button variant="primary" @click="submit">Log in</b-button>
      <p>Don't have an account <b-button :to="{ name: 'register' }">register</b-button></p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { authorizationManagerInjectionKey } from '@/authorization/authorization-manager'
import { computed, inject, ref } from 'vue'

const authManager = inject(authorizationManagerInjectionKey)

let username = ref('')
let password = ref('')
let errorMessage = ref('')

const showErrorMessage = computed(() => !!errorMessage.value)

const submit = async () => {
  try {
    await authManager!.login({
      username: username.value,
      password: password.value
    })
  } catch (error) {
    errorMessage.value = (error as Error).message
    username.value = ''
    password.value = ''
  }
}
</script>

<style>
.login-page {
  display: flex;
  height: 100%;
}

.login-form {
  min-width: 18rem;
  margin: auto;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
</style>
