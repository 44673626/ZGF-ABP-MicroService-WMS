<template>
  <div class="app-container">
    <div class="fn-block" id="reader-iframe"></div>
  </div>
</template>

<script>
import officeSDKExports from "@/directive/hxsign/document-viewer.js";
export default {
  name: 'DocumentViewer',
  data() {
    return {
      docViewerHelper: null, // 软航阅读器对象
    };
  },
  mounted() {
    this.initDocumentViewer();
  },
  methods: {
    initDocumentViewer() {
      // 检查 document-viewer.js 是否已加载
      if (typeof document.newDocumentSdk === 'function') {
        this.docViewerHelper = document.newDocumentSdk();
        console.log("document.newDocumentSdk()");
        this.createReader();
      } else {
        console.error('document.newDocumentSdk 未定义');
      }
    },
    createReader() {
      if (this.docViewerHelper.isReaderReady()) {
        alert('PDF阅读器已加载');
        return;
      }

      // 设定软航阅读器回调
      this.setupReaderCallbacks();

      // 嵌入软航阅读器到页面
      this.docViewerHelper.embedReader('reader-iframe', 'http://localhost:8099/ntko-doc-viewer', {
        viewMode: 'DigitalSign',
        // signMode: 'Extension',
        // consumer: 'D3A2D76C-C0A7-46BB-A0D4-896F70BBCF97', // 授权用户标识
        // scrollbars: true, // 是否显示滚动条
        // ribbonMenu: true, // 是否显示菜单
        // ribbonTitle: true, // 是否显示菜单标题
        // compactViewer: false, // 是否阅读视图
        // keepMenu: true, // 菜单显示模式
      });
    },
    setupReaderCallbacks() {
      // 添加阅读器加载完毕的回调
      this.docViewerHelper.addReaderReadyCallback(() => {
        console.log('阅读器加载完毕');

        // 设置签章服务器
        this.setSignServer();

        // 设定用户信息
        this.docViewerHelper.setSessionUser('测试用户1');
        // this.docViewerHelper.setSessionUser({ userName: '测试用户1', userId: 'demo' });

        // 调用自定义打开文档方法
        this.openPdfDocument();
      });
    },
    setSignServer() {
      // 设置签章服务器的逻辑
      console.log('设置签章服务器');
      // 示例：设置签章服务器地址
      this.docViewerHelper.setSignServer('http://demo.ntko.com:1987/ntkoSignServer');

    },
    openPdfDocument() {
      // 打开文档的逻辑
      console.log('打开文档');
      // 示例：打开服务器上的文档
      const documentUrl = '../../directive/hxsign/test.pdf';
      this.docViewerHelper.openDocument(documentUrl);
    },
  
  },
};
</script>

<style scoped>
.fn-block {
  width: 100%;
  height: 800px; /* 设置阅读器的高度 */
  border: 1px solid #ccc;
}
</style>