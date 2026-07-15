namespace Examples.Logging.Serilog;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogWorkerStarting(DateTimeOffset.Now);

        WriteStructuredLogging();
        WriteProperties();

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogWorkerRunning(DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }

        logger.LogWorkerStopping(DateTimeOffset.Now);
    }

    private void WriteStructuredLogging()
    {
        if (!logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        // Simple, Scalar Values
        {
            var count = 456;
            logger.LogInformation("Retrieved {Count} records", count);

            logger.LogInformation("Booleans  - {value}", true);
            logger.LogInformation("Numerics  - {value}", 123.45d);
            logger.LogInformation("Strings   - {value}", "Hello, World!");
            logger.LogInformation("Temporals - {value}", DateTimeOffset.Now);
            logger.LogInformation("Others    - {value}", Guid.NewGuid());
            logger.LogInformation("Nulls     - {value}", (object?)null);
        }

        // Collections
        {
            var values = new[] { 1, 2, 3 };
            logger.LogInformation("The list of numbers are {Numbers}", values);

            var fruit = new[] { "Apple", "Pear", "Orange" };
            logger.LogInformation("In my bowl I have {Fruit}", fruit);                  // ???
            logger.LogInformation("In my bowl I have {Fruit}", fruit.AsEnumerable());   // OK
            global::Serilog.Log.Information("In my bowl I have {Fruit}", fruit);        // OK
        }

        {
            var fruit = new Dictionary<string, int> { { "Apple", 1 }, { "Pear", 5 } };
            logger.LogInformation("In my bowl I have {Fruit}", fruit);
        }

        // Preserving Object Structure
        {
            var sensorInput = new { Latitude = 25, Longitude = 134 };
            logger.LogInformation("Processing {@SensorInput}", sensorInput);
            logger.LogInformation("Processing {$SensorInput}", sensorInput);

            var order = new Order()
            {
                OrderId = 2,
                Status = OrderStatus.Processing
            };

            logger.LogInformation("Processing by @: {@Order}", order);
            logger.LogInformation("Processing by $: {$Order}", order);
        }

        // Forcing Stringification
        {
            var unknown = new[] { 1, 2, 3 };
            logger.LogInformation("Received {$Data}", unknown);
        }
    }

    private enum OrderStatus
    {
        New,
        Processing,
        Completed
    }

    private class Order
    {
        public int OrderId { get; init; }
        public OrderStatus Status { get; init; }
    }

    private void WriteProperties()
    {
        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = "user-123",
            ["OperationType"] = "update",
        }))
        {
            // UserId and OperationType are set for all logging events in these brackets
            logger.LogInformation("Write Property is ...");
        }
    }
}
