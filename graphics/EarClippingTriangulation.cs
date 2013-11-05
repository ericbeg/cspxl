using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace pxl
{
    /// <summary>
    /// 
    /// </summary>
    public static class EarClippingTriangulation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static uint[] Triangulate(Vector3[] polygon)
        {
            return Triangulate(polygon, Vector3.zAxis);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static uint[] Triangulate(Vector3[] polygon, Vector3 normal)
        {
            List<uint> triangulation = new List<uint>( (polygon.Length - 2)*3);
            List<uint> cropped = new List<uint>();
            cropped.Capacity = polygon.Length;

            // put all vertex indices in cropped
            for (uint v = 0; v < polygon.Length; ++v)
            {
                cropped.Add(v);
            }

            // Ears clipping
            bool earFound = true;
            while (cropped.Count > 2 && earFound)
            {
                earFound = false;
                float max_min_angle = -1.0f;
                uint clip_ear = 0;

                // search for the ear with the maximum smallest angle.
                for (uint i = 0; i < cropped.Count; ++i)
                {
                    if (IsEar(polygon, cropped, i, normal))
                    {
                        //printf("is ear %u\n", i);
                        uint v0, v1, v2; GetTriangleFromEar(cropped, i, out v0, out v1, out v2);
                        float min_angle = SmallestAngle(
                            polygon[(int)cropped[(int)v0]],
                            polygon[(int)cropped[(int)v1]],
                            polygon[(int)cropped[(int)v2]]);
                        if (min_angle > max_min_angle)
                        {
                            max_min_angle = min_angle;
                            clip_ear = i;
                        }
                        earFound = true;
                    }
                }

                // clip the ear; removes its ear tip vertex from polygon.
                if (earFound)
                {
                    uint v0, v1, v2; GetTriangleFromEar(cropped, clip_ear, out v0, out v1, out v2);

                    // add the ear as a new triangle
                    triangulation.Add(cropped[(int)v0]);
                    triangulation.Add(cropped[(int)v1]);
                    triangulation.Add(cropped[(int)v2]);

                    // remove the ear tip vertex from cropped
                    cropped.RemoveAt((int)clip_ear);
                }
            }

            return triangulation.ToArray();

        }

        private static bool IsPointInsideTriangle(Vector3 v, Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 av = a - v;
            Vector3 bv = b - v;
            Vector3 cv = c - v;
            double x = Vector3.Angle(av, bv) + Vector3.Angle(bv, cv) + Vector3.Angle(cv, av);
            bool isInside = Math.Abs(2.0 * Math.PI - x) < 1e-4;
            return isInside;
        }

        private static bool IsLeftTurn(Vector3 a, Vector3 b, Vector3 c, Vector3 normal)
        {
            Vector3 u = b - a;
            Vector3 v = c - b;
            Vector3 t = Vector3.Cross(u, v);
            return Vector3.Dot( t, normal) > 0.0f;
        }

        private static float SmallestAngle(Vector3 a, Vector3 b, Vector3 c)
        {

            Vector3 ba = b - a;
            Vector3 ca = c - a;

            Vector3 ab = a - b;
            Vector3 cb = c - b;

            Vector3 ac = a - c;
            Vector3 bc = b - c;

            float A = Vector3.Angle(ba, ca);
            float B = Vector3.Angle(ab, cb);
            float C = Vector3.Angle(ac, bc);

            return Math.Min(A, Math.Min(B, C));
        }

        private static void GetTriangleFromEar(List<uint> cropped, uint v, out uint v0, out  uint v1, out uint v2)
        {
            v1 = v;
            v0 = (v1 == 0) ? (uint)(cropped.Count - 1) : v1 - 1;
            v2 = (v1 + 1) % (uint)cropped.Count;
        }

        private static bool IsEar(Vector3[] polygon, List<uint> cropped, uint v, Vector3 normal)
        {
            uint v0, v1, v2;
            GetTriangleFromEar(cropped, v, out v0, out v1, out v2);

            bool isEmptyTriangle = false;
            bool isLeftTurn_b = IsLeftTurn(polygon[cropped[(int)v0]], polygon[cropped[(int)v1]], polygon[cropped[(int)v2]], normal);

            if (isLeftTurn_b)
            {
                isEmptyTriangle = true;
                for (uint i = 0; i < cropped.Count; ++i)
                {
                    if (i == v0 || i == v1 || i == v2)
                        continue;

                    if (IsPointInsideTriangle(polygon[cropped[(int)i]], polygon[cropped[(int)v0]], polygon[cropped[(int)v1]], polygon[cropped[(int)v2]]))
                    {
                        isEmptyTriangle = false;
                        break;
                    }
                }
            }

            return isLeftTurn_b && isEmptyTriangle;
        }

    }
}
