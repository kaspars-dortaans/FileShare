import type { AxiosError } from 'axios'

interface AppException {
  message?: string
  errors?: { Value: string[] }
  title?: string
}

export class ApiError extends Error {
  constructor(error: AxiosError) {
    let errorMessage = error.message
    if (error.response?.data) {
      const apiException = error.response.data as AppException
      if (apiException.message) {
        errorMessage = apiException.message
      } else if (apiException.errors?.Value?.length) {
        errorMessage = apiException.errors.Value[0]
      } else if (apiException.title) {
        errorMessage = apiException.title
      }
    }
    super(errorMessage)
  }
}
