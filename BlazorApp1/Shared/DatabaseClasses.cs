using ServiceStack.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlazorApp1.Shared
{
    public interface IHasId
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public abstract class HasId
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public virtual string Name { get; set; }
    }

    public class WatchedStreamer : HasId
    {
        [Required]
        [Unique]
        [NotNull]
        public override string Name { get; set; }

        public DateTime LastCheck { get; set; } = DateTime.MinValue;
        
    }
    

    public class Watcher : HasId
    {
        [References(typeof(WatchedStreamer))]
        [Required]
        [NotNull]
        public int WatchedStreamerId { get; set; }
    }

    public class Clip : HasId
    {
    

        [Required]
        [Unique]
        [NotNull]
        public string ClipUrl { get; set; }

        [References(typeof(WatchedStreamer))]
        [Required]
        [NotNull]
        public int WatchedStreamerId { get; set; }
    }

    public class ClipEvent : HasId
    {
 

        [References(typeof(Clip))]
        public int ClipId { get; set; }

        public int Start { get; set; }
        public int End { get; set; }
        public string EventType { get; set; }
    }
}