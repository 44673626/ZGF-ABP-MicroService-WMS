<template>
  <div class="login">
    <!-- card -->
    <el-form
      ref="loginForm"
      :model="loginForm"
      :rules="loginRules"
      label-position="left"
      label-width="0px"
      class="login-form"
    >
      <h3 class="title">智慧WMS仓储工厂软件</h3>
      <el-form-item prop="tenant">
        <el-input
          v-model="loginForm.tenant"
          type="text"
          auto-complete="off"
          placeholder="公司"
        >
          <svg-icon
            slot="prefix"
            icon-class="cloud"
            class="el-input__icon input-icon"
          />
        </el-input>
      </el-form-item>
      <el-form-item prop="username">
        <el-input
          v-model="loginForm.username"
          type="text"
          auto-complete="off"
          placeholder="账号"
        >
          <svg-icon
            slot="prefix"
            icon-class="user"
            class="el-input__icon input-icon"
          />
        </el-input>
      </el-form-item>
      <el-form-item prop="password">
        <el-input
          v-model="loginForm.password"
          type="password"
          auto-complete="off"
          placeholder="密码"
          @keyup.enter.native="handleLogin"
        >
          <svg-icon
            slot="prefix"
            icon-class="password"
            class="el-input__icon input-icon"
          />
        </el-input>
      </el-form-item>
      <el-form-item style="width: 100%">
        <el-button
          :loading="loading"
          size="medium"
          type="primary"
          style="width: 100%"
          @click.native.prevent="handleLogin"
        >
          <span v-if="!loading">登 录</span>
          <span v-else>登 录 中...</span>
        </el-button>
      </el-form-item>
    </el-form>
    <!--  底部  -->
  </div>
</template>

<script>
import VanillaTilt from "vanilla-tilt";
import LangSelect from "@/components/LangSelect";
import config from "../../../static/config";

export default {
  name: "Login",
  components: { LangSelect },
  data() {
    const validatePassword = (rule, value, callback) => {
      if (value.length < 6) {
        callback(new Error("The password can not be less than 6 digits"));
      } else {
        callback();
      }
    };
    return {
      loginForm: {
        tenant: undefined,
        username: "",
        password: "",
        client_id: config.client.client_id,
        grant_type: config.client.grant_type,
        scope: config.client.scope,
      },
      loginRules: {
        username: [
          { required: true, message: "username is required", trigger: "blur" },
        ],
        password: [
          { required: true, trigger: "blur", validator: validatePassword },
        ],
      },
      passwordType: "password",
      capsTooltip: false,
      loading: false,
      showDialog: false,
      redirect: undefined,
      otherQuery: {},
    };
  },
  watch: {
    $route: {
      handler: function (route) {
        const query = route.query;
        if (query) {
          this.redirect = query.redirect;
          this.otherQuery = this.getOtherQuery(query);
        }
      },
      immediate: true,
    },
  },
  created() {
    // window.addEventListener('storage', this.afterQRScan)
    setTimeout(function () {
      VanillaTilt.init(document.querySelectorAll(".card"), {
        max: 25,
        speed: 400,
        glare: true,
        "max-glare": 1,
      });
    }, 2000);
  },
  destroyed() {
    // window.removeEventListener('storage', this.afterQRScan)
  },
  methods: {
    checkCapslock(e) {
      const { key } = e;
      this.capsTooltip = key && key.length === 1 && key >= "A" && key <= "Z";
    },
    showPwd() {
      if (this.passwordType === "password") {
        this.passwordType = "";
      } else {
        this.passwordType = "password";
      }
      this.$nextTick(() => {
        this.$refs.password.focus();
      });
    },
    handleLogin() {
      this.$refs.loginForm.validate((valid) => {
        if (valid) {
          this.loading = true;
          this.$store
            .dispatch("user/userLogin", this.loginForm)
            .then(() => {
              this.$router.push({
                path: this.redirect || "/",
                query: this.otherQuery,
              });
              this.loading = false;
            })
            .catch(() => {
              this.loading = false;
            });
        } else {
          return false;
        }
      });
    },
    getOtherQuery(query) {
      return Object.keys(query).reduce((acc, cur) => {
        if (cur !== "redirect") {
          acc[cur] = query[cur];
        }
        return acc;
      }, {});
    },
  },
};
</script>

<style lang="scss">
$bg: #283443;
$light_gray: #fff;
$cursor: #fff;

.el-form-item {
  margin-bottom: 20px;
}
.container {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  max-width: 1200px;
  flex-wrap: wrap;
  z-index: 1;
}

.container .card {
  position: relative;
  width: 280px;
  height: 400px;
  margin: 30px;
  box-shadow: 20px 20px 50px rgba(0, 0, 0, 0.5);
  border-radius: 15px;
  background: rgba(255, 255, 255, 0.1);
  overflow: hidden;
  display: flex;
  justify-content: center;
  align-items: center;
  border-top: 1px solid rgba(255, 255, 255, 0.5);
  border-left: 1px solid rgba(255, 255, 255, 0.5);
  backdrop-filter: blur(5px);
}

.container .card .content {
  padding: 20px;
  text-align: center;
  transform: translateY(25px);
  transition: 0.5s;
}

.container .card:hover .content {
  transform: translateY(0px);
}

.container .card .content h2 {
  position: absolute;
  top: -80px;
  right: 20px;
  font-size: 8em;
  color: rgba(255, 255, 255, 0.05);
  pointer-events: none;
}

.container .card .content h3 {
  font-size: 1.8em;
  color: #fff;
  z-index: 1;
}

.container .card .content p {
  font-size: 1em;
  color: #fff;
  font-weight: 300;
}

.container .card .content a {
  position: relative;
  display: inline-block;
  padding: 8px 20px;
  margin-top: 15px;
  background: #fff;
  color: #000;
  border-radius: 20px;
  text-decoration: none;
  font-weight: 500;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
}
/* reset element-ui css */
.login {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  background-image: url("../../../static/image/background.jpg");
}
.title {
  margin: 0 auto 30px auto;
  text-align: center;
  color: #707070;
}

.login-form {
  border-radius: 6px;
  background: #ffffff;
  width: 385px;
  padding: 25px 25px 5px 25px;
  .el-input {
    height: 38px;
    input {
      height: 38px;
    }
  }
  .input-icon {
    height: 39px;
    width: 14px;
    margin-left: 2px;
  }
}
.login-tip {
  font-size: 13px;
  text-align: center;
  color: #bfbfbf;
}
.login-code {
  width: 33%;
  display: inline-block;
  height: 38px;
  float: right;
  img {
    cursor: pointer;
    vertical-align: middle;
  }
}
</style>
