using BlazorApp1.Shared;
using ClipHunta2;
using Microsoft.AspNetCore.SignalR;
using ServiceStack.Data;

namespace BlazorApp1.Server.Hubs
{
    public class StreamHub: Hub {
        private TaskRaceTaskManager taskRaceTaskManager;

        public async Task Ack(int taskId,int streamerId)
        {
            taskRaceTaskManager.GetLongTasker()?.Put(new LongTaskQueueItem<(int,int)>((taskId, streamerId)));
        }
        public async Task Update(int taskId,TaskStatus status)
        {
            
        }
        public StreamHub(TaskRaceTaskManager taskRaceTaskManager)
        {
            this.taskRaceTaskManager = taskRaceTaskManager;
        }
        public async Task TaskManagerUpdate(TaskManagerUpdate update)
        {
            //todo: this shit
        }


    }
}