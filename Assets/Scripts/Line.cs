using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private EdgeCollider2D _collider;

    private List<Vector2> _points = new List<Vector2>();
   

    public int distanceLimit = 3;
    public void SetPosition(Vector2 pos) { 
        if (!CanAppend(pos)) return;

        _points.Add(pos);

        _renderer.positionCount++;
        _renderer.SetPosition(_renderer.positionCount - 1, pos);

        _collider.points = _points.ToArray(); 
    }

    private bool CanAppend(Vector2 pos)
    {
        if (_renderer.positionCount == 0) return true; //always true for first point
        if (GetTotalDistance() >= distanceLimit) return false; //returns false when line too big
        return Vector2.Distance(_renderer.GetPosition(_renderer.positionCount - 1), pos) > DrawManager.RESOLUTION; //checks distance between last point
    }

    private float GetTotalDistance()
    {
        //Finds total distance between each point
        float totalDistance = 0f;
        if (_points.Count > 1) //if list only had one element, you couldn't find the distance
        {
            for (int i = 1; i < _points.Count; i++)
            {
                totalDistance += Vector2.Distance(_points[i - 1], _points[i]);
            }

        }
        return totalDistance;


    }

    public List<Vector2> GetPoints()
    {
        return _points;
    }
}
