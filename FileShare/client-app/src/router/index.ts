import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import LogInView from '@/views/LogInView.vue'
import SettingsView from '@/views/SettingsView.vue'
import RegisterView from '@/views/RegisterView.vue'
import FileView from '@/views/FileView.vue'

export const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/login',
    name: 'login',
    component: LogInView
  },
  {
    path: '/settings',
    name: 'settings',
    component: SettingsView
  },
  {
    path: '/register',
    name: 'register',
    component: RegisterView
  },
  {
    path: '/files',
    name: 'files',
    component: FileView
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes
})

export default router
