<template>
  <div class="size-input">
    <b-form-group label="Width" class="col" label-cols="3">
      <b-form-input v-model="width" type="number" min="0" />
    </b-form-group>
    <b-form-group label="Height" class="col" label-cols="3">
      <b-form-input v-model="height" type="number" minh="0" />
    </b-form-group>
  </div>
</template>

<script setup lang="ts">
import { computed, ref, toRefs, watch } from 'vue'
import { BFormInput } from 'bootstrap-vue-next'

const props = defineProps({
  modelValue: String
})
const modelValue = toRefs(props).modelValue!

const emit = defineEmits(['update:model-value'])

const width = ref(0)
const height = ref(0)

const size = computed(() => {
  return `${width.value} * ${height.value}`
})

const updateModelValue = (value: string) => {
  emit('update:model-value', value)
}

watch(size, (value: string) => {
  updateModelValue(value)
})

watch(
  modelValue!,
  (value: string | undefined) => {
    const split = value?.split('*')
    if (split?.length === 2) {
      width.value = parseInt(split[0].trim())
      height.value = parseInt(split[1].trim())
    } else {
      width.value = 0
      height.value = 0
    }
  },
  { immediate: true }
)
</script>

<style scoped>
.size-input {
  display: inline-flex;
  gap: 1rem;
  border: 1px solid lightgray;
  padding: 1rem;
  text-align: center;
}
</style>
