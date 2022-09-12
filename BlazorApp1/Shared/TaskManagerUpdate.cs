using ClipHunta2;

public class TaskManagerUpdate
{
    public TaskManagerUpdate()
    {
    }

    public int TesseractLongTaskManager { get; init; }
    public int ImageScannerTaskManager { get; init; }
    public int EventRouterTaskManager { get; init; }
    public int ImagePrepperTaskManager { get; init; }
  //  public Dictionary<string, IGrouping<string, (StreamDefinition streamDefinition, FrameEvent frameEvent)>> Events { get; internal set; }
}