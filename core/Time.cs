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
        internal static float _fixedDt = 0.0f;
        internal static DateTime _startingDate;
        internal static DateTime _previousFrameDate;
        internal static DateTime _currentFrameDate;

        internal static void _Update()
        {
            _t = (float)(_currentFrameDate.Subtract(_startingDate).TotalMilliseconds * 0.001);
            _dt = (float)(_currentFrameDate.Subtract(_previousFrameDate).TotalMilliseconds * 0.001);
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
}
