import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import BootstrapVueNext from 'bootstrap-vue-next'
import Vue3Toasity, { type ToastContainerOptions } from 'vue3-toastify'
import axios from 'axios'
import AuthorizationManager from './authorization'

import { defineValidationRules } from '@/validators'

//css
//bootstrap
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue-next/dist/bootstrap-vue-next.css'
//toastify
import 'vue3-toastify/dist/index.css'
//project css
import './assets/main.css'

const app = createApp(App)

app.use(BootstrapVueNext)
app.use(Vue3Toasity, {
  autoClose: 5000,
  style: {
    opacity: '1',
    userSelect: 'initial'
  }
} as ToastContainerOptions)

app.use(router)

app.use(AuthorizationManager, { axios: axios, router: router })

defineValidationRules()

app.mount('#app')
