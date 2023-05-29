<template>
  <div class="header">
    <h4 class="my-3 mr-auto">Files</h4>
    <b-button varaint="primary" class="icon-btn" @click="addFile"
      ><i-ic-baseline-plus />Add File</b-button
    >
  </div>

  <b-table :items="fileList" :fields="fields" primary-key="id" :busy="showSpinner">
    <template #cell(actions)="data">
      <div class="action-buttons">
        <b-button variant="outline-dark" class="icon-btn" @click="downloadFile(data.item)">
          <i-material-symbols-download />
        </b-button>
        <b-button variant="outline-dark" class="icon-btn" @click="editFile(data.item)">
          <i-tabler-edit />
        </b-button>
        <b-button variant="outline-dark" class="icon-btn" @click="deleteFile(data.item)">
          <i-ant-design-delete-outlined />
        </b-button>
      </div>
    </template>
  </b-table>
  <EditOrAddFile ref="editOrAddFileModal" @completed="fetchData" />
</template>

<script setup lang="ts">
import type { FileListItem } from '@/common/interfaces/view-models/file/file-list-item'
import { DownloadFile } from '@/common/utils/file-utils'
import EditOrAddFile from '@/components/file/EditOrAddFile.vue'
import { FileService } from '@/services/files-service'
import type { AxiosError } from 'axios'
import { onMounted, ref } from 'vue'
import { toast } from 'vue3-toastify'

const editOrAddFileModal = ref<InstanceType<typeof EditOrAddFile> | null>(null)

const showSpinner = ref(true)
const fields = ['name', 'comment', 'actions']
const fileList = ref<FileListItem[]>([])

onMounted(() => {
  fetchData()
})

const fetchData = async function () {
  showSpinner.value = true
  fileList.value = await FileService.getFiles()
  showSpinner.value = false
}

const downloadFile = async function (item: FileListItem) {
  try {
    const response = await FileService.downloadFile(item.id)
    const contentDisposition = (response.headers['content-disposition'] as string) ?? ''
    const nameIndex = contentDisposition.indexOf('=')
    const name = nameIndex < 0 ? '' : contentDisposition.substring(nameIndex + 1)
    const blob = new Blob([response.data], { type: response.headers['content-type'] })
    DownloadFile(blob, name)
  } catch (err) {
    toast.error((err as AxiosError).message)
  }
}

const addFile = function () {
  editOrAddFileModal.value?.addFile()
}

const editFile = function (item: FileListItem) {
  editOrAddFileModal.value?.editFile(item.id)
}

const deleteFile = async function (item: FileListItem) {
  showSpinner.value = true
  try {
    await FileService.deleteFile(item.id)
    toast.success('File was successfully deleted')
    await fetchData()
  } catch (err) {
    toast.error((err as AxiosError).message)
  }
}
</script>

<style scoped>
.header {
  padding: 0 1rem;
  display: flex;
  justify-content: space-between;
  align-items: baseline;
}
</style>
