import axios, { AxiosError } from 'axios'
import { ApiError } from './api-error'

axios.defaults.baseURL = '/api/'

export async function get(url: string) {
  try {
    return (await axios.get(url)).data
  } catch (error) {
    throw new ApiError(error as AxiosError)
  }
}

export async function post<T>(url: string, data: object): Promise<T> {
  try {
    return (await axios.post(url, data)).data
  } catch (error) {
    throw new ApiError(error as AxiosError)
  }
}
