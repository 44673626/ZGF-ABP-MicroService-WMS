using Magicodes.ExporterAndImporter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Business.Samples.Boms.Dto
{
    public class BomExportDto
    {

        [ExporterHeader(DisplayName = "期间")]
        public string Version { set; get; }
        /// <summary>
        /// 工厂
        /// </summary>
        [ExporterHeader(DisplayName = "工厂")]
        public string Factory { set; get; }


        [ExporterHeader(DisplayName = "ERP总成物料号")]
        public string ParentItemCode { get; set; }

        [ExporterHeader(DisplayName = "ERP总成物料号描述")]
        public string ParentItemDesc { get; set; }

        [ExporterHeader(DisplayName = "ERP组件物料号")]
        public string ChildItemCode { get; set; }

        [ExporterHeader(DisplayName = "ERP组件物料号描述")]
        public string ChildItemDesc { get; set; }

        [ExporterHeader(DisplayName = "组件数量")]
        public decimal Qty { get; set; }

        [ExporterHeader(DisplayName = "组件计量单位")]
        public string ChildItemUom { get; set; }
    }
}
