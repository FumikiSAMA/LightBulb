using System;
using Tyrrrz.Extensions;

namespace LightBulb.Internal
{
    internal class SyncedTimer : IDisposable
    {
        private readonly Timer _timer;
        private DateTime _firstTickDateTime;
        private TimeSpan _interval;

        public bool IsEnabled
        {
            get => _timer.IsEnabled;
            set
            {
                if (value)
                    SyncInterval();
                _timer.IsEnabled = value;
            }
        }

        public TimeSpan Interval
        {
            get => _interval;
            set
            {
                if (value <= TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(Interval));

                _interval = value;
                SyncInterval();
            }
        }

        public DateTime FirstTickDateTime
        {
            get => _firstTickDateTime;
            set
            {
                _firstTickDateTime = value;
                SyncInterval();
            }
        }

        public event EventHandler Tick;

        public SyncedTimer(TimeSpan interval, DateTime firstTickDateTime)
        {
            _timer = new Timer();
            _timer.Tick += (sender, args) =>
            {
                Tick?.Invoke(this, EventArgs.Empty);
                SyncInterval();
            };

            _interval = interval;
            _firstTickDateTime = firstTickDateTime;

            SyncInterval();
        }

        public SyncedTimer(TimeSpan interval)
            : this(interval, DateTime.Today)
        {
        }

        public SyncedTimer()
            : this(TimeSpan.FromMilliseconds(100), DateTime.Today)
        {
        }

        private void SyncInterval()
        {
            var now = DateTime.Now;

            if (now < FirstTickDateTime)
            {
                _timer.Interval = FirstTickDateTime - now;
            }
            else
            {
                var timePassed = now - FirstTickDateTime;
                var totalTicks = timePassed.TotalMilliseconds / Interval.TotalMilliseconds;
                var msUntilNextTick = (Math.Ceiling(totalTicks) - totalTicks) * Interval.TotalMilliseconds;
                msUntilNextTick = msUntilNextTick.ClampMin(1);
                _timer.Interval = TimeSpan.FromMilliseconds(msUntilNextTick);
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}