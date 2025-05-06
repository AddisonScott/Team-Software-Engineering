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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
            //GetComponent<LineSync>().SyncLine(_currentLine);
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
                    break;
                }
            }
        }
    }
}
