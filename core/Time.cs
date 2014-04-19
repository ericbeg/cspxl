#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pxl
{

    public class Time
    {
        internal static float _dt = 0.0f;
        internal static float _t = 0.0f;
        internal static float _fixedDt = 0.05f;
        internal static DateTime _startingDate;
        internal static DateTime _previousFrameDate;
        internal static DateTime _currentFrameDate;

        internal static DateTime _previousFixedFrameDate;

        internal static void _Initialize()
        {
            Time._startingDate = DateTime.Now;
            Time._currentFrameDate = Time._startingDate;
            Time._previousFrameDate = Time._startingDate;
            Time._previousFixedFrameDate = Time._startingDate;
        }

        internal static void _Update()
        {
            _t  = _currentFrameDate.SubstractInSeconds(_startingDate); // now - t0
            _dt = _currentFrameDate.SubstractInSeconds(_previousFrameDate); // now - t[n-1]
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
            return (float)(dateTime.Subtract(other).TotalMilliseconds * 0.001); 
        }
    }
}
