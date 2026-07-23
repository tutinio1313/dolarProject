using common.model;

namespace worker.helpers;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            bool result = CanRun(out int timeout);
            if (logger.IsEnabled(LogLevel.Information))
            {
                Console.WriteLine($"Can run? {result}");
                if(result)
                {

                    IEnumerable<Exchange> exchanges = await ApiWorker.ExecuteAsync();

                    if(exchanges is not null && exchanges.Any())
                    {
                        SQLHelper sqlHelper = new();

                        if(await sqlHelper.LoadExchangesAsync(exchanges))
                        {
                            await ValkeyHelper.PutItemsInValkey(exchanges);   
                        }        
                    }
                }
                else
                {
                    Console.WriteLine("Process was skipped due it is not on valid time.");
                }

                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            DateTime scheduledDate = DateTime.UtcNow.AddHours(-3).AddMilliseconds(timeout);
            logger.LogInformation($"The worker is scheduled to run in {scheduledDate:yyyy:MM:dd:HH:mm:ss:z}");
            await Task.Delay(timeout, stoppingToken);
        }
    }


    private bool CanRun(out int TimeOut)
    {
        DateTime today = DateTime.UtcNow.AddHours(-3);

        if (Equals(today.DayOfWeek, DayOfWeek.Sunday))
        {
            TimeOut = ((8 + 24 - today.Hour) * 3_600 + (60 - today.Minute) * 60) * 1_000;
            return false;
        }
        else if (Equals(today.DayOfWeek, DayOfWeek.Saturday))
        {
            TimeOut = ((8 + 48 - today.Hour) * 3_600 + (60 - today.Minute) * 60) * 1_000;
            return false;
        }
        else if (Equals(today.DayOfWeek, DayOfWeek.Friday) && today.Hour > 18)
        {
            TimeOut = ((8 + 72 - today.Hour) * 3_600 + (60 - today.Minute) * 60) * 1_000;
            return false;
        }
        else
        {
            if(today.Hour < 8)
            {
                TimeOut = (((8-today.Hour)*3_600 )+ (60 - today.Minute * 60))*1_000;
            }

            else if(today.Hour > 18)
            {
                TimeOut = (((32-today.Hour)*3_600 )+ (60 - today.Minute * 60))*1_000;
            }

            else 
            {
                TimeOut = 15 * 60 * 1_000;
            }
            return true;

        }
    }
}
