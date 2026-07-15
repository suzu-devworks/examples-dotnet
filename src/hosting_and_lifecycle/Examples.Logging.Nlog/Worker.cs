namespace Examples.Logging.Nlog;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogWorkerStarting(DateTimeOffset.Now);

        WriteStructuredLogging();

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

        object? o = null;

        logger.LogInformation("Test(null): {value1}", o); // null case. Result:  Test NULL
        logger.LogInformation("Test(DateTime): {value1}", new DateTime(2018, 03, 25)); // datetime case. Result:  Test 25-3-2018 00:00:00 (locale ToString)
        logger.LogInformation("Test(List<string>): {value1}", new List<string> { "a", "b" }); // list of strings. Result: Test a, b
        logger.LogInformation("Test(string[]): {value1}", new[] { "a", "b" }); // array. Result: Test a, b
        logger.LogInformation("Test(Dictionary<string, int>): {value1}", new Dictionary<string, int> { { "key1", 1 }, { "key2", 2 } }); // dict. Result:  Test key1=1, key2=2

        var order = new Order()
        {
            OrderId = 2,
            Status = OrderStatus.Processing
        };

        logger.LogInformation("Test(Order): {value1}", order);  // object Result: Test MyProgram.Program+Order
        logger.LogInformation("Test(@Order): {@value1}", order); // object Result: Test {"OrderId":2, "Status":"Processing"}
        logger.LogInformation("Test(anonymous): {value1}", new { OrderId = 2, Status = "Processing" });  // anonymous object. Result: Test { OrderId = 2, Status = Processing }
        logger.LogInformation("Test(@anonymous): {@value1}", new { OrderId = 2, Status = "Processing" }); // anonymous object. Result: Test {"OrderId":2, "Status":"Processing"}
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
}
