using WebSocketSharp;
using WebSocketSharp.Server;

namespace Warb;

public class RefreshBehaviour : WebSocketBehavior
{
    public static bool HasUpdates { get; set; }

    protected override void OnOpen()
    {
        while (true) {
            if (HasUpdates) {
                Send("update");
                HasUpdates = false;
            }
            Thread.Sleep(500);
        }
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        
    }
}
