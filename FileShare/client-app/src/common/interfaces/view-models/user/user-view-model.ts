import type { Role } from '@/common/enums/role'

export interface UserViewModel {
  id: number
  firstName: string
  lastName: string
  username: string
  role: Role
}
