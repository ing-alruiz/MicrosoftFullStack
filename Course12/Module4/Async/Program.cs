using System.Diagnostics;
using System.Text.Json;

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

        try
        {
            await SimulateWorkAsync("Task 1", 2000);
            await SimulateWorkAsync("Task 2", 1500);
            await SimulateWorkAsync("Task 3", 1000);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in sequential async operations: {ex.Message}");
        }

        stopwatch.Stop();
        Console.WriteLine($"Total async sequential time: {stopwatch.ElapsedMilliseconds}ms\n");

        // Demo 3: Concurrent Asynchronous Operations
        Console.WriteLine("3. CONCURRENT ASYNCHRONOUS OPERATIONS:");
        stopwatch.Restart();

        try
        {
            var task1 = SimulateWorkAsync("Task 1", 2000);
            var task2 = SimulateWorkAsync("Task 2", 1500);
            var task3 = SimulateWorkAsync("Task 3", 1000);

            await Task.WhenAll(task1, task2, task3);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in concurrent async operations: {ex.Message}");
        }

        stopwatch.Stop();
        Console.WriteLine($"Total concurrent async time: {stopwatch.ElapsedMilliseconds}ms\n");

        // Demo 4: Exception handling with Task.WhenAll
        Console.WriteLine("4. EXCEPTION HANDLING WITH TASK.WHENALL:");
        await TaskWhenAllExceptionDemo();

        // Demo 5: Real-world example with HTTP calls
        Console.WriteLine("\n5. REAL-WORLD EXAMPLE - Web Requests:");
        try
        {
            await WebRequestDemo();
            await HttpResponseDataDemo(); // Add new demo
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in web request demo: {ex.Message}");
        }

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
        try // Added try-catch to handle potential exceptions
        {
            Console.WriteLine($"[ASYNC] {taskName} started");
            await Task.Delay(delayMs); // Non-blocking delay
            Console.WriteLine($"[ASYNC] {taskName} completed after {delayMs}ms");
            return $"Result of {taskName}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ASYNC] {taskName} failed: {ex.Message}");
            throw; // Rethrow the exception to be handled by the caller
        }
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

    static async Task HttpResponseDataDemo()
    {
        Console.WriteLine("\n5. EXTRACTING DATA FROM HTTP RESPONSES:");
        using var httpClient = new HttpClient();

        try
        {
            var url = "https://httpbin.org/json"; // Returns JSON data
            var response = await httpClient.GetAsync(url);

            // Method 1: Get response as string
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response as string:\n{responseContent}\n");

            // Method 2: Get response as byte array
            url = "https://httpbin.org/json";
            response = await httpClient.GetAsync(url);
            byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();
            Console.WriteLine($"Response as bytes (length): {responseBytes.Length} bytes\n");

            // Method 3: Get response headers
            Console.WriteLine("Response Headers:");
            foreach (var header in response.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
            Console.WriteLine();

            // Method 4: Get specific response properties
            Console.WriteLine($"Status Code: {response.StatusCode}");
            Console.WriteLine($"Status Description: {response.ReasonPhrase}");
            Console.WriteLine($"Content Type: {response.Content.Headers.ContentType}");
            Console.WriteLine($"Content Length: {response.Content.Headers.ContentLength}");

            // Method 5: Using HttpClient.GetStringAsync (shortcut for string content)
            Console.WriteLine("\nUsing GetStringAsync shortcut:");
            string directString = await httpClient.GetStringAsync("https://httpbin.org/uuid");
            Console.WriteLine($"Direct string response: {directString}");

            // Method 6: JSON deserialization example
            Console.WriteLine("\nJSON Deserialization Example:");
            await JsonDeserializationExample(httpClient);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in HTTP response demo: {ex.Message}");
        }
    }

    static async Task JsonDeserializationExample(HttpClient httpClient)
    {
        try
        {
            Console.WriteLine("--- Simple JSON Object ---");
            // Get a simple JSON object
            var response = await httpClient.GetAsync("https://httpbin.org/uuid");
            var jsonString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Raw JSON: {jsonString}");
            
            // Deserialize to UuidResponse object
            var uuidData = JsonSerializer.Deserialize<UuidResponse>(jsonString);
            Console.WriteLine($"UUID from object: {uuidData.uuid}");

            Console.WriteLine("\n--- Complex JSON Object ---");
            // Get a more complex JSON object
            response = await httpClient.GetAsync("https://httpbin.org/json");
            jsonString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Raw JSON: {jsonString}");
            
            // Deserialize to HttpBinResponse object
            var httpBinData = JsonSerializer.Deserialize<HttpBinResponse>(jsonString);
            Console.WriteLine($"Slideshow Title: {httpBinData.slideshow.title}");
            Console.WriteLine($"Slideshow Author: {httpBinData.slideshow.author}");
            Console.WriteLine($"Slideshow Date: {httpBinData.slideshow.date}");
            
            Console.WriteLine("Slides:");
            foreach (var slide in httpBinData.slideshow.slides)
            {
                Console.WriteLine($"  - {slide.title} (Type: {slide.type})");
                if (slide.items != null)
                {
                    foreach (var item in slide.items)
                    {
                        Console.WriteLine($"    * {item}");
                    }
                }
            }

            Console.WriteLine("\n--- Array of Objects Example ---");
            // Example with JSONPlaceholder API
            var posts = await httpClient.GetStringAsync("https://jsonplaceholder.typicode.com/posts?_limit=3");
            Console.WriteLine($"Raw JSON Array: {posts[..100]}..."); // Show first 100 chars
            
            var postObjects = JsonSerializer.Deserialize<Post[]>(posts);
            Console.WriteLine("Posts:");
            foreach (var post in postObjects)
            {
                Console.WriteLine($"ID: {post.Id}");
                Console.WriteLine($"Title: {post.Title}");
                Console.WriteLine($"Body: {post.Body[..50]}..."); // Show first 50 chars
                Console.WriteLine($"User ID: {post.UserId}\n");
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"JSON deserialization error: {ex.Message}");
        }
    }

    static async Task TaskWhenAllExceptionDemo()
    {
        Console.WriteLine("--- What happens when one task fails in Task.WhenAll? ---\n");

        // Demo 1: Task.WhenAll with one failing task
        Console.WriteLine("Scenario 1: One task fails, others succeed");
        try
        {
            var task1 = SimulateWorkWithPossibleFailure("Success Task 1", 1000, false);
            var task2 = SimulateWorkWithPossibleFailure("Failing Task 2", 1500, true);  // This will fail
            var task3 = SimulateWorkWithPossibleFailure("Success Task 3", 2000, false);

            // Task.WhenAll will throw as soon as any task fails
            var results = await Task.WhenAll(task1, task2, task3);
            Console.WriteLine("All tasks completed successfully (this won't be reached)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Task.WhenAll threw exception: {ex.Message}");
            
            // Check individual task states
            Console.WriteLine("\nChecking individual task statuses:");
            var task1 = SimulateWorkWithPossibleFailure("Success Task 1", 1000, false);
            var task2 = SimulateWorkWithPossibleFailure("Failing Task 2", 1500, true);
            var task3 = SimulateWorkWithPossibleFailure("Success Task 3", 2000, false);
            
            try { await Task.WhenAll(task1, task2, task3); }
            catch { /* Ignore for status checking */ }
            
            Console.WriteLine($"Task 1 Status: {task1.Status}");
            Console.WriteLine($"Task 2 Status: {task2.Status}");
            Console.WriteLine($"Task 3 Status: {task3.Status}");
        }

        Console.WriteLine("\n--- Alternative: Handle exceptions individually ---");
        
        // Demo 2: Handling each task's exceptions individually
        var tasks = new[]
        {
            SimulateWorkWithPossibleFailure("Task A", 1000, false),
            SimulateWorkWithPossibleFailure("Task B", 1200, true),   // This will fail
            SimulateWorkWithPossibleFailure("Task C", 800, false)
        };

        // Wait for all tasks to complete (successful or failed)
        await Task.WhenAll(tasks.Select(async task =>
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Individual task handling - Caught: {ex.Message}");
                return $"Failed: {ex.Message}";
            }
        }));

        Console.WriteLine("\nAll tasks have been processed individually.");

        // Demo 3: Using Task.WhenAll with try-catch around each task
        Console.WriteLine("\n--- Method 3: Wrap each task in try-catch ---");
        
        var safeTasks = new[]
        {
            SimulateWorkWithPossibleFailure("Safe Task 1", 900, false),
            SimulateWorkWithPossibleFailure("Safe Task 2", 1100, true),  // Will fail
            SimulateWorkWithPossibleFailure("Safe Task 3", 700, false)
        };

        var safeResults = await Task.WhenAll(safeTasks);
        
        Console.WriteLine("Results from safe execution:");
        for (int i = 0; i < safeResults.Length; i++)
        {
            Console.WriteLine($"Task {i + 1}: {safeResults[i]}");
        }
    }

    static async Task<string> SimulateWorkWithPossibleFailure(string taskName, int delayMs, bool shouldFail)
    {
        Console.WriteLine($"[ASYNC] {taskName} started");
        await Task.Delay(delayMs);
        
        if (shouldFail)
        {
            Console.WriteLine($"[ASYNC] {taskName} is about to fail!");
            throw new InvalidOperationException($"{taskName} encountered an error");
        }
        
        Console.WriteLine($"[ASYNC] {taskName} completed successfully after {delayMs}ms");
        return $"Result of {taskName}";
    }

    static async Task<string> SafeExecuteTask(Func<Task<string>> taskFactory)
    {
        try
        {
            return await taskFactory();
        }
        catch (Exception ex)
        {
            return $"Failed: {ex.Message}";
        }
    }

    // JSON response classes - these match the structure of the JSON
    public class UuidResponse
    {
        public string uuid { get; set; }
    }

    public class HttpBinResponse
    {
        public Slideshow slideshow { get; set; }
    }

    public class Slideshow
    {
        public string author { get; set; }
        public string date { get; set; }
        public Slide[] slides { get; set; }
        public string title { get; set; }
    }

    public class Slide
    {
        public string title { get; set; }
        public string type { get; set; }
        public string[] items { get; set; }
    }

    public class Post
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
