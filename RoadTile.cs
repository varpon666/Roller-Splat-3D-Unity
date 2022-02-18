using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile : MonoBehaviour
{
    public bool IsPainted
    {
        set { _isPainted = value; }
        get { return _isPainted; }
    }

    private Vector3 _position;
    private bool _isPainted;

    private void Awake()
    {
        _position = transform.position;
        _isPainted = false;
    }
}
