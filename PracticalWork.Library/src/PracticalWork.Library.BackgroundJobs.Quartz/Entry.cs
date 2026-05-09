using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticalWork.Library.BackgroundJobs.Quartz.Interfaces;
using PracticalWork.Library.BackgroundJobs.Quartz.Listeners;
using PracticalWork.Library.BackgroundJobs.Quartz.Options;
using Quartz;

namespace PracticalWork.Library.BackgroundJobs.Quartz;

public static class Entry
{
    /// <summary>
    /// Добавления зависимостей для работы с фоновыми задачами
    /// </summary>
    public static IServiceCollection AddQuartz(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JobSettings>(configuration.GetSection("App:JobSettings"));    
        var jobSettings = configuration.GetSection("App:JobSettings").Get<JobSettings>();
        services.AddSingleton<LoggingJobListener>();
        
        var jobTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(t => typeof(ILibraryJob).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();
        
        services.AddQuartz(q =>
        {
            q.AddJobListener<LoggingJobListener>();

            foreach (var jobType in jobTypes)
            {
                var jobConfig = jobSettings!.Jobs[jobType.Name];
                var jobKey = new JobKey(jobType.Name);
                
                q.AddJob(jobType, jobKey, opts => opts
                    .WithIdentity(jobKey)
                    .UsingJobData("TimeoutMinutes", jobConfig.TimeoutMinutes)
                    .UsingJobData("MaxRetries", jobConfig.MaxRetries));
                
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity($"{jobType.Name}-trigger")
                    .WithCronSchedule(jobConfig.CronExpression,
                        x => x.InTimeZone(TimeZoneInfo.Utc))
                    .StartNow());
            }
        });
        
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}