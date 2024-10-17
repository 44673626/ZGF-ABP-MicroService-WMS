import Vue from 'vue'

import Cookies from 'js-cookie'
import UmyUi from 'umy-ui'
import 'umy-ui/lib/theme-chalk/index.css';// 引入样式
import printJS from "print-js";
import 'normalize.css/normalize.css' // a modern alternative to CSS resets

import Element from 'element-ui'
import './styles/element-variables.scss'

import '@/styles/index.scss' // global css

import App from './App'
import store from './store'
import router from './router'

import i18n from './lang' // internationalization
import './icons' // icon
import './permission' // permission control
import './utils/error-log' // error log
import axios from './axios'
import moment from 'moment'

import * as filters from './filters' // global filters

/**
 * If you don't want to use mock-server
 * you want to use MockJs for mock api
 * you can execute: mockXHR()
 *
 * Currently MockJs will be used in the production environment,
 * please remove it before going online ! ! !
 */
if (process.env.NODE_ENV === 'production') {
  const { mockXHR } = require('../mock')
  mockXHR()
}

Vue.use(Element, {
  size: Cookies.get('size') || 'medium', // set element-ui default size
  i18n: (key, value) => i18n.t(key, value)
})
Vue.prototype.tableRowClass = function (row) {
  if (row.rowIndex % 2 == 0) {
    return 'rowstyle'
  } else {
    return 'rowstyle1'
  }


}
import '@/assets/public.css'
Vue.use(UmyUi);
Vue.prototype.$axios = axios
Vue.config.productionTip = false

new Vue({
  el: '#app',
  router,
  store,
  i18n,
  render: h => h(App)
})
