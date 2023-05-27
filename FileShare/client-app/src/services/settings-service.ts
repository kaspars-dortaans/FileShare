import type { SettingListItem } from '@/common/interfaces/view-models/setting/setting-list-item'
import { get, post, httpDelete } from './api-client'
import type { SettingViewModel } from '@/common/interfaces/view-models/setting/setting-view-model'

export const SettingsService = {
  getSettings: async () => await get<SettingListItem[]>('setting/GetSettings'),
  getEditSettingData: async (id: number) =>
    await get<SettingViewModel>('/Setting/EditSetting', { id }),
  editSetting: async (model: SettingViewModel) => post<void>('/Setting/EditSetting', model),
  clearSetting: async (id: number) => httpDelete<void>('/Setting/ClearSetting', { id })
}
