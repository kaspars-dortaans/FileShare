import type { AuthenticateRequest } from '@/common/interfaces/view-models/user/authenticate-request'
import type { AuthenticateResponse } from '@/common/interfaces/view-models/user/authenticate-response'
import type { UserViewModel } from '@/common/interfaces/view-models/user/user-view-model'
import { ApiError } from '@/services/api-error'
import type { AxiosError, AxiosStatic } from 'axios'
import { ref, type InjectionKey, type Ref } from 'vue'
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
  userInfo: Ref<UserViewModel | null> = ref(null)

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

    this._updateState()
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
      this._updateState()
    }
  }

  logout() {
    window.localStorage.removeItem(tokenKey)
    window.localStorage.removeItem(userInformationKey)
    if (logoutRedirectRoute) {
      this.router.push({ name: logoutRedirectRoute })
    }
    this._updateState()
  }

  getUserInfo(): UserViewModel | null {
    const infoJson = window.localStorage.getItem(userInformationKey)
    if (!infoJson) {
      return null
    }

    return JSON.parse(infoJson)
  }

  _updateState() {
    const userInfo = window.localStorage.getItem(userInformationKey)
    this.userInfo.value = userInfo ? JSON.parse(userInfo) : null

    this.isLogedIn.value = !!window.localStorage.getItem(tokenKey) && !!userInfo
  }
}

export const authorizationManagerInjectionKey = Symbol(
  'authorization-manager'
) as InjectionKey<AuthorizationManager>
