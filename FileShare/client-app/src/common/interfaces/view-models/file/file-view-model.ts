import type { Size } from '../../common/size'

export interface FileViewModel {
  id: number | null
  name: string
  comment: string
  file: File | null
  minSize: Size | null
  maxFileSize: number | null
  extension: string | null
}
