import axios from 'axios';
// 菜单模块管理
export const businessclass = params => {
    return axios.get(`/api/business/hb/class`, {params: params});
};
export const businessclassinfo = params => {
    return axios.post(`/api/business/hb/class/handle-classinfo`, params);
};
export const businessdelete = params => {
    return axios.delete(`/api/business/hb/class`, {params: params});
};
export const businessclassid = params => {
    return axios.get(`/api/business/hb/class/`+params);
};



export const SCMBomAssChangeApplyExaminedA = params => {
    return axios.put(`/api/permission/put`, params);
};
