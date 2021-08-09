using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class HeartBeat
    {
        private CancellationTokenSource tokenSource;
        private int timeoutms;
        public int Timeoutms
        {
            get { return timeoutms; }
            private set { timeoutms = value; }
        }

        private DateTime heartBeatTime;
        public event EventHandler HeartBeatTimeoutEvent;
        public event EventHandler HeartBeatAliveEvent;
        ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();
        private bool alive;
        public bool Alive
        {
            get { return alive; }
            private set 
            {
                alive = value; 
            }
        }

        public HeartBeat(int millisecondsTimeout)
        {
            timeoutms = millisecondsTimeout;
        }

        public void UpdateHeartBeat(DateTime time)
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
                        lockSlim.EnterWriteLock();
                        alive = false;
                        lockSlim.ExitWriteLock();
                        HeartBeatTimeoutEvent?.Invoke(null, null);
                    }
                    else
                    {
                        lockSlim.EnterWriteLock();
                        alive = true;
                        lockSlim.ExitWriteLock();
                        HeartBeatAliveEvent?.Invoke(null, null);
                    }

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
