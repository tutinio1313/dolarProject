namespace backend.Helper;

public static class Constant
{
    private static Dictionary<string, EventId> ErrorEvents = new()
    {
        {"NO_HISTORIC_API_URL", new(id: 0, name:"ERROR_MISSING_HISTORIC_API_URL")},
        {"NO_WORKER_API_URL", new(id:1, name:"NO_WORKER_API_URL")},
        {"NO_WORKER_API_URL", new(id:2, name:"NO_WORKER_API_URL")},
        {"DEFAULT_ERROR", new(id:3)}
    };

    public static EventId GetErrorEvent(string key)
    {
        try
        {
            ErrorEvents.TryGetValue(key, out EventId value);

            if(!Equals(value, null))
            {
                return value;
            }
        }
        catch
        {                           
            Console.WriteLine($"Something went wrong searching the event ${key}.");
        }
        return ErrorEvents["DEFAULT_ERROR"];
    }
}