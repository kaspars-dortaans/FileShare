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
          <Field
            v-if="!isEdit"
            name="File"
            v-model="model.file"
            v-slot="{ handleChange, handleBlur }"
          >
            <input
              ref="fileInput"
              type="file"
              class="form-control"
              @change="handleChange"
              @blur="handleBlur"
            />
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

const emit = defineEmits(['completed'])

const form = ref<InstanceType<typeof Form> | null>(null)
const fileInput = ref<InstanceType<typeof HTMLInputElement> | null>(null)

const modalVisible = ref(false)
const loadedState = ref(false)
const model: FileViewModel = reactive({
  id: null,
  name: '',
  comment: '',
  file: null,
  minSize: null,
  maxFileSize: null,
  extension: null
})

const isEdit = computed(() => {
  return !!model.id
})

const title = computed(() => {
  return isEdit.value ? 'Edit File' : 'Add File'
})

const fullFileName = computed(() => {
  return `${model.name}${model.extension}`
})

const validationSchema = computed(() => {
  return {
    File: isEdit.value ? '' : fileValidation
  }
})

const fileValidation = async function (file: File) {
  if (!file) {
    return 'The File field is required'
  }

  if (model.maxFileSize && file.size / 1024 ** 2 > model.maxFileSize) {
    return `Selected file size is too big, max file size is ${model.maxFileSize} MB`
  }

  if (!model.minSize || !model.file?.type.includes('image')) {
    return true
  }

  const image = new Image()
  image.src = await GetBlobUrlData(file)
  await image.decode()

  if (image.width < model.minSize!.width || image.height < model.minSize!.height) {
    return `Image dimensions are too small, minimal dimensions: ${model.minSize!.width} * ${
      model.minSize!.height
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

  const formData = await FileService.getFileFormData(fileId)
  Object.assign(model, formData)
  fileInput.value!.value = ''
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
      await FileService.editFile(model as FileListItem)
      toast.success('Toast was successfully saved')
    } else {
      const formData = new FormData()
      formData.append(
        'name',
        model.name ?? model.file?.name.substring(0, model.file.name.lastIndexOf('.'))
      )
      formData.append('comment', model.comment ?? '')
      formData.append('file', model.file as Blob)
      await FileService.addFile(formData)
      toast.success('Toast was successfully added')
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
