using System.Timers;

namespace DediServerCron
{
    public class RepeatingTimers
    {
        public static Timer loopTimer;

        private static void SetTimer()
        {
            loopTimer = new Timer();
        }

    }
}