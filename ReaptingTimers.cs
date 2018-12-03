using System.Threading.Tasks;
using System.Timers;

namespace DediServerCron
{
    public class ReaptingTimers
    {
        private static Timer loopingTimer;
        

        internal static Task StartTimer()
        {
            
            loopingTimer = new Timer()
            {
                Interval = 5000,
                AutoReset = true,
                Enabled = true
            };
            loopingTimer.Elapsed += OnTimerTicked;


            return Task.CompletedTask;
        }
        private static async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {

           
        }
    }
}