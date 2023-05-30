<template>
  <div class="file-view">
    <div class="header">
      <h4 class="my-3 mr-auto">Files</h4>
      <b-button varaint="primary" class="icon-btn" @click="addFile">
        <i-ic-baseline-plus />Add File
      </b-button>
    </div>
    <b-table :items="fileList" :fields="fields" :busy="isFetchingData" table-class="sticky-header-table" sticky-header
      responsive ref="table">
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
    <EditOrAddFile ref="editOrAddFileModal" @completed="reload" />
  </div>
</template>

<script setup lang="ts">
import type { FileListItem } from '@/common/interfaces/view-models/file/file-list-item'
import { DownloadFile } from '@/common/utils/file-utils'
import EditOrAddFile from '@/components/file/EditOrAddFile.vue'
import { FileService } from '@/services/files-service'
import type { AxiosError } from 'axios'
import type { BTable } from 'bootstrap-vue-next'
import { onMounted, onUnmounted, ref } from 'vue'
import { toast } from 'vue3-toastify'

const editOrAddFileModal = ref<InstanceType<typeof EditOrAddFile> | null>(null)
const table = ref<InstanceType<typeof BTable> | null>(null)

const isFetchingData = ref(false)
const fileList = ref<FileListItem[]>([])
const fields = ['name', 'comment', 'actions']
const pageSize = 25
let currentPage = 0
let allFetched = false

onMounted(async () => {
  const tableElement = table.value?.$el
  tableElement.addEventListener('scroll', onScrool)
  reload()
})

onUnmounted(() => {
  table.value?.$el.removeEventListener('scroll', onScrool)
})

const onScrool = function (e: Event) {
  if (isFetchingData.value) {
    return
  }

  const target = e.target as HTMLElement
  if (target.scrollTop + target.clientHeight >= target.scrollHeight) {
    getFileList()
  }
}

const getFileList = async function () {
  if (allFetched) {
    return
  }

  isFetchingData.value = true
  currentPage++
  try {
    const newFiles = await FileService.getFiles(currentPage, pageSize)
    if (newFiles.length < pageSize) {
      allFetched = true
    }
    fileList.value = fileList.value.concat(newFiles)
  } catch (err) {
    toast.error((err as AxiosError).message)
    allFetched = true
  }
  isFetchingData.value = false
}

const reload = async function () {
  currentPage = 0
  allFetched = false
  fileList.value = []

  const tableElement = table.value?.$el
  while (!allFetched && tableElement.scrollHeight <= tableElement.clientHeight) {
    await getFileList()
  }
}

const downloadFile = async function (item: FileListItem) {
  try {
    const response = await FileService.downloadFile(item.id)
    const contentDisposition = (response.headers['content-disposition'] as string) ?? ''
    const nameIndex = contentDisposition.indexOf('=')
    const name = nameIndex < 0 ? '' : contentDisposition.substring(nameIndex + 1).replace(/[\'\"]/g, "")
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
  isFetchingData.value = true
  try {
    await FileService.deleteFile(item.id)
    toast.success('File was successfully deleted')
    reload()
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

.file-view {
  display: flex;
  flex-direction: column;
  overflow: auto;
  position: relative;
}
</style>
