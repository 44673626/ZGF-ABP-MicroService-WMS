using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Win.Abp.SerialNumber
{
    public interface ISerialNumberGenerator: ISingletonDependency
    {
        Task<string> InitAsync(DateTime time,  string prefix = null);

        Task<string> CreateAsync(DateTime time,string datetimeFormat = null, string prefix = null, string separator = null,
             int numberCount = 0, int step = 0 );

        Task<string> SetAsync(DateTime time,  string prefix = null,int serial=0);
    }
}