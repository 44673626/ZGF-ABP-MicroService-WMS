<template>
    <div>
        <el-form :model="fromData" ref="from">
            <el-table :data="fromData.domains" style="width: 100%"  :cell-style="tableRowstyle" 
          tooltip-effect="light"
          header-row-class-name="tableRowstyle"
        cell-class-name="Rowstyle"
           stripe border>
          <el-table-column type="index" width="50" label="序号" align="center" />
                <el-table-column show-overflow-tooltip v-for="item in matchingparameters" :key="item.key" :prop="item.key" :label="item.label" :min-width="item.width?item.width:120">
                    <template slot-scope="scope">
                        <span v-if="!item.type">{{ scope.row[item.key] }}</span>
                        <el-form-item v-else-if="item.type=='select'" :prop="'domains.'+scope.$index+'.'+item.key" style="margin:0">
                            <el-select size="mini" filterable  clearable v-model="scope.row[item.key]" placeholder="请选择">
                                <el-option
                                v-for="item in item.options"
                                :key="item.value"
                                :label="item.label"
                                :value="item.value">
                                </el-option>
                            </el-select>
                        </el-form-item>
                    </template>
                </el-table-column>
            </el-table>
        </el-form>
    </div>
</template>
<script>
export default {
    name: 'MatchingTable',
    props: {
        matchingdata: {
            type: Object,
            default() {
                return {
                    list: [],
                    listLoading: false,
                }
            }
        },
        matchingparameters: {
            type: Array,
            default() {
                return []
            }
        }
    },
    data(){
        return{
            fromData:{
                domains:[]
            }
        }
    },
    watch:{
        matchingdata: {
            handler: function (val, oldVal) {
                this.fromData.domains = val.list
            },
            deep: true
        }
    },
    methods: {
        getdata(){
            return this.fromData
        },
        tableRowstyle({
      	row,
      	column,
      	rowIndex,
      	columnIndex
      }) {
      	return 'font-size:12px;padding:0px;height:30px;'
      },
      Rowstyle({
      	row,
      	column,
      	rowIndex,
      	columnIndex
      }) {
      	if (row.VoucherAct === 'D') {
      		return 'font-size:12px;display:none;padding:0;heigth:30px;'
      	} else {
      		return 'font-size:12px;padding:0;heigth:30px;'
      	}
      },
    },
}
</script>