using System;

namespace LightBulb.Internal
{
    internal class Timer : IDisposable
    {
        private bool _isBusy;
        private TimeSpan _interval;

        protected System.Timers.Timer InternalTimer { get; }

        public bool IsEnabled { get; set; }

        public TimeSpan Interval
        {
            get => _interval;
            set
            {
                if (_interval == value)
                    return;

                _interval = value;
                InternalTimer.Interval = value.TotalMilliseconds;
            }
        }

        public event EventHandler Tick;

        public Timer(TimeSpan interval)
        {
            InternalTimer = new System.Timers.Timer();
            Interval = interval;
            InternalTimer.Elapsed += (sender, args) => TimerTickInternal();
            InternalTimer.Start();
        }

        public Timer()
            : this(TimeSpan.FromMilliseconds(100))
        {
        }

        private void TimerTickInternal()
        {
            if (!IsEnabled || _isBusy) return;
            _isBusy = true;
            TimerTick();
            _isBusy = false;
        }

        protected virtual void TimerTick()
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }


        public void Dispose()
        {
            InternalTimer.Dispose();
        }
    }
}