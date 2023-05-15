import type { UserViewModel } from './user-view-model'

export interface AuthenticateResponse {
  userData: UserViewModel
  token: string
}
