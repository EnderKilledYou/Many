using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

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
    public static class TableUp
    {
        public static IEnumerable<IHasId?> GetTypesWithAttribute(Type attributeType)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(x => typeof(IHasId).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(a => (IHasId?)Activator.CreateInstance(a)));

        }
        public static void DoAllTableUps(IDbConnection db)
        {
            foreach (var type in GetTypesWithAttribute(typeof(TableUpAttribute)).OrderBy((type => type.GetType().GetCustomAttribute<TableUpAttribute>().Order)))
            {


                type?.TableUp(db);
            }
        }


    }

    public class TableUpAttribute : Attribute
    {
        private readonly int order;

        public TableUpAttribute(int order)
        {
            this.order = order;
        }

        public int Order => order;
    }
}