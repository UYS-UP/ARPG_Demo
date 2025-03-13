using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveMove : EffectMoveBase
{
    BezierCurveMoveConfig config;
    Vector3 begin;
    Vector3 p1, p2;
    Vector3 end;
    public int type = 0;//0:二阶 1:三阶
    float elapsedTime;//经过的时间
    bool stop = false;
    public void Init(BezierCurveMoveConfig config) {

        this.config = config;
        begin = transform.position;
        p1 = transform.Find("p1").transform.position;
        var p2Obj = transform.Find("p2");
        if (p2Obj != null) {
            p2 = p2Obj.transform.position;
            type = 1;
        }
        else
        {
            type = 0;
        }

        end = transform.Find("end").position;
        stop = false;
        elapsedTime = 0;
    }

    void Update()
    {
        if (stop==false)
        {
            elapsedTime += GameTime.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / config.duration);
            if (type == 0)
            {
                _transform.position = BezierCurve.GetPointOnQuadraticBezierCurve(begin, p1, end, t);
            }
            else if (type == 1)
            {
                _transform.position = BezierCurve.GetPointOnCubicBezierCurve(begin, p1, p2, end, t);
            }
            if (t >= 1)
            {
                stop = true;
            }
        }
       
    }

    public bool debug = false;
    public int resolution = 100;//绘制曲线的分辨率
    private void OnDrawGizmos()
    {
        if (debug) {
            if (type == 0)
            {
                Gizmos.color = Color.red;//二阶 红色
                var p1 = transform.Find("p1").transform.position;
                var end = transform.Find("end").position;
                Vector3 begin = transform.position;
                Vector3 last_point = begin;
                Gizmos.DrawSphere(last_point, 0.1f);//绘制起点

                for (int i = 1; i <= resolution; i++)
                {
                    float t = (float)i / resolution;
                    var point = BezierCurve.GetPointOnQuadraticBezierCurve(begin, p1, end, t);
                    Gizmos.DrawLine(last_point, point);
                    last_point = point;
                }

            }
            else if (type == 1)
            {
                Gizmos.color = Color.green;//三阶 绿色
                var p1 = transform.Find("p1").transform.position;
                var p2 = transform.Find("p2").transform.position;
                var end = transform.Find("end").position;
                Vector3 begin = transform.position;
                Vector3 last_point = begin;
                Gizmos.DrawSphere(last_point, 0.1f);//绘制起点
                for (int i = 1; i <= resolution; i++)
                {
                    float t = (float)i / resolution;
                    var point = BezierCurve.GetPointOnCubicBezierCurve(begin, p1, p2, end, t);
                    Gizmos.DrawLine(last_point, point);
                    last_point = point;
                }
            }
        }
        
    }




}
