using ServiceStack.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ServiceStack.OrmLite;
namespace BlazorApp1.Shared
{
    public interface IHasId
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        public void TableUp(IDbConnection db);
        public void TableDown(IDbConnection db);
    }

    public abstract class HasId :IHasId
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
   
        public virtual void TableUp(IDbConnection db)
        {
            db.CreateTableIfNotExists(new Type[] { GetType() });
        }
        public virtual void TableDown(IDbConnection db)
        {
            db.DropTable(GetType());
        }

        public virtual string Name { get; set; } = "";
    }
    [TableUp(1)]
    public class WatchedStreamer : HasId
    {
        [Required]
        [Unique]
      
        public override string Name { get; set; }

        public DateTime LastCheck { get; set; } = DateTime.MinValue;

    }
    [TableUp(2)]
    [CompositeKey("WatchedStreamerId", "WatcherId")]
    public class WatchingTask : HasId
    {
        public DateTime Started { get; set; } = DateTime.MinValue;
        public DateTime Stopped { get; set; } = DateTime.MinValue;
        [Index]
        public bool Finished = false;
        [References(typeof(WatchedStreamer))]
        [Required]

        public int WatchedStreamerId { get; set; }

        [References(typeof(Watcher))]
        [Required]       
        
        public int WatcherId { get; set; }

        
        
    }
    [TableUp(2)]
    public class ClipTask : HasId
    {
        public DateTime Started { get; set; } = DateTime.MinValue;
        public DateTime Stopped { get; set; } = DateTime.MinValue;
        [Index]
        public bool Finished = false;
        [References(typeof(Clip))]
        [Required]

        public int CllpId { get; set; }

        [References(typeof(Watcher))]
        [Required]

        public int WatcherId { get; set; }



    }
    [TableUp(3)]
    public class WatchingTaskState : HasId
    {
        [References(typeof(WatchingTask))]
        [Required]

        public int WatchingTaskId { get; set; }
        public Dictionary<string, int> State { get; set; } = new Dictionary<string, int>();
    }

    [TableUp(2)]
    public class Watcher : HasId
    {
        [References(typeof(WatchedStreamer))]
   
   
        public int? WatchedStreamerId { get; set; }
    }
    [TableUp(2)]
    public class Clip : HasId
    {
    

        [Required]
        [Unique]
     
        public string ClipUrl { get; set; }

        [References(typeof(WatchedStreamer))]
        [Required]
 
        public int WatchedStreamerId { get; set; }
    }
    [TableUp(2)]
    public class ClipEvent : HasId
    {
 

        [References(typeof(Clip))]
        public int ClipId { get; set; }

        public int Start { get; set; }
        public int End { get; set; }
        public string EventType { get; set; }
    }
}