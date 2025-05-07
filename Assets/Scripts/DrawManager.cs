using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public Camera _cam;
    [SerializeField] private Line _linePrefab;
    private Line[] _lines = new Line[3]; 
    public const float RESOLUTION = 0.1f;
    private Line _currentLine;
    private int lineNumber = 0;
    private float lastDrawTime = -0.5f;

    public bool Active = true;

    void Update()
    {
        if (!Active)
            return;

        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        if (lineNumber < _lines.Length)
        {
            if (Input.GetMouseButtonDown(0) && Time.time - lastDrawTime >= 0.5f)
            {
                _currentLine = Instantiate(_linePrefab);
                _lines[lineNumber] = _currentLine;
                lineNumber++;
                lastDrawTime = Time.time;
            }
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
            for (int i = _lines.Length - 1; i >= 0; i--)
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
        _currentLine = Instantiate(_linePrefab);
        _currentLine.GetComponent<EdgeCollider2D>().enabled = true;
        for (int i = 0; i < points.Count; i++)
        {
            _currentLine.SetPosition(points[i]);
        }
        _lines[lineNumber] = _currentLine;
        lineNumber++;
        lastDrawTime = Time.time;
        _currentLine = null;
    }

    public void RemoveLine(int index)
    {
        Debug.Log(index);
        Debug.Log(lineNumber);
        if(index >= 0 && index < lineNumber)
        {
            Destroy(_lines[index].gameObject);
            _lines[index] = null;
            lineNumber--;
        }
    }

    public void Clear()
    {
        for (int i = _lines.Length - 1; i >= 0; i--)
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
