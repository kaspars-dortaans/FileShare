<template>
  <b-modal v-model="modalVisible" title="Edit Setting" ok-title="Save Setting" @ok="onSubmit">
    <b-overlay :show="!loadedState">
      <b-form>
        <b-form-group label="Name">
          <b-form-input v-model="model.name" disabled />
        </b-form-group>
        <b-form-group label="Description">
          <b-form-input v-model="model.description" />
        </b-form-group>
        <b-form-group label="Value">
          <settingValueFieldInfo.component
            v-model="settingValue"
            :placeholder="settingValueFieldInfo.placeholder"
          />
          <b-form-invalid-feedback force-show>
            {{ settingValueError }}
          </b-form-invalid-feedback>
        </b-form-group>
      </b-form>
    </b-overlay>
  </b-modal>
</template>

<script setup lang="ts">
import type { SettingViewModel } from '@/common/interfaces/view-models/setting/setting-view-model'
import { ref, reactive, computed, type Component } from 'vue'
import { useField } from 'vee-validate'
import { SettingDataType } from '@/common/enums/data-types'
import { BFormInput } from 'bootstrap-vue-next'
import SizeInput from '@/components/form/SizeInput.vue'
import { SettingsService } from '@/services/settings-service'
import { toast } from 'vue3-toastify'
import type { AxiosError } from 'axios'

const emit = defineEmits(['completed'])

const modalVisible = ref(false)
const model: SettingViewModel = reactive({
  id: -1,
  dataType: SettingDataType.String,
  name: '',
  description: '',
  value: ''
})
const loadedState = ref(false)

const validateSettignValue = function (value: string) {
  return settingValueFieldInfo.value.validate(value)
}

const {
  value: settingValue,
  errorMessage: settingValueError,
  validate
} = useField<string>('Value', validateSettignValue)

const validateWithRegex = function (value: string | undefined, regex: RegExp) {
  if (!value) {
    return 'Field Value is required'
  }
  const match = !!value.match(regex)
  return match ? true : 'The Value field format is invalid'
}

const settingValueFieldInfo = computed(() => {
  let component: Component = BFormInput,
    validate: (value: string) => boolean | string = (value: string) =>
      !value ? 'Field Value is required' : true,
    placeholder = 'Setting Value'

  const set = (
    setComponent: Component,
    setValidate: (value: string) => boolean | string,
    setPlaceholder: string
  ) => {
    component = setComponent
    validate = setValidate
    placeholder = setPlaceholder
  }

  switch (model.dataType) {
    case SettingDataType.Size:
      set(SizeInput, (value: string) => validateWithRegex(value, /\d+\s\*\s\d+/), '')
      break
    case SettingDataType.StringList:
      set(
        BFormInput,
        (value: string) => validateWithRegex(value, /^[\w\.]+(,\s*[\w\.]+)*$/),
        "Format: 'item1, itme2, itme3...'"
      )
      break
    case SettingDataType.PositiveInteger:
      set(BFormInput, (value: string) => validateWithRegex(value, /\d+/), 'e.g. 200')
      break
  }
  return { component, validate, placeholder }
})

const editSetting = async (id: number) => {
  modalVisible.value = true
  loadedState.value = false

  try {
    const formData = await SettingsService.getEditSettingData(id)
    Object.assign(model, formData)
    settingValue.value = model.value
  } catch (err) {
    toast.error((err as AxiosError).message)
    modalVisible.value = false
  }
  loadedState.value = true
}

const onSubmit = async (e: Event) => {
  e.preventDefault()
  const validationResult = await validate()
  if (!validationResult?.valid) {
    return
  }

  loadedState.value = false
  try {
    model.value = settingValue.value
    await SettingsService.editSetting(model)
    toast.success('Setting was successfully saved')
    modalVisible.value = false
    emit('completed')
  } catch (err) {
    toast.error((err as AxiosError).message)
  }
  loadedState.value = true
}

defineExpose({ editSetting })
</script>
