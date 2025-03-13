using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UInput 
{
    public static float GetAxis_Horizontal()
    {
        if (DialogueViewController.Instance.IsOpen()) {
            return 0;
        }
        var x = Input.GetAxis("Horizontal");
        return x;
    }

    public static float GetAxis_Vertical()
    {
        if (DialogueViewController.Instance.IsOpen())
        {
            return 0;
        }
        var z = Input.GetAxis("Vertical");
        return z;
    }


    public static float GetAxis_Mouse_X()
    {
        if (DialogueViewController.Instance.IsOpen())
        {
            return 0;
        }
        return Input.GetAxis("Mouse X");
    }


    public static float GetAxis_Mouse_Y()
    {
        if (DialogueViewController.Instance.IsOpen())
        {
            return 0;
        }
        return Input.GetAxis("Mouse Y");
    }

    public static float GetAxis_Mouse_ScrollWheell()
    {
        return Input.GetAxis("Mouse ScrollWheel");
    }


    public static bool GetKeyDown_Space() {
        return Input.GetKeyDown(KeyCode.Space) && DialogueViewController.Instance.IsOpen() == false;
    }

    public static bool GetMouseButtonUp_0() { 
         return Input.GetMouseButtonUp(0)&&DialogueViewController.Instance.IsOpen()==false;
    }

    public static bool GetKeyDown_Q()
    {
        return Input.GetKeyDown(KeyCode.Q) && DialogueViewController.Instance.IsOpen() == false;
    }

    public static bool GetKeyDown_E()
    {
        return Input.GetKeyDown(KeyCode.E) && DialogueViewController.Instance.IsOpen() == false; 
    }

    public static bool GetKeyDown_R()
    {
        return Input.GetKeyDown(KeyCode.R) && DialogueViewController.Instance.IsOpen() == false; 
    }

    public static bool GetKeyDown_T()
    {
        return Input.GetKeyDown(KeyCode.T) && DialogueViewController.Instance.IsOpen() == false;
    }

    internal static bool GetMouseButtonDown_1()
    {
        return Input.GetMouseButtonDown(1) && DialogueViewController.Instance.IsOpen() == false;
    }

    internal static bool GetMouseButtonUP_1()
    {
        return Input.GetMouseButtonUp(1) && DialogueViewController.Instance.IsOpen() == false;
    }

    internal static bool GetKeyUp_LeftShift()
    {
        return Input.GetKeyDown(KeyCode.LeftShift) && DialogueViewController.Instance.IsOpen() == false;
    }

    internal static bool GetMouseButton_0()
    {
       return Input.GetMouseButton(0) && DialogueViewController.Instance.IsOpen() == false;
    }
}
