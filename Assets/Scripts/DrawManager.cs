using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public Camera _cam;
    [SerializeField] private Line _linePrefab;

    public const float RESOLUTION = 0.1f;
    private Line _currentLine;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint( Input.mousePosition );

        if (Input.GetMouseButtonDown(0)) _currentLine = Instantiate(_linePrefab);

        if(Input.GetMouseButton(0)) _currentLine.SetPosition( mousePos );
    }
}
