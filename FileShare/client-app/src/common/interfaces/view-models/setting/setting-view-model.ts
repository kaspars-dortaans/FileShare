import type { SettingDataType } from '@/common/enums/data-types'

export interface SettingViewModel {
  id: number
  value: string
  name: string
  description: string
  dataType: SettingDataType
}
