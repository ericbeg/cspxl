#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;


namespace pxl
{

    public class Time
    {
        internal static float _dt = 0.0f;
        internal static float _t = 0.0f;
        internal static float _fixedDt = 0.05f;

        internal static float _previousFrameDate;
        internal static float _currentFrameDate;

        internal static float _previousFixedFrameDate;
        internal static Stopwatch _stopWatch = new Stopwatch();

        internal static void _Initialize()
        {
            _stopWatch.Start();
            Time._currentFrameDate = 0.0f;
            Time._previousFrameDate = 0.0f;
            Time._previousFixedFrameDate = 0.0f;
        }

        internal static void _Update()
        {
            _t  = _currentFrameDate; // now - t0
            _dt = _currentFrameDate - _previousFrameDate; // now - t[n-1]
        }

        public static float dt
        {
            get
            {
                return _dt;
            }
        }

        public static float fixedDt
        {
            get
            {
                return _fixedDt;
            }

            set
            {
                _fixedDt = value;
            }
        }

        public static float t
        {
            get
            {
                return _t;
            }
        }

    }

    static public class DateTimeExtensions
    {
        public static float SubstractInSeconds(this DateTime dateTime, DateTime other)
        {
            return (float)(dateTime.Subtract(other).TotalSeconds); 
        }
    }
}
