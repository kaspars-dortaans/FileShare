<template>
  <b-modal v-model="modalVisible" :title="title" :ok-title="title" @ok="onSubmit">
    <b-overlay :show="!loadedState">
      <Form as="b-form" :validation-schema="validationSchema" ref="form">
        <b-form-group label="Name">
          <b-form-input v-model="model.name" placeholder="my file" />
        </b-form-group>
        <b-form-group label="Comment">
          <b-form-input v-model="model.comment" placeholder="This is my file" />
        </b-form-group>
        <b-form-group label="File">
          <Field v-if="!isEdit" name="File" v-model="model.file" v-slot="{ handleChange, handleBlur }">
            <input ref="fileInput" type="file" class="form-control" :accept="formData.allowedExtensions ?? undefined"
              @change="handleChange" @blur="handleBlur" />
          </Field>
          <b-form-input v-else v-model="fullFileName" disabled />
          <ErrorMessage name="File" as="b-form-invalid-feedback" v-slot="{ message }" force-show>
            {{ message }}
          </ErrorMessage>
        </b-form-group>
      </Form>
    </b-overlay>
  </b-modal>
</template>

<script setup lang="ts">
import type { FileViewModel } from '@/common/interfaces/view-models/file/file-view-model'
import { computed, reactive, ref } from 'vue'
import { Form, Field, ErrorMessage } from 'vee-validate'
import { toast } from 'vue3-toastify'
import type { AxiosError } from 'axios'
import { FileService } from '@/services/files-service'
import type { FileListItem } from '@/common/interfaces/view-models/file/file-list-item'
import { GetBlobUrlData } from '@/common/utils/file-utils'
import type { FileFormData } from '@/common/interfaces/view-models/file/file-form-data'

const emit = defineEmits(['completed'])

const form = ref<InstanceType<typeof Form> | null>(null)
const fileInput = ref<InstanceType<typeof HTMLInputElement> | null>(null)

const modalVisible = ref(false)
const loadedState = ref(true)
const formData: FileFormData = reactive({
  id: null,
  name: '',
  comment: null,
  minSize: null,
  maxFileSize: null,
  extension: null,
  allowedExtensions: null
})

const model: FileViewModel = reactive({
  name: '',
  comment: '',
  file: null
})

const isEdit = computed(() => {
  return !!formData.id
})

const title = computed(() => {
  return isEdit.value ? 'Edit File' : 'Add File'
})

const fullFileName = computed(() => {
  return `${model.name}${formData.extension}`
})

const validationSchema = computed(() => {
  return {
    File: isEdit.value ? '' : fileValidation
  }
})

const fileValidation = async function (file: File) {
  //reqiured
  if (!file) {
    return 'The File field is required'
  }

  //file extensions
  const dotIndex = file.name.lastIndexOf(".")
  const fileExtension = dotIndex < 0 ? "" : file.name.substring(dotIndex)
  const allowedxtensions = formData.allowedExtensions?.split(", ");
  if (allowedxtensions && !allowedxtensions.some((extension) => extension === fileExtension)) {
    return "File with given extension is not allowed";
  }

  //max file size
  if (Number.isInteger(formData.maxFileSize) && file.size > formData.maxFileSize! * 1024 * 1024) {
    return `Selected file size is too big, max file size is ${formData.maxFileSize} MB`
  }

  if (!formData.minSize || !model.file?.type.includes('image')) {
    return true
  }

  //min image size
  const image = new Image()
  image.src = await GetBlobUrlData(file)
  await image.decode()

  if (image.width < formData.minSize!.width || image.height < formData.minSize!.height) {
    return `Image dimensions are too small, minimal dimensions: ${formData.minSize!.width} * ${formData.minSize!.height
      }`
  }
  return true
}

const addFile = function () {
  fetchData()
}

const editFile = function (fileId: number) {
  fetchData(fileId)
}

const fetchData = async function (fileId?: number) {
  loadedState.value = false
  modalVisible.value = true

  const response = await FileService.getFileFormData(fileId)
  Object.assign(formData, response)
  model.name = response.name ?? ""
  model.comment = response.comment ?? ""

  if (fileInput?.value) {
    fileInput.value.value = ''
  }

  form.value?.resetForm()

  loadedState.value = true
}

const onSubmit = async function (e: Event) {
  e.preventDefault()

  const validationResult = await form.value?.validate()
  if (!validationResult?.valid) {
    return
  }

  loadedState.value = false

  try {
    if (isEdit.value) {
      const rq: FileListItem = {
        id: formData.id!,
        ...model
      }
      await FileService.editFile(rq)

      toast.success('File was successfully saved')
    } else {
      const formData = new FormData()
      formData.append('name', model.name || model.file!.name.substring(0, model.file!.name.lastIndexOf('.')))
      formData.append('comment', model.comment)
      formData.append('file', model.file as Blob)
      await FileService.addFile(formData)
      toast.success('File was successfully added')
    }
    emit('completed')
    modalVisible.value = false
  } catch (err) {
    toast.error((err as AxiosError).message)
  }
  loadedState.value = true
}

defineExpose({ addFile, editFile })
</script>
