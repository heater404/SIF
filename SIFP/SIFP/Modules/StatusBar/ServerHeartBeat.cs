using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private DateTime heartBeatTime;
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
            Debug.WriteLine("StartHeartBeat");
            heartBeatTime = DateTime.Now;
            tokenSource = cancellationTokenSource;
            Task.Run(() =>
            {
                while (!tokenSource.Token.IsCancellationRequested)
                {
                    if ((DateTime.Now - heartBeatTime).TotalMilliseconds > timeoutms)
                    {
                        Debug.WriteLine("HeartBeatTimeout");
                        HeartBeatTimeoutEvent?.Invoke(null, null);
                    }
                    Thread.Sleep(100);
                }
            }, tokenSource.Token);
        }

        public void StopHeartBeat()
        {
            Debug.WriteLine("StopHeartBeat");
            tokenSource.Cancel();
        }
    }
}
