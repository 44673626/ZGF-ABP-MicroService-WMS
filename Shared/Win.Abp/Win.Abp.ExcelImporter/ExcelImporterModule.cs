using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Win.Abp.ExcelImporter
{
    public class ExcelImporterModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();


            //context.Services.AddTransient(typeof(IExportImport<>),
            //    typeof(ExportImport<>));
        }
    }
}
