using System.Net;
using System.Net.Sockets;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using WebSocketSharp.Server;

namespace Warb;

[Command(Description = "Watches and refreshes the browser")]
public class Command : ICommand
{
    [CommandParameter(0, Description = "Path to watch")]
    public required string WatchPath { get; init; }
    
    public async ValueTask ExecuteAsync(IConsole console)
    {
        var queue = new Queue<bool>();
        
        var fullWatchPath = Path.Combine(Directory.GetCurrentDirectory(), WatchPath);
        var watcher = new FileSystemWatcher(fullWatchPath);

        void Handler(object sender, FileSystemEventArgs e)
        {
            console.Output.WriteLine(e.FullPath);
            queue.Enqueue(true);
            RefreshBehaviour.HasUpdates = true;
        }

        watcher.Changed += Handler;
        watcher.Created += Handler;
        watcher.Deleted += Handler;

        watcher.EnableRaisingEvents = true;
        watcher.IncludeSubdirectories = true;
        
        var wssv = new WebSocketServer (9797);
        wssv.AddWebSocketService<RefreshBehaviour>("/watch");
        wssv.Start();
        
        while (true) {

            // Thread.Sleep(1000);
        }
    }
}
