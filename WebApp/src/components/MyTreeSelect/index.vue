<template>
    <div>
        <treeselect :disabled="disabled" v-model="value" :disable-branch-nodes="true" :options="orgs" placeholder="选择上级机构"
            noOptionsText="暂无数据" noChildrenText="暂无数据" noResultsText="暂无数据" @input="changeP()" />
    </div>
</template>
<script>
import Treeselect from "@riophae/vue-treeselect";
import "@riophae/vue-treeselect/dist/vue-treeselect.css";
import { LOAD_CHILDREN_OPTIONS } from "@riophae/vue-treeselect"
import { getorgs } from "@/api/base"
export default {
    name: 'MyTreeSelect',
    components: {
        Treeselect
    },
    props: {
        disabled: {  // 是否禁用
            type: Boolean,
            default: false
        },
        value: {
            type: String,
            default: ''
        }
    },
    data() {
        return {
            orgs: []
        }
    },
    created() {
        this.gettreeselectdata()
        this.getchilds()
    },
    methods: {
        gettreeselectdata() {
            let _this = this
            getorgs().then(response => {
                this.orgs = response.items.map(function (obj) {
                    console.log('%c [ obj ]-40', 'font-size:13px; background:pink; color:#bf2c9f;', obj.id)
                    obj.label = obj.name
                    if (obj.leaf) {
                        delete obj.children
                    } else {
                        obj.children = _this.getchilds(obj.id)
                    }
                    return obj;
                });
            })
        },
        getchilds(id) {
            let _this = this
            const childs = []
            getorgs({ id: id }).then(response => {
                response.items.map(function (obj) {
                    obj.label = obj.name
                    if (obj.leaf) {
                        delete obj.children
                    } else {
                        obj.children = _this.getchilds(obj.id)
                    }
                    childs.push(obj)
                });
            })
            return childs
        },
        changeP() {

        },
        loadOrgs({ action, parentNode, callback }) {
            if (action === LOAD_CHILDREN_OPTIONS) {
                getorgs({ id: parentNode.id }).then(response => {
                    parentNode.children = response.items.map(function (obj) {
                        obj.label = obj.name
                        if (obj.leaf) {
                            delete obj.children
                        } else {
                            obj.children = null
                        }
                        return obj;
                    });
                    setTimeout(() => {
                        callback();
                    }, 100);
                })
            }
        },
    },
}
</script>