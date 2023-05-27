<template>
  <div class="register-page">
    <Form
      class="register-form"
      as="b-form"
      :validation-schema="validationSchema"
      @submit="onSubmit"
    >
      <h4>Register</h4>
      <b-form-group label="First Name">
        <Field
          name="First Name"
          as="b-form-input"
          v-model="formData.firstName"
          placeholder="Jhon"
        />
        <ErrorMessage name="First Name" v-slot="{ message }">
          <b-form-invalid-feedback force-show>{{ message }}</b-form-invalid-feedback>
        </ErrorMessage>
      </b-form-group>
      <b-form-group label="Last Name">
        <Field name="Last Name" as="b-form-input" v-model="formData.lastName" placeholder="Doe" />
        <ErrorMessage name="Last Name" v-slot="{ message }">
          <b-form-invalid-feedback force-show>{{ message }}</b-form-invalid-feedback>
        </ErrorMessage>
      </b-form-group>
      <b-form-group label="Username">
        <Field
          name="Username"
          as="b-form-input"
          v-model="formData.username"
          placeholder="JhonDoe"
        />
        <ErrorMessage name="Username" as="b-form-invalid-feedback" v-slot="{ message }" force-show>
          {{ message }}
        </ErrorMessage>
      </b-form-group>
      <b-form-group label="Password">
        <Field
          name="Password"
          as="b-form-input"
          type="password"
          v-model="formData.password"
          placeholder="Password"
        />
        <ErrorMessage name="Password" as="b-form-invalid-feedback" v-slot="{ message }" force-show>
          {{ message }}
        </ErrorMessage>
      </b-form-group>
      <b-form-group label="Confirm Password">
        <Field
          name="Confirm Password"
          as="b-form-input"
          type="password"
          v-model="formData.confirmPassword"
          placeholder="Confirm Password"
        />
        <ErrorMessage
          name="Confirm Password"
          as="b-form-invalid-feedback"
          v-slot="{ message }"
          force-show
        >
          {{ message }}
        </ErrorMessage>
      </b-form-group>
      <b-button type="submit" variant="primary"
        >Register <b-spinner v-if="showSpinner" :small="true"></b-spinner
      ></b-button>
    </Form>
  </div>
</template>

<script setup lang="ts">
import type { RegisterUserViewModel } from '@/common/interfaces/view-models/user/register-user-view-model'
import { UserService } from '@/services/user-service'
import { Form, Field, ErrorMessage } from 'vee-validate'
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { toast } from 'vue3-toastify'

const router = useRouter()

const formData: RegisterUserViewModel = reactive({
  firstName: '',
  lastName: '',
  username: '',
  password: '',
  confirmPassword: ''
})
const showSpinner = ref(false)

const validationSchema = {
  'First Name': 'required|min:3',
  'Last Name': 'required|min:3',
  Username: 'required|min:3',
  Password: {
    required: true,
    regex: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/
  },
  'Confirm Password': 'required|confirmed:@Password'
}

const onSubmit = async () => {
  try {
    showSpinner.value = true

    await UserService.register(formData)

    toast.success('You have successfully registered an acount, please login')
    showSpinner.value = false
    router.push({ name: 'login' })
  } catch (error) {
    toast.error((error as Error).message)
  }
}
</script>

<style>
.register-page {
  display: flex;
  height: 100%;
}

.register-form {
  min-width: 18rem;
  margin: auto;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
</style>
