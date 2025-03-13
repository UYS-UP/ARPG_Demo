using System.Collections;
using System.Collections.Generic;

using Pathfinding;

using UnityEngine;

public class NavHelper 
{
    static NavHelper instance = new NavHelper();
    public static NavHelper Instance => instance;
    NNConstraint nNConstraint;
    public Vector3 GetWalkNearestPosition(Vector3 pos)
    {
        if (nNConstraint==null)
        {
            nNConstraint=new NNConstraint();
            nNConstraint.constrainWalkability = true;
            nNConstraint.walkable = true;
            nNConstraint.constrainDistance = true;
        }
        return AstarPath.active.GetNearest(pos, nNConstraint).position;

    }
}
