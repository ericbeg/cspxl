using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace pxl
{
    class EarClippingTriangulation
    {

        bool IsPointInsideTriangle(Vector3 v, Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 av = a - v;
            Vector3 bv = b - v;
            Vector3 cv = c - v;
            double x = Vector3.Angle(av, bv) + Vector3.Angle(bv, cv) + Vector3.Angle(cv, av);
            bool isInside = Math.Abs(2.0 * Math.PI - x) < 1e-4;
            return isInside;
        }


        bool isLeftTurn(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 u = b - a;
            Vector3 v = c - b;
            Vector3 t = Vector3.Cross(u, v);
            return t.z > 0.0f;
        }


        float smallestAngle(Vector3 a, Vector3 b, Vector3 c)
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

        void getTriangleFromEar(List<int> cropped,  int v, out int v0, out  int v1, out int v2)
        {
            v1 = v;
            v0 = (v1 == 0) ? cropped.Count - 1 : v1 - 1;
            v2 = (v1 + 1) % cropped.Count;
        }

        bool isEar(List<Vector3> polygon, List<int> cropped, int v)
        {
            int v0, v1, v2;
            getTriangleFromEar(cropped, v, out v0, out v1, out v2);

            bool isEmptyTriangle = false;
            bool isLeftTurn_b = isLeftTurn(polygon[cropped[v0]], polygon[cropped[v1]], polygon[cropped[v2]]);

            if (isLeftTurn_b)
            {
                isEmptyTriangle = true;
                for (int i = 0; i < cropped.Count; ++i)
                {
                    if (i == v0 || i == v1 || i == v2)
                        continue;

                    if (IsPointInsideTriangle(polygon[cropped[i]], polygon[cropped[v0]], polygon[cropped[v1]], polygon[cropped[v2]]))
                    {
                        isEmptyTriangle = false;
                        break;
                    }
                }
            }

            return isLeftTurn_b && isEmptyTriangle;
        }


        void polygon_triangulation(List<Vector3> polygon, List<int> triangulation)
{
   triangulation.Clear();
   triangulation.Capacity = polygon.Count - 2 ;
   
   List<int> cropped = new List<int>();
   cropped.Capacity = polygon.Count;
   
   // put all vertex indices in cropped
   for(int v = 0; v < polygon.Count; ++v)
   {
      cropped.Add(v);
   }
   
   // Ears clipping
   bool earFound = true;
   while ( cropped.Count > 2  && earFound )
   {
      earFound = false;
      float max_min_angle = -1.0f;
      int clip_ear = 0;

      // search for the ear with the maximum smallest Vector3.Angle.
      for(int i = 0; i < cropped.Count; ++i)
      {
         if( isEar(polygon, cropped, i) )
         {
            //printf("is ear %u\n", i);
            int v0, v1, v2; getTriangleFromEar( cropped, i, out v0, out v1, out v2 );
            float min_angle = smallestAngle(  polygon[cropped[v0]], polygon[cropped[v1]], polygon[cropped[v2]] );
            if( min_angle > max_min_angle )
            {
               max_min_angle = min_angle;
               clip_ear = i;
            }
            earFound = true;
         }
      }
      
      // clip the ear; removes its ear tip vertex from polygon.
      if( earFound )
      {
         int v0, v1, v2; getTriangleFromEar( cropped, clip_ear, out v0, out v1, out v2 );
         
         // add the ear as a new triangle
         triangulation.Add( cropped[v0] );
         triangulation.Add( cropped[v1] );
         triangulation.Add( cropped[v2] );
         
         // remove the ear tip vertex from cropped
         cropped.Remove( clip_ear );

         
      }
   }
   
}

    }
}
