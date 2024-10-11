﻿using Com.Ctrip.Framework.Apollo.Enums;
using Com.Ctrip.Framework.Apollo.Logging;
using Com.Ctrip.Framework.Apollo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp;

namespace Com.ApolloConfig.ApolloCS
{
    public class MesConfigurations : ITransientDependency
    {
        private readonly IConfiguration _configuration;
        public MesConfigurations()
        { }
        public MesConfigurations(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 接入Apollo
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="jsonPath">apollo配置文件路径 如果写入appsettings.json中 则jsonPath传null即可</param>
        private void AddConfigurationApolloL(string jsonPath)
        {
            //构建IConfigurationBuilder
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(jsonPath, optional: true, reloadOnChange: false);
            //builder = builder.AddEnvironmentVariables();
            //阿波罗的日志级别调整
            LogManager.UseConsoleLogging(LogLevel.Warn);
            var options = new ApolloNewOptions();
            var root = builder.Build();
            root.Bind("Apollo", options);
            if (options.Enable)
            {
                var apolloBuilder = builder.AddApollo(root.GetSection("Apollo:Config"));

                foreach (var item in options.Namespaces)
                {
                    apolloBuilder.AddNamespace(item.Name, MatchConfigFileFormatL(item.Format));
                }
                //监听apollo配置
                Monitor(builder.Build());
            }

        }
        #region private
        /// <summary>
        /// 监听配置
        /// </summary>
        private void Monitor(IConfigurationRoot root)
        {
            //TODO 需要根据改变执行特定的操作 如 mq redis  等其他跟配置相关的中间件
            //TODO 初步思路：将需要执行特定的操作key和value放入内存字典中，在赋值操作时通过标准事件来执行特定的操作。

            //要重新Build 此时才将Apollo provider加入到ConfigurationBuilder中
            ChangeToken.OnChange(() => root.GetReloadToken(), () =>
            {
                foreach (var apolloProvider in root.Providers.Where(p => p is ApolloConfigurationProvider))
                {
                    var property = apolloProvider.GetType().BaseType.GetProperty("Data", BindingFlags.Instance | BindingFlags.NonPublic);
                    var data = property.GetValue(apolloProvider) as IDictionary<string, string>;
                    foreach (var item in data)
                    {
                        Console.WriteLine($"key {item.Key}   value {item.Value}");
                    }
                }
            });
        }

        //匹配格式
        private ConfigFileFormat MatchConfigFileFormatL(string value) => value switch
        {
            "json" => ConfigFileFormat.Json,
            "properties" => ConfigFileFormat.Properties,
            "xml" => ConfigFileFormat.Xml,
            "yml" => ConfigFileFormat.Yml,
            "yaml" => ConfigFileFormat.Yaml,
            "txt" => ConfigFileFormat.Txt,
            _ => throw new FormatException($"与apollo命名空间的所允许的类型不匹配：{string.Join(",", GetConfigFileFormat())}"),
        };
        //获取数据格式对应的枚举
        private IEnumerable<string> GetConfigFileFormat() => Enum.GetValues<ConfigFileFormat>().Select(u => u.ToString().ToLower());
        #endregion


        public void UseApollo()
        {
            var apolloJsonName = "appsettings.apollo.json";
            var filepath = Environment.CurrentDirectory + @"\" + apolloJsonName;
            if (File.Exists(filepath))
            {
                AddConfigurationApolloL(apolloJsonName);
            }
            else
            {
                throw new UserFriendlyException($"阿波罗Apollo配置文件{apolloJsonName}不存在，请检查！");
            }
            
        }
    }
}

