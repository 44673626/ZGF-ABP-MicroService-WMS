using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Win.Utils
{
    public static class SerializeExtensions
    {
        /// <summary>
        /// 实体对象转JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ignoreNull"></param>
        /// <returns></returns>
        public static string ToJson(this object obj, bool ignoreNull = false)
        {
            var options = new JsonSerializerOptions()
            {
                IgnoreNullValues = ignoreNull,
                 Converters = { new DateTimeConverterUsingDateTimeParse() },
            };
            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// JSON字符串转实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string jsonStr)
        {
            return string.IsNullOrEmpty(jsonStr) ? default : JsonSerializer.Deserialize<T>(jsonStr);
        }

        /// <summary>
        /// 字符串序列化成字节序列
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] SerializeUtf8(this string str)
        {
            return str == null ? null : Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// 字节序列序列化成字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string DeserializeUtf8(this byte[] stream)
        {
            return stream == null ? null : Encoding.UTF8.GetString(stream);
        }

        /// <summary>
        /// JSON字符串转List<实体对象>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static List<T> FromListJson<T>(this string jsonStr)
        {
            return JsonSerializer.Deserialize<List<T>>(jsonStr);
        }
    }
}