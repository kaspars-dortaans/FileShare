<template>
  <b-modal v-model="modalVisible" title="Edit Setting" ok-title="Save Setting" @ok="onSubmit">
    <b-overlay :show="!loadedState">
      <Form as="b-form" :validation-schema="validationSchema" @submit="onSubmit($event as Event)">
        <b-form-group label="Name">
          <b-form-input v-model="model.name" disabled />
        </b-form-group>
        <b-form-group label="Description">
          <b-form-input v-model="model.description" />
        </b-form-group>
        <b-form-group label="Value">
          <Field name="Value" v-model="model.value" v-slot="{ field }">
            <valueValidation.component v-bind="field" :placeholder="valueValidation.placeholder" />
          </Field>
          <ErrorMessage name="Value" as="b-form-invalid-feedback" v-slot="{ message }" force-show>
            {{ message }}
          </ErrorMessage>
        </b-form-group>
      </Form>
    </b-overlay>
  </b-modal>
</template>

<script setup lang="ts">
import type { SettingViewModel } from '@/common/interfaces/view-models/setting/setting-view-model'
import { ref, reactive, computed, type Component } from 'vue'
import { Form, Field, ErrorMessage } from 'vee-validate'
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

const valueValidation = computed(() => {
  let component: Component = BFormInput,
    rules: string | object = '',
    placeholder = 'Setting Value'
  const set = (setComponent: Component, setRules: string | object, setPlaceholder: string) => {
    component = setComponent
    rules = setRules
    placeholder = setPlaceholder
  }

  switch (model.dataType) {
    case SettingDataType.Size:
      set(SizeInput, { regex: /\d+\s\*\s\d+/ }, '')
      break
    case SettingDataType.StringList:
      set(BFormInput, { regex: /^[\w\.]+(,\s*[\w\.]+)*$/ }, "Format: 'item1, itme2, itme3...'")
      break
  }
  return { component, rules, placeholder }
})

const validationSchema = computed(() => {
  return {
    Value: valueValidation.value.rules
  }
})

const editSetting = async (id: number) => {
  modalVisible.value = true
  loadedState.value = false

  try {
    const formData = await SettingsService.getEditSettingData(id)
    Object.assign(model, formData)
  } catch (err) {
    toast.error((err as AxiosError).message)
    modalVisible.value = false
  }
  loadedState.value = true
}

const onSubmit = async (e: Event) => {
  e.preventDefault()
  loadedState.value = false
  try {
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
