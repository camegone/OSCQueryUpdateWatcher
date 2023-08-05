// Make alias to avoid namespace collisions. (Sistem.Timers.Timer vs Sistem.Threading.Timer).
using SysTimer = System.Timers;
using static System.Console;

namespace camegone.OSCQuery.Listener;

static class MainClass
{
    static void Main()
    {
        // Create Listener instance.
        using var logger = new OSCListener(Configs.AllowedClientPrefixes);

        // Create timer to call function periodically.
        var timer = new SysTimer.Timer(Configs.ListenDelayMillis);
        timer.Elapsed += (s, e) =>
        {
            // Feel free to change this scope to suit your needs.
            // To get current Data, first call async func RequestTree().
            logger.RequestTree().Wait();

        /*  // codes below are no longer used, but I'll left them here for reference
            // Then, access to the LatestTree field to obtain the tree
            var tree = logger.LatestTree; WriteLine(tree);

            var node = logger.LatestTree.GetNodeWithPath("/avatar/parameters/YOUR_OWN_PARAMETER");
            WriteLine($"{(node == null ? "NotFound" : node)}");

            // You can get the value (specify right type of valiable!)
            bool getValSuccess = logger.TryGetNodeValue("/avatar/parameters/AFK", out bool val);
            if (getValSuccess)
                WriteLine($"AFK: {val}");

            // If you want to get type of values at run-time, you can obtain it as well
            bool getValTypeSucess = logger.TryGetNodeValue("/avatar/parameters/Earmuffs", out dynamic? valt, out string type);
            if (getValTypeSucess)
                WriteLine($"Earmuffs ({type}): {valt}");
        */
        };
        // Start the timer.
        timer.Start();

        // You can subscribe actions as well (you still need to send request)
        logger.OnTreeChanged += () => {WriteLine($"OnTreeChanged: {logger.LatestTree}");};
        logger.OnTargetChanged += () => {WriteLine("OnTargetChanged");};

        // PRESS ANY KEY TO END THIS PROCESS
        Console.Read();
    }
}