import { get, post, httpDelete, axios } from './api-client'
import type { FileListItem } from '@/common/interfaces/view-models/file/file-list-item'
import type { FileViewModel } from '@/common/interfaces/view-models/file/file-view-model'

export const FileService = {
  getFiles: async () => await get<FileListItem[]>('File/GetFiles'),
  getFileFormData: async (id?: number) => await get<FileViewModel>('/File/GetFileFormData', { id }),
  downloadFile: async (id: number) =>
    await axios.get<string>('/File/DownloadFIle', { params: { id }, responseType: 'arraybuffer' }),
  addFile: async (data: FormData) =>
    await post<FileViewModel>('/File/AddFile', data, {
      headers: { 'Content-Type': 'multipart/form-data' }
    }),
  editFile: async (data: FileListItem) => post<void>('/File/EditFile', data),
  deleteFile: async (id: number) => httpDelete<void>('/File/DeleteFile', { id })
}
