using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerLine : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;

    private List<Vector2> _points = new List<Vector2>();
    private void Awake()
    {
        _renderer.sortingOrder = -1; 
    }
    public void SetPosition(Vector2 pos)
    {
        if (!CanAppend(pos)) return;
        _points.Add(pos);
        _renderer.positionCount++;
        _renderer.SetPosition(_renderer.positionCount - 1, pos);
    }

    private bool CanAppend(Vector2 pos)
    {
        if (_renderer.positionCount == 0) return true; //always true for first point
        return Vector2.Distance(_renderer.GetPosition(_renderer.positionCount - 1), pos) > PlayerDrawManager.RESOLUTION; //checks distance between last point
    }

    public List<Vector2> GetPoints()
    {
        return _points;
    }
}
