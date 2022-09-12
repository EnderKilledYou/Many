using ClipHunta2;

internal class TaskManagerUpdate
{
    public TaskManagerUpdate()
    {
    }

    public int TesseractLongTaskManager { get; internal set; }
    public int ImageScannerTaskManager { get; internal set; }
    public int EventRouterTaskManager { get; internal set; }
    public int ImagePrepperTaskManager { get; internal set; }
    public Dictionary<string, IGrouping<string, (StreamDefinition streamDefinition, FrameEvent frameEvent)>> Events { get; internal set; }
}