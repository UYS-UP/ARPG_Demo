using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSystem 
{
    public bool init = false;
    static GameSystem instance = new GameSystem();
    public static GameSystem Instance =>instance;

    public SceneController SceneController;
    public UCameraController CameraController;

    public EventSystem EventSystem;

    public DialogueTrigger dialogueTrigger;
}
