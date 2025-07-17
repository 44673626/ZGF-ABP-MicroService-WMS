using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public class RequestPageEntityDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 排序方式
        /// </summary>
        [BindNever]
        [JsonIgnore]
        public virtual string Sorting { get => "CreationTime Desc"; }
        public override int MaxResultCount { get; set; } = int.MaxValue;
        [BindNever]
        [JsonIgnore]
        public virtual bool IncludeDetails { get; set; } = false;
 
        public RequestPageEntityDto()
        {
            MaxMaxResultCount = int.MaxValue;
        }

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
                // 处理创建时间的开始和结束
                if (item.Name == "CreationTimeStart")
                {
                    var startValue = (DateTime?)item.GetValue(this);
                    if (startValue.HasValue)
                    {
                        startValue = startValue.Value.Date; // 设置为当天的0:00:00
                        filterConditions.Add(new FilterCondition(FirstCharToUpper(filter.Column) ?? item.Name,
                            startValue.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            filter.Logic,
                            filter.Action));
                        continue;
                    }
                }
                if (item.Name == "CreationTimeEnd")
                {
                    var endValue = (DateTime?)item.GetValue(this);
                    if (endValue.HasValue)
                    {
                        endValue = endValue.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59); // 设置为当天的23:59:59
                        filterConditions.Add(new FilterCondition(FirstCharToUpper(filter.Column) ?? item.Name,
                            endValue.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            filter.Logic,
                            filter.Action));
                        continue;
                    }
                }
                filterConditions.Add(new FilterCondition(FirstCharToUpper(filter.Column) ?? item.Name,
                    Convert.ToString(item.GetValue(this)),
                    filter.Logic,
                    filter.Action));
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
