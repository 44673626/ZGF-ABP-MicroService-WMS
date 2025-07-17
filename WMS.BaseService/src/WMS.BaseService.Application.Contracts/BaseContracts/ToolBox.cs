using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace WMS.BaseService.BaseContracts
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举自定义名称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var displayAttribute = (DisplayAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DisplayAttribute));
            return displayAttribute?.GetName() ?? value.ToString();
        }
        /// <summary>
        /// 获取字符串类型的枚举值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetHasCodeStr(this Enum value)
        {
            return value.GetHashCode().ToString();
        }


        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static TEnum GetEnum<TEnum>(string value, string errorCode) where TEnum : struct
        {
            if (!Enum.TryParse(value, out TEnum entity))
                throw new BusinessException(message: errorCode);
            return entity;
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static bool TryGetEnum<TEnum>(string value, out TEnum entity) where TEnum : struct
        {
            return Enum.TryParse(value, out entity);
        }
    }
}
