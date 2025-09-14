using Quartz;

namespace RestaurantWebAPI.Jobs;

public class DbSeedJob(IDbSeeder dbSeeder) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await dbSeeder.SeedData();
    }
}
