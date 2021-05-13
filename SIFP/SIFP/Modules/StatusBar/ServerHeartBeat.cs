using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StatusBar
{
    public class ServerHeartBeat
    {
        private CancellationTokenSource tokenSource;
        private int timeoutms;
        private DateTime heartBeatTime = DateTime.Now;
        public event EventHandler HeartBeatTimeoutEvent;

        public ServerHeartBeat(int millisecondsTimeout)
        {
            timeoutms = millisecondsTimeout;
        }

        public void HeartBeat(DateTime time)
        {
            this.heartBeatTime = time;
        }

        public void StartHeartBeat(CancellationTokenSource cancellationTokenSource)
        {
            tokenSource = cancellationTokenSource;
            Task.Run(() =>
            {
                while (!tokenSource.Token.IsCancellationRequested)
                {
                    if ((DateTime.Now - heartBeatTime).TotalMilliseconds > timeoutms)
                    {
                        HeartBeatTimeoutEvent?.Invoke(null, null);
                    }
                    Thread.Sleep(timeoutms);
                }
            }, tokenSource.Token);
        }

        public void StopHeartBeat()
        {
            tokenSource.Cancel();
        }
    }
}
