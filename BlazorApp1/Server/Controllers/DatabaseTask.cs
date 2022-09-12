using BlazorApp1.Shared;
using ClipHunta2;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace BlazorApp1.Server.Controllers;

  

public class TaskRaceTask : LongTask<(int watcherId, int streamerId)>
{
    private readonly IDbConnectionFactory dbConnectionFactory;

    public TaskRaceTask(CancellationTokenSource cts, ServiceStack.Data.IDbConnectionFactory dbConnectionFactory) : base(cts)
    {
        this.dbConnectionFactory = dbConnectionFactory;
    }

    protected override async Task _action((int watcherId,int streamerId) value)

    {
        (int watcherId, int streamerId) = value;
        using var db = await dbConnectionFactory.OpenAsync();

        var watchedStreamer = await db.SingleAsync<WatchedStreamer>(streamerId);

        var watcher = await db.SingleAsync<Watcher>(watcherId);

        //check for null

        if (watchedStreamer.LastCheck != DateTime.MinValue)
        {
            return;
        }
        int id = watchedStreamer.Id;
        var count = await db.UpdateOnlyAsync(() => new WatchedStreamer { LastCheck = DateTime.Now }, a => a.Id == streamerId && a.LastCheck == DateTime.MinValue);
        if (count == 1)
        {
            await db.UpdateOnlyAsync(() => new Watcher { WatchedStreamerId = id }, a => a.Id == watcherId);
        }
        
        

    }
}