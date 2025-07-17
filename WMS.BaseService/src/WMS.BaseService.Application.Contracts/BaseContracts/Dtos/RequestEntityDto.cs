using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using WMS.BaseService.BaseContracts.Filters;

namespace WMS.BaseService.BaseContracts.Dtos
{
    public class RequestEntityDto : EntityDto
    {
        /// <summary>
        /// 包含详情
        /// </summary>
        public virtual bool IncludeDetails => false;
        /// <summary>
        /// 获取筛选条件(继承类实现Filter标签的集合)
        /// </summary>
        /// <returns></returns>
        public List<FilterCondition> ToFilters()
        {
            List<FilterCondition> filterConditions = new();
            var properties = GetType().GetProperties();
            foreach (var item in properties)
            {
                if (item.GetValue(this) == null || string.IsNullOrWhiteSpace(Convert.ToString(item.GetValue(this))))
                    continue;
                var filter = item.GetCustomAttribute<FilterAttribute>();
                if (filter == null) continue;
                filterConditions.Add(new FilterCondition(FirstCharToUpper(filter.Column) ?? item.Name, Convert.ToString(item.GetValue(this))));
            }
            return filterConditions;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string FirstCharToUpper(string str)
        {
            if (str == null || str.Length == 0) return str;
            if (str.Length == 1) return str.ToUpper();
            return string.Concat(str[..1].ToUpper(), str.AsSpan(1));
        }
    }
}
