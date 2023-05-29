<template>
  <h4 class="my-3">Settings</h4>
  <b-table
    :items="settingList"
    :fields="fields"
    primary-key="id"
    :busy="showSpinner"
    responsive
    sticky-header
  >
    <template #cell(actions)="data">
      <div class="action-buttons">
        <b-button variant="outline-dark" class="icon-btn" @click="editSetting(data.item)"
          ><i-tabler-edit
        /></b-button>
        <b-button variant="outline-dark" class="icon-btn" @click="clearSetting(data.item)"
          ><i-ic-baseline-clear
        /></b-button>
      </div>
    </template>
  </b-table>
  <EditSetting ref="editSettingModal" @completed="fetchData" />
</template>

<script setup lang="ts">
import type { SettingListItem } from '@/common/interfaces/view-models/setting/setting-list-item'
import { SettingsService } from '@/services/settings-service'
import { onMounted, ref } from 'vue'
import { toast } from 'vue3-toastify'
import EditSetting from '@/components/settings/EditSetting.vue'

const editSettingModal = ref<InstanceType<typeof EditSetting> | null>(null)

const settingList = ref<Array<SettingListItem>>([])
const fields = ['name', 'value', 'description', 'actions']
const showSpinner = ref(true)

onMounted(() => {
  fetchData()
})

const editSetting = (item: SettingListItem) => {
  editSettingModal.value?.editSetting(item.id)
}

const clearSetting = async (item: SettingListItem) => {
  showSpinner.value = true
  await SettingsService.clearSetting(item.id)
  fetchData()
  toast.success('Setting successfully cleared')
}

const fetchData = async () => {
  showSpinner.value = true
  settingList.value = await SettingsService.getSettings()
  showSpinner.value = false
}
</script>
