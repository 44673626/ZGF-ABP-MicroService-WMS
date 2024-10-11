using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ApolloConfig.ApolloCS
{
    public class ApolloNewOptions
    {
        public bool Enable { get; set; }
        public List<ChildNamespace> Namespaces { get; set; }

        public class ChildNamespace
        {
            /// <summary>
            /// 命名空间名字
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 数据格式 Json/Yml/Yaml等
            /// </summary>
            public string Format { get; set; }
        }
    }
}
