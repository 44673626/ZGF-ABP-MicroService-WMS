<template>
    <div>
        <el-table 
			ref="multipleTable" 
			:stripe="tableobject.stripe" 
			v-loading="tableobject.listLoading"
			:height="tableobject.tabheight" 
			:data="tableobject.list" 
			size="small" 
			style="width: 100%;"
			tooltip-effect="light" 
			header-row-class-name="tableRowstyle"
			cell-class-name="Rowstyle"
			@row-click="handleRowClick"
			border>
            <el-table-column type="index" width="50" label="序号" align="center" fixed="left" />
            <el-table-column v-for="item in basicstableparams" :key="item.key" :sortable="item.sortable ? item.sortable : true"
                :label="item.label" :prop="item.prop" :show-overflow-tooltip="item.istooltip ? item.istooltip : true"
                :width="item.width ? item.width : 135" :align="item.align?item.align:'left'">
                <template slot-scope="scope">
                    <slot name="switch" :row="scope.row" v-if="item.type == 'switch'" />
                    <span v-else-if="item.type == 'status'">
                        <el-tag :type="scope.row[item.prop] ? 'success' : 'danger'" size="mini">
                            {{ scope.row[item.prop] ? '启用' : '停用' }}
                        </el-tag>
                    </span>
                    <el-link @click="viewdetails(scope.row.state,scope.row)" v-else-if="item.type == 'link'"
                        type="primary">{{ scope.row[item.prop] }}</el-link>
                    <el-link @click="vieworderdetails(scope.row.saleOrderId)" v-else-if="item.type == 'linkorder'"
                        type="primary">{{ scope.row[item.prop] }}</el-link>
					<span v-else-if="item.type == 'colorStatus'">
						<span v-if="scope.row[item.prop] == '已分图' ||scope.row[item.prop] == '已工艺' " style="color:green;">{{'· '+scope.row[item.prop]}}</span>
						<!-- <span v-else-if="scope.row[item.prop] == '已工艺'" style="color:green;">{{'· '+scope.row[item.prop]}}</span> -->
						<span v-else style="color:red;">{{'· '+scope.row[item.prop]}}</span>
					</span>
                    <span v-else>{{ scope.row[item.prop] }}</span>
                </template>
            </el-table-column>
            <el-table-column v-if="tableobject.operatingarea?tableobject.operatingarea:false" label="操作" align="center" width="100" fixed="right">
                <template slot-scope="scope">
                    <slot name="action" :row="scope.row" />
                </template>
            </el-table-column>
        </el-table>
    </div>
</template>
<script>
export default {
    name: 'BasicsTableList',
    props: {
        basicstableobject: {
            type: Object,
            default() {
                return {
                    stripe: true,
                    list: [],
                    listLoading: false,
                    tabheight: 'auto',
                    operatingarea:true
                }
            }
        },
        basicstableparams: {
            type: Array,
            default() {
                return []
            }
        }
    },
    data() {
        return {
            tableobject: {}
        }
    },
    watch: {
        basicstableobject: {
            handler: function (val, oldVal) {
                this.tableobject = val
            },
            deep: true
        }
    },
    methods: {
        viewdetails(state,row) {
            this.$emit('handleDetailButton', state,row)
        },
        vieworderdetails(id) {
            this.$emit('handleorderDetailButton', id, false)
        },
        handleRowClick(row) {
            this.$emit('handleRowClick', row)
        }
    },

}
</script>