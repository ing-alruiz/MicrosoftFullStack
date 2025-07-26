using System.Diagnostics;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Synchronous vs Asynchronous Programming Demo ===\n");

        // Demo 1: Basic Synchronous Operations
        Console.WriteLine("1. SYNCHRONOUS OPERATIONS:");
        var stopwatch = Stopwatch.StartNew();

        SimulateWork("Task 1", 2000); // 2 seconds
        SimulateWork("Task 2", 1500); // 1.5 seconds
        SimulateWork("Task 3", 1000); // 1 second

        stopwatch.Stop();
        Console.WriteLine($"Total synchronous time: {stopwatch.ElapsedMilliseconds}ms\n");

        // Demo 2: Basic Asynchronous Operations
        Console.WriteLine("2. ASYNCHRONOUS OPERATIONS (Sequential):");
        stopwatch.Restart();

        await SimulateWorkAsync("Task 1", 2000);
        await SimulateWorkAsync("Task 2", 1500);
        await SimulateWorkAsync("Task 3", 1000);

        stopwatch.Stop();
        Console.WriteLine($"Total async sequential time: {stopwatch.ElapsedMilliseconds}ms\n");

        // Demo 3: Concurrent Asynchronous Operations
        Console.WriteLine("3. CONCURRENT ASYNCHRONOUS OPERATIONS:");
        stopwatch.Restart();

        var task1 = SimulateWorkAsync("Task 1", 2000);
        var task2 = SimulateWorkAsync("Task 2", 1500);
        var task3 = SimulateWorkAsync("Task 3", 1000);

        await Task.WhenAll(task1, task2, task3);

        stopwatch.Stop();
        Console.WriteLine($"Total concurrent async time: {stopwatch.ElapsedMilliseconds}ms\n");

        // Demo 4: Real-world example with HTTP calls
        Console.WriteLine("4. REAL-WORLD EXAMPLE - Web Requests:");
        await WebRequestDemo();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
        
    }

    // Synchronous method - blocks the calling thread
    static void SimulateWork(string taskName, int delayMs)
    {
        Console.WriteLine($"[SYNC] {taskName} started");
        Thread.Sleep(delayMs); // Blocks the thread
        Console.WriteLine($"[SYNC] {taskName} completed after {delayMs}ms");
    }

        // Asynchronous method - doesn't block the calling thread
    static async Task<string> SimulateWorkAsync(string taskName, int delayMs)
    {
        Console.WriteLine($"[ASYNC] {taskName} started");
        await Task.Delay(delayMs); // Non-blocking delay
        Console.WriteLine($"[ASYNC] {taskName} completed after {delayMs}ms");
        return $"Result of {taskName}";
    }

    static async Task WebRequestDemo()
    {
        using var httpClient = new HttpClient();
        
        // Synchronous approach (don't do this in real apps!)
        Console.WriteLine("Synchronous web requests:");
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // These would block if we used .Result (not recommended)
            var urls = new[]
            {
                "https://httpbin.org/delay/1",
                "https://httpbin.org/delay/1",
                "https://httpbin.org/delay/1"
            };

            // Sequential async calls
            foreach (var url in urls)
            {
                var response = await httpClient.GetAsync(url);
                Console.WriteLine($"Response from {url}: {response.StatusCode}");
            }
            
            stopwatch.Stop();
            Console.WriteLine($"Sequential requests time: {stopwatch.ElapsedMilliseconds}ms");

            // Concurrent async calls
            Console.WriteLine("\nConcurrent web requests:");
            stopwatch.Restart();
            
            var tasks = urls.Select(url => httpClient.GetAsync(url)).ToArray();
            var responses = await Task.WhenAll(tasks);
            
            for (int i = 0; i < responses.Length; i++)
            {
                Console.WriteLine($"Response from {urls[i]}: {responses[i].StatusCode}");
            }
            
            stopwatch.Stop();
            Console.WriteLine($"Concurrent requests time: {stopwatch.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during web requests: {ex.Message}");
        }
    }
}
