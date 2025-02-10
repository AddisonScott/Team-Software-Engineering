using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public Camera _cam;
    [SerializeField] private Line _linePrefab;
    private Line[] _lines = new Line[2]; 
    public const float RESOLUTION = 0.1f;
    private Line _currentLine;
    private int lineNumber = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        if (lineNumber < _lines.Length)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _currentLine = Instantiate(_linePrefab);
                _lines[lineNumber] = _currentLine;
                lineNumber++;
            }
        }


        if (Input.GetMouseButton(0) && _currentLine != null)
        {
            _currentLine.SetPosition(mousePos);
        }


        if (Input.GetMouseButtonUp(0))
        {
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
                    break;
                }
            }
        }
    }
}
