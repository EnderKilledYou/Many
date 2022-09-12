using BlazorApp1.Server.Controllers;
using ServiceStack.Data;

namespace ClipHunta2;

public class TaskRaceTaskManager : LongTaskManger<TaskRaceTask>
{
    
    private readonly IDbConnectionFactory dbConnectionFactory;

    public TaskRaceTaskManager(IDbConnectionFactory dbConnectionFactory) : base()
    {
        _longTasks = Array.Empty<TaskRaceTask>();
 
        this.dbConnectionFactory = dbConnectionFactory;
    }

    public override TaskRaceTask createOne()
    {
        return new TaskRaceTask(_cancellationToken, this.dbConnectionFactory);
    }

 
}