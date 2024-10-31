// 验证是否整数
export function isInteger(rule, value, callback) {
  if (!value && value !== 0) {
    return callback(new Error('输入不可以为空'));
  }
  setTimeout(() => {
    if (!Number(value) && Number(value) !== 0) {
      callback(new Error('请输入正整数'));
    } else {
      const re = /^[+]{0,1}(\d+)$/;
      const rsCheck = re.test(value);
      if (!rsCheck || Number(value) < 0) {
        callback(new Error('请输入正整数'));
      } else {
        callback();
      }
    }
  }, 0);
}

// 验证 整数 6位小数 大于等于0的小数(6位小数)
export function isPositivedecimal(rule, value, callback) {
  console.log('%c [ value ]-23', 'font-size:13px; background:pink; color:#bf2c9f;', value)
  if (!value && value !== 0) {
    return callback(new Error('输入不可以为空'));
  }
  setTimeout(() => {
    if (!Number(value) && Number(value) !== 0) {
      callback(new Error('请输入正数可保留6位小数'));
    } else {
      const re = /^(([1-9]{1}\d*)|(0{1}))(\.\d{1,6})?$/;
      const rsCheck = re.test(value);
      if (!rsCheck) {
        callback(new Error('请输入正数可保留6位小数'));
      } else {
        callback();
      }
    }
  }, 0);
}



// 验证 整数 6位小数  大于不等于0的小数(6位小数)
export function isPositive(rule, value, callback) {
  if (!value || value == 0) {
    return callback(new Error('输入不可以为空'));
  }
  setTimeout(() => {
    if (!Number(value) && Number(value) !== 0) {
      callback(new Error('请输入正数可保留6位小数'));
    } else {
      const re = /^(([1-9]{1}\d*)|(0{1}))(\.\d{1,6})?$/;
      const rsCheck = re.test(value);
      if (!rsCheck) {
        callback(new Error('请输入正数可保留6位小数'));
      } else {
        callback();
      }
    }
  }, 0);
}


// 验证是否为空
export function isrequired(rule, value, callback) {
  if (!value && value !== 0) {
    return callback(new Error('输入不可以为空'));
  }
}

// 验证为非负数
export function NonnegativeNumber(rule, value, callback) {
  if(value){
    const re = /((^[1-9]\d*)|0|^0)(\.\d{0,2}){0,1}$/;
    const rsCheck = re.test(value);
      if (!Number(value) && Number(value) !== 0 && !rsCheck) {      
      
      callback(new Error('请输入非负数'));
    } else {
      callback();

    }
  }else{
    callback();
  }
}


// 验证手机号码
export function  validateMobilePhone(rule, value, callback) {
  if(value){
    var reg=/^1[3456789]\d{9}$/;
    if(!reg.test(value)){
      callback(new Error('请输入有效的手机号码'));
    }else{
      callback();
    }
  }else{
    callback();
  }
  
}

  export function  validateEmail(rule, value, callback) {
  if (value === '') {
    callback(new Error('请正确填写邮箱'));
  } else {
    if (value !== '') { 
      var reg=/^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/;
      if(!reg.test(value)){
        callback(new Error('请输入有效的邮箱'));
      }
    }
    callback();
  }
};

