using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerLineManager : MonoBehaviour
{
    [SerializeField] private Line _linePrefab;
    private List<Line> _lines = new List<Line>();
    private Line _currentLine;

    public void AddLine(List<Vector2> points)
    {
        _currentLine = Instantiate(_linePrefab);
        _currentLine.GetComponent<EdgeCollider2D>().enabled = true;
        for(int i = 0; i < points.Count; i++)
        {
            _currentLine.SetPosition(points[i]);
        }
        _lines.Add(_currentLine);
        _currentLine = null;
    }
}