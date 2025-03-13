using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierCurve 
{
    /// <summary>
    /// ���ױ���������
    /// </summary>
    /// <param name="p0">���</param>
    /// <param name="p1">������,���ڿ�������</param>
    /// <param name="p2">�յ�</param>
    /// <param name="t">0-1 ������ʱ��/�����ĵ�ʱ��</param>
    /// <returns>ָ��ʱ��������λ��</returns>
    public static Vector3 GetPointOnQuadraticBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }


    /// <summary>
    /// ���ױ���������
    /// </summary>
    /// <param name="p0">���</param>
    /// <param name="p1">�������ߵ�1</param>
    /// <param name="p2">�������ߵ�2</param>
    /// <param name="p3">�յ�</param>
    /// <param name="t">0-1 ������ʱ��/�����ĵ�ʱ��</param>
    /// <returns>ָ��ʱ��������λ��</returns>
    public static Vector3 GetPointOnCubicBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = (uuu * p0) + (3 * uu * t * p1) + (3 * u * tt * p2) + (ttt * p3);
        return p;
    }
    //(B(t) = (1-t)^2*P_0 + 2t(1-t)*P_1 + t^2*P_2)
    //(B(t) = (1-t)^3*P_0 + 3t(1-t)^2*P_1 + 3t^2(1-t)*P_2 + t^3*P_3)

}
