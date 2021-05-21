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
        public event EventHandler HeartBeatAliveEvent;
        ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();
        public ServerHeartBeat(int millisecondsTimeout)
        {
            timeoutms = millisecondsTimeout;
        }

        public void HeartBeat(DateTime time)
        {
            lockSlim.EnterWriteLock();
            this.heartBeatTime = time;
            lockSlim.ExitWriteLock();
        }

        public void StartHeartBeat(CancellationTokenSource cancellationTokenSource)
        {
            Debug.WriteLine("StartHeartBeat");
            lockSlim.EnterWriteLock();
            heartBeatTime = DateTime.Now;
            lockSlim.ExitWriteLock();
            tokenSource = cancellationTokenSource;
            Task.Run(() =>
            {
                while (!tokenSource.Token.IsCancellationRequested)
                {
                    lockSlim.EnterReadLock();
                    var elapsed = DateTime.Now - heartBeatTime;
                    lockSlim.ExitReadLock();

                    if (elapsed.TotalMilliseconds > timeoutms)
                    {
                        Debug.WriteLine("HeartBeatTimeout");
                        HeartBeatTimeoutEvent?.Invoke(null, null);
                    }
                    else
                        HeartBeatAliveEvent?.Invoke(null, null);
                    Thread.Sleep(1000);
                }
            }, tokenSource.Token);
        }

        public void StopHeartBeat()
        {
            Debug.WriteLine("StopHeartBeat");
            tokenSource?.Cancel();
        }
    }
}
