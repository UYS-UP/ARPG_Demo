using System;
using System.Collections;
using System.Collections.Generic;
using Game.Config;

using UnityEngine;

public class GameEvent 
{
    public static Action<int, bool> DOHitlag;
    public static Action<RadialBlurConfig> DORadialBlur;

    public static Action<FSM, SkillEntity> OnPlayerAtk;


    public static Action ResetSortOrder;

    public static Action OnSceneLoadComplete;
}
