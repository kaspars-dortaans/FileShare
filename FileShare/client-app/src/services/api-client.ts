import axios, { AxiosError, type AxiosRequestConfig } from 'axios'
import { ApiError } from './api-error'

axios.defaults.baseURL = '/api/'

export async function get<T>(url: string, params?: object, config: AxiosRequestConfig = {}) {
  config.params = params
  try {
    return (await axios.get<T>(url, config)).data
  } catch (error) {
    throw new ApiError(error as AxiosError)
  }
}

export async function post<T>(url: string, data: object, config: AxiosRequestConfig = {}) {
  try {
    return (await axios.post<T>(url, data, config)).data
  } catch (error) {
    throw new ApiError(error as AxiosError)
  }
}

export async function httpDelete<T>(
  url: string,
  params: object,
  config: AxiosRequestConfig = {}
): Promise<T> {
  config.params = params
  try {
    return await axios.delete(url, config)
  } catch (error) {
    throw new ApiError(error as AxiosError)
  }
}

export { axios }
