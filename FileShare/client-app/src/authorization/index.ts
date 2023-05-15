import type { AxiosStatic } from 'axios'
import type { App } from 'vue'
import type { Router } from 'vue-router'
import { AuthorizationManager, authorizationManagerInjectionKey } from './authorization-manager'

export default {
  install: (app: App, options: { axios: AxiosStatic; router: Router }) => {
    app.provide(
      authorizationManagerInjectionKey,
      new AuthorizationManager(options.axios, options.router)
    )
  }
}
