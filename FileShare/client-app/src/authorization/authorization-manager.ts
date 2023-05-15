import type { AuthenticateRequest } from '@/common/interfaces/view-models/user/authenticate-request'
import type { AuthenticateResponse } from '@/common/interfaces/view-models/user/authenticate-response'
import { ApiError } from '@/services/api-error'
import type { AxiosError, AxiosStatic } from 'axios'
import { ref, type InjectionKey } from 'vue'
import type { Router } from 'vue-router'

const loginUrl = 'users/Authenticate'
const tokenKey = 'authToken'
const userInformationKey = 'userInfo'

const loginRedirectRoute = 'home'
const logoutRedirectRoute = 'home'
const forbidenOrUnauthorizedRedirect = 'home'

export class AuthorizationManager {
  axios: AxiosStatic
  router: Router
  isLogedIn = ref(false)

  constructor(axios: AxiosStatic, router: Router) {
    this.axios = axios
    this.router = router

    axios.interceptors.request.use((config) => {
      const token = window.localStorage.getItem(tokenKey)
      if (token) {
        config.headers['Authorization'] = `Bearer ${token}`
      }
      return config
    })

    axios.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response) {
          const status = error.response.status
          if (status === 401 || status === 403) {
            this.router.push({ name: forbidenOrUnauthorizedRedirect })
          }
        }
        return Promise.reject(error)
      }
    )

    this._checkIfLogedIn()
  }

  async login(request: AuthenticateRequest) {
    try {
      const response = (await this.axios.post(loginUrl, request)).data as AuthenticateResponse
      window.localStorage.setItem(tokenKey, response.token)
      window.localStorage.setItem(userInformationKey, JSON.stringify(response.userData))

      if (loginRedirectRoute) {
        this.router.push({ name: loginRedirectRoute })
      }
    } catch (error) {
      throw new ApiError(error as AxiosError)
    } finally {
      this._checkIfLogedIn()
    }
  }

  logout() {
    window.localStorage.removeItem(tokenKey)
    window.localStorage.removeItem(userInformationKey)
    if (logoutRedirectRoute) {
      this.router.push({ name: logoutRedirectRoute })
    }
    this._checkIfLogedIn()
  }

  _checkIfLogedIn() {
    this.isLogedIn.value =
      !!window.localStorage.getItem(tokenKey) && !!window.localStorage.getItem(userInformationKey)
  }
}

export const authorizationManagerInjectionKey = Symbol(
  'authorization-manager'
) as InjectionKey<AuthorizationManager>
