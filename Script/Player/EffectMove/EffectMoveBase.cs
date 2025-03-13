using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMoveBase : MonoBehaviour
{
    public bool yet = false;
    public Transform _transform;
    public GameObject _gameObject;
    private void Awake()
    {
        _transform = this.transform;
        _gameObject = this.gameObject;
    }

    private void OnDestroy()
    {
        _transform = null;
        _gameObject = null;
    }
}
