import type { AxiosError } from 'axios'

interface AppException {
  message: string
}

export class ApiError extends Error {
  constructor(error: AxiosError) {
    let errorMessage = error.message
    if (error.response?.data) {
      const apiException = error.response.data as AppException
      if (apiException.message) {
        errorMessage = apiException.message
      }
    }
    super(errorMessage)
  }
}
