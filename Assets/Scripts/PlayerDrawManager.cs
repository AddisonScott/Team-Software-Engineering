using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawManager : MonoBehaviour
{
    public Camera _cam;
    [SerializeField] private PlayerLine _linePrefab;
    private List<PlayerLine> _lines = new List<PlayerLine>();
    public const float RESOLUTION = 0.02f;
    private PlayerLine _currentLine;
    private int lineNumber = 0;

    public bool Active = true;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Active)
            return;

        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            _currentLine = Instantiate(_linePrefab);
            _lines.Add(_currentLine);
            lineNumber++;
        }

        if (Input.GetMouseButton(0) && _currentLine != null)
        {
            _currentLine.SetPosition(mousePos);
        }


        if (Input.GetMouseButtonUp(0))
        {
            ClientSend.CreateLine(_currentLine);
            _currentLine = null;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = _lines.Count - 1; i >= 0; i--)
            {
                if (_lines[i] != null)
                {
                    Destroy(_lines[i].gameObject);
                    _lines[i] = null;
                    lineNumber--;
                    ClientSend.RemoveLine(i);
                    break;
                }
            }
        }
    }

    public void AddLine(List<Vector2> points)
    {
        // Use a new object instead of _currentLine as _currentLine may be currently being used by the Update loop
        PlayerLine line = Instantiate(_linePrefab);
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(points[i]);
        }
        _lines.Add(line);
        lineNumber++;
    }

    public void RemoveLine(int index)
    {
        if (index >= 0 && index < _lines.Count)
        {
            Destroy(_lines[index].gameObject);
            _lines[index] = null;
            lineNumber--;
        }
    }

    public void Clear()
    {
        for (int i = _lines.Count - 1; i >= 0; i--)
        {
            if (_lines[i] != null)
            {
                Destroy(_lines[i].gameObject);
                _lines[i] = null;
                lineNumber--;
            }
        }
    }
}
