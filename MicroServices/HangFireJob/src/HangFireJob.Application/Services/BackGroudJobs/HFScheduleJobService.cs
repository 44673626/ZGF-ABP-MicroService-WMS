using HangFireJob.Samples.IReport.Export;
using HangFireJob.Services.Common;
using HangFireJob.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp;
using HangFireJob.IServices;
using HangFireJob.IServices.Dto;
using Microsoft.AspNetCore.ResponseCaching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using HangFireJob.Dapper;
using System.Data.Common;

namespace HangFireJob.Services.BackGroudJobs
{
    /// <summary>
    /// 单次任务（包含延时任务）,一次性“消费”任务
    /// </summary>
    public class HFScheduleJobService : ApplicationService, IHFScheduleJobService
    {
        private readonly IRepository<HttpJobDescriptor, Guid> _repository;
        private readonly DapperDbContext _dbDapperContext;
        public HFScheduleJobService(IRepository<HttpJobDescriptor, Guid> repository, DapperDbContext dbDapperContext)
        {
            _repository= repository;
            _dbDapperContext= dbDapperContext;
        }
        /// <summary>
        /// 获取所有单次任务列表
        /// </summary>
        /// <param name="jobName">过滤条件</param>
        /// <returns></returns>
        public async Task<List<HttpJobDescriptorDto>> GetHttpbackgroundjobList(string? jobName)
        {
            string sql = "SELECT Job.Id, Job.HttpUrl,Job.JobName,Job.HttpMethod,Job.JobParameter,Job.TimeSpanFromSeconds,Job.Cron,Job.Remark,job.JobType,HangFire.JobId,hangfire.Name as StateName\n" +
                "FROM [dbo].[Hangfire_HttpJob] as job\n" +
                "INNER JOIN \n" +
                "    (SELECT *, ROW_NUMBER() OVER (PARTITION BY JobId ORDER BY Id DESC) as rn \n" +
                "     FROM [hx].[State]) as hangfire\n" +
                "ON job.JobId = hangfire.JobId\n" +
                "WHERE (hangfire.rn = 1) AND (job.JobType='1' or job.JobType='2')";
            var result = await _dbDapperContext.QueryAsync<HttpJobDescriptorDto>(sql, databaseType: DatabaseType.Secondary);
            result = result.WhereIf(!string.IsNullOrEmpty(jobName), p => p.JobName.Contains(jobName));
            return result.ToList();
        }



        /// <summary>
        /// 添加后台作业
        /// </summary>
        /// <param name="jobDescriptor"></param>
        /// <returns></returns>
        public async Task<string> AddHttpbackgroundjob([FromBody] HttpJobDescriptorDto jobDescriptor)
        {
            //任务ID
            var jobId = string.Empty;
            var entity = ObjectMapper.Map<HttpJobDescriptorDto, HttpJobDescriptor>(jobDescriptor);
            if (jobDescriptor.TimeSpanFromSeconds == 0)
            {
                //立即执行
                jobId = Hangfire.BackgroundJob.Enqueue(() => HttpJobExecutor.DoRequest(jobDescriptor));
                entity.JobType = ((int)HangFireJobTypeEnum.Enqueue).ToString();//标识类型
            }
            else
            {
                //延时加载
                jobId = Hangfire.BackgroundJob.Schedule(() => HttpJobExecutor.DoRequest(jobDescriptor), 
                    TimeSpan.FromSeconds(jobDescriptor.TimeSpanFromSeconds));
                entity.JobType = ((int)HangFireJobTypeEnum.Schedule).ToString();
            }
            entity.JobId = int.Parse(jobId);
            await _repository.InsertAsync(entity);//往调度表中添加数据
            return jobId;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobname"></param>
        /// <returns></returns>
        public async Task<bool> DeleteJob(string jobId)
        {
            var getData = (await _repository.GetQueryableAsync()).FirstOrDefault(x => x.JobName == jobId);
            if (getData != null)
            {
                await _repository.DeleteAsync(getData.Id);//往调度表中删除数据
            }
            return Hangfire.BackgroundJob.Delete(jobId);
        }

        /// <summary>
        /// 重新执行任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<bool> Requeue(string jobId)
        {
            return Hangfire.BackgroundJob.Requeue(jobId);
        }
    }
}
