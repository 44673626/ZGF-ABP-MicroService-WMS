import axios from 'axios'
import { MessageBox, Message } from 'element-ui'
import store from '@/store'
import { getToken } from '@/utils/auth'

// create an axios instance

const service = axios.create({
  // baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  timeout: 120000 // request timeout 
})

// request interceptor
service.interceptors.request.use(
  config => {
    if (store.getters.token) {
      // let each request carry token
      // ['X-Token'] is a custom headers key
      // please modify it according to the actual situation
      config.headers['X-Token'] = getToken()
      config.headers['Content-Type'] = 'application/json'
      config.headers['wrap-result'] = true
      config.baseURL = process.env.VUE_APP_BASE_API
    }
    return config
  },
  error => {
    // do something with request error
    return Promise.reject(error)
  }
)

// response interceptor
service.interceptors.response.use(
  /**
   * If you want to get http information such as headers or status
   * Please return  response => response
  */

  /**
   * Determine the request status by custom code
   * Here is just an example
   * You can also judge the status by HTTP Status Code
   */
  response => {
    const res = response.data
    // if the custom code is not 20000, it is judged as an error.
    // if (res.code == 403) {

    //   Message({
    //     message: res.code || 'Error',
    //     type: 'error',
    //     duration: 5 * 1000
    //   })
    //   return Promise.reject(new Error(res.code || 'Error'))
    // } else {
    //   return res
    // }
    if (res.status == 200) {
      if (res.success) {
        return res.response
      } else {
        Message({
          message: res.msg,
          type: 'error',
          duration: 5 * 1000
        })
        return Promise.reject(new Error(res.msg))
      }
    }
  },
  error => {
    if (error.response) {
      Message({
        message: error.response.data.error.code || error.response.data.error.message,
        type: 'error',
        duration: 5 * 1000
      })
      return Promise.reject(error)
    }

  }
)

export default service
