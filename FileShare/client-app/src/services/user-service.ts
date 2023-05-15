import type { RegisterUserViewModel } from '@/common/interfaces/view-models/user/register-user-view-model'
import { get, post } from './api-client'

export const UserService = {
  getAllUsers: async () => await get('users/GetAll'),
  register: async (model: RegisterUserViewModel) => await post('users/Register', model)
}
