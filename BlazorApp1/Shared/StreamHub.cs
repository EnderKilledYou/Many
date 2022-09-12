namespace BlazorApp1.Shared
{
    public class TaskAnnoucement
    {
        public string taskType { get; init; }
        public int taskId { get; init; }
    }
    public class TaskAssignment
    {
        public string taskType { get; init; }
        public int taskId { get; init; }
        public string Streamer { get; init; }
        public bool isClip { get; set; }
    }
    public class StreamerTask
    {
        public string Streamer { get; set; }
    }
}