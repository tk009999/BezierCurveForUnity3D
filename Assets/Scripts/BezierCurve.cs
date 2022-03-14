using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    private static float[] Factorial = new float[]
    {
        1.0f,
        1.0f,
        2.0f,
        6.0f,
        24.0f,
        120.0f,
        720.0f,
        5040.0f,
        40320.0f,
        362880.0f,
        3628800.0f,
        39916800.0f,
        479001600.0f,
        6227020800.0f,
        87178291200.0f,
        1307674368000.0f,
        20922789888000.0f,
    };

    /// <summary>
    /// https://lh3.googleusercontent.com/oDGiXFR-y1hHHvYPnHpjAc2vfjhGvsKEdS8dXk_C7JquvnhqAEMAz77aHtqdguUb4PQue2BqReDhbQ3qBiVDF2tX8eo9zen-qk8w3H5dkU12-GsZ3ZHWvYq2g5LFNcTQ0KrBFjxM
    /// </summary>
    /// <param name="n"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    private static float Binomial(int n, int i)
    {
        float ni;
        float a1 = Factorial[n];
        float a2 = Factorial[i];
        float a3 = Factorial[n - i];
        ni = a1 / (a2 * a3);
        return ni;
    }

    /// <summary>
    /// https://lh5.googleusercontent.com/1OBAGflqAb-Q34CiQHXxpam5rZeEkWb51LLWPKvClrW-_4B5GUzZZ3cRLfTMfBsqV3F8N3KbRHty1onhEi86d02D8KEg_FzPktZiSC1w4AGXi4MTVI1APAYHIpkrXV_8Fp7yU58L
    /// </summary>
    /// <param name="n"></param>
    /// <param name="i"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private static float Bernstein(int n, int i, float t)
    {
        float t_i = Mathf.Pow(t, i);
        float t_n_minus_i = Mathf.Pow((1 - t), (n - i));
        float basis = Binomial(n, i) * t_i * t_n_minus_i;
        return basis;
    }

    /// <summary>
    /// https://lh4.googleusercontent.com/9RH2Bj_QBkXVMlW646QjJqUijow5zsovgLM1s29kQy5WtVOJyM5_2HczdkRv_b6rqhX54QFL0iioqUIO51zeYeRIrrIf_1xdv0BHcht6Dc-EZSXITqeQisq4BVODbH7RQswliR5L
    /// </summary>
    /// <param name="t"></param>
    /// <param name="P"></param>
    /// <returns></returns>
    public static Vector3 Point3(float t, List<Vector3> P)
    {
        int N = P.Count - 1;
        if (N > 16)
        {
            Debug.Log("You have used more than 16 control points. The maximum control points allowed is 16.");
            P.RemoveRange(16, P.Count - 16);
        }
        if (t <= 0) return P[0];
        if (t >= 1) return P[N];

        Vector3 p = new Vector3();

        for (int i = 0; i < P.Count; ++i)
        {
            Vector3 bn = Bernstein(N, i, t) * P[i];
            p += bn;
        }

        return p;
    }

    public static List<Vector3> PointList3(List<Vector3> controlPoints, float interval = 0.01f)
    {
        int N = controlPoints.Count - 1;
        if (N > 16)
        {
            Debug.Log("You have used more than 16 control points. The maximum control points allowed is 16.");
            controlPoints.RemoveRange(16, controlPoints.Count - 16);
        }

        List<Vector3> points = new List<Vector3>();
        for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
        {
            Vector3 p = new Vector3();
            for (int i = 0; i < controlPoints.Count; ++i)
            {
                Vector3 bn = Bernstein(N, i, t) * controlPoints[i];
                p += bn;
            }
            points.Add(p);
        }

        return points;
    }
}
