import { localize } from '@vee-validate/i18n'
import { min, regex, required, confirmed } from '@vee-validate/rules'
import { defineRule, configure } from 'vee-validate'
import en from '@vee-validate/i18n/dist/locale/en.json'

export const defineValidationRules = () => {
  defineRule('required', required)
  defineRule('min', min)
  defineRule('regex', regex)
  defineRule('confirmed', confirmed)

  configure({
    generateMessage: localize({ en })
  })
}
