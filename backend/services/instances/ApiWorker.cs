using backend.database.repository.currency;
using backend.services.interfaces;
using common.model;

namespace backend.services.instances;

public class ApiWorker(ILogger<ApiWorker> logger, IApiService apiService, ICurrencyRepository currencyRepository) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            if (CanRun(out int TimeOut))
            {
                logger.LogInformation($"Started {DateTime.UtcNow.ToLongDateString}");
                try
                {                   
                    Exchange[] ApiResponse = await apiService.PingAPI();

                    if (!Equals(ApiResponse, null))
                    {
                        for(int index = 0; index < ApiResponse.Length; index++)
                        {
                            ApiResponse[index].ID = Guid.NewGuid().ToString();
                        }

                        logger.LogInformation("Data was retrieved from Dolar API.");
                        await currencyRepository.AddRangeAsync(ApiResponse);
                        logger.LogInformation("Data was stored succesfully.");
                    }
                    else
                    {
                        logger.LogWarning("There is no data retrieved from API Service.");
                    }
                

                }
                catch (Exception ex)
                {
                    logger.LogError($"Something went wrong invoking API Service. \n ERROR:{ex.Message}");
                    throw;
                }
            }
            else
            {
                DateTime scheduledDate = DateTime.UtcNow.AddHours(-3).AddMilliseconds(TimeOut);
                logger.LogInformation($"The worker is scheduled to run in ${scheduledDate.ToLongDateString}");
            }

            await Task.Delay(TimeOut, stoppingToken);
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
        }

        TimeOut = 3_600 * 1_000;
        return true;
    }
}