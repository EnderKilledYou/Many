using BlazorApp1.Shared;
using ClipHunta2;
using Microsoft.AspNetCore.SignalR.Client;
using OpenCvSharp;
using Serilog;
using Tesseract;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/testing.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();


#if TrainingData
TrainingDataTaskManager.GetInstance().AddLongTasker();
#endif
Init();

Console.WriteLine("Hello, World!");


TesseractLongTaskManager.GetInstance().Free();

static void Init()
{
    ImageScannerTaskManager.GetInstance().AddLongTasker();
    ImageScannerTaskManager.GetInstance().AddLongTasker();
    ImageScannerTaskManager.GetInstance().AddLongTasker();
    ImageScannerTaskManager.GetInstance().AddLongTasker();
    EventRouterTaskManager.GetInstance().AddLongTasker();
    TesseractLongTaskManager.GetInstance().AddLongTasker();
    TesseractLongTaskManager.GetInstance().AddLongTasker();
    ImagePrepperTaskManager.GetInstance().AddLongTasker();
    ImagePrepperTaskManager.GetInstance().AddLongTasker();
}



public sealed class HubHelper
{

    async Task WatchStream(TaskAssignment stream)
    {
        var now = DateTime.Now;
        var cancellationTokenSource = new CancellationTokenSource();
        StreamCaptureTaskStarterTask streamCaptureTaskStarterTask =
            new StreamCaptureTaskStarterTask(cancellationTokenSource, stream.Streamer, StreamCaptureType.Stream);
        var streamStatus = streamCaptureTaskStarterTask.Start(stream.Streamer);
        var lastFrame = 0;
        while (streamStatus.FinishedCount != streamStatus.FinalFrameCount)
        {
            Task.Delay(500, cancellationTokenSource.Token).Wait(cancellationTokenSource.Token);
            Console.WriteLine(streamStatus);
            Console.WriteLine("Image Scanner " + ImageScannerTaskManager.GetInstance());
            Console.WriteLine("Image Prepped" + ImagePrepperTaskManager.GetInstance());
     
            var newEvents = EventRouterTask.eventsrecv.Where(a => a.frameEvent.FrameNumber > lastFrame).OrderBy(A => A.frameEvent.Second).GroupBy(a => a.frameEvent.EventName).ToDictionary(a => a.Key);
      
            await HubHelper.GetInstance().HubConnection.SendAsync("TaskManagerUpdate", new TaskManagerUpdate()
            {
                TesseractLongTaskManager = TesseractLongTaskManager.GetInstance().TaskCountCount(),
                ImageScannerTaskManager = ImageScannerTaskManager.GetInstance().TaskCountCount(),
                EventRouterTaskManager = EventRouterTaskManager.GetInstance().TaskCountCount(),
                ImagePrepperTaskManager = ImagePrepperTaskManager.GetInstance().TaskCountCount(),
           //     Events= newEvents
            });
             
        }

        cancellationTokenSource.Cancel(false);


        var items = EventRouterTask.eventsrecv.OrderBy(A => A.frameEvent.Second).GroupBy(a => a.frameEvent.EventName).ToDictionary(a => a.Key);


        foreach (string eventName in items.Keys)
        {
            List<int> removeIndex = new();
            var evented = items[eventName].GroupBy(a => a.frameEvent.Second).Select(a => a.First()).ToList();
            var removing = false;

            var removeEnd = 0;
            for (int i = 0; i < evented.Count; i++)
            {
                (StreamDefinition streamDefinition, FrameEvent frameEvent) value = evented[i];
                if (removing)
                {
                    if (removeEnd < value.frameEvent.Second)
                    {
                        removing = false;
                    }
                    else
                    {
                        removeIndex.Add(i);
                    }

                }

                if (value.frameEvent.EventName == "elimed")
                {
                    removing = true;

                    removeEnd = value.frameEvent.Second + 8;
                }
            }
            foreach (var index in removeIndex.OrderByDescending(a => a))
            {
                evented.RemoveAt(index);
            }

            foreach (var value in evented)
            {

                Console.WriteLine($"{value.frameEvent.EventName} {value.frameEvent.Second} {value.streamDefinition.StreamerName}");
            }
            //Console.WriteLine(valueTuple.frameEvents.Select(a => a.ToString()).ToArray());
        }

        var endtime = DateTime.Now;
        var elapse = endtime - now;
        Console.WriteLine(elapse);
    }

    private HubHelper()
    {
        HubConnection = new HubConnectionBuilder()
            .WithUrl(new Uri("https://localhost:7278/StreamHub"), options =>
            {
                options.Headers.Add("Authorization", "Basic: xx");
            }).Build();

        HubConnection.Reconnected += HubConnection_Reconnected;

        HubConnection.Reconnecting += HubConnection_Reconnecting;
        HubConnection.Closed += HubConnection_Closed;


        HubConnection.On<TaskAnnoucement>("ReceiveMessage", (message) =>
        {
            HubConnection.SendAsync("Ack", message.taskId);
        });
        HubConnection.On<TaskAssignment>("ReceiveMessage", (message) =>
        {
            _ = WatchStream(message);
            HubConnection.SendAsync("AckStart", message.taskId);
        });

        HubConnection.StartAsync().Wait();
    }
    private static readonly HubHelper _hub = new();

    public HubConnection HubConnection { get; }

    public static HubHelper GetInstance()
    {
        return _hub;
    }

    private async Task HubConnection_Closed(Exception? arg)
    {

    }

    async Task HubConnection_Reconnecting(Exception? arg)
    {

    }

    async Task HubConnection_Reconnected(string? arg)
    {
        //logger
    }
}


