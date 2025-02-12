using Unity.VisualScripting;
using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    public Camera _cam;
    private Vector2 initialPosition;
    private Vector2 finalPosition;
    private bool isDragging = false;
    private float distanceToMove;
    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            initialPosition = mousePos;
            isDragging = true;
        }
        if (Input.GetMouseButtonUp(1))
        {

            isDragging = false;
        }

        if (isDragging)
        {
            finalPosition = mousePos;
            distanceToMove = Vector2.Distance(finalPosition, initialPosition);
            moveCamera(distanceToMove);
        }
    }
    void moveCamera(float Pos)
    {
        Vector3 direction = (finalPosition - initialPosition).normalized;
        _cam.transform.position -= new Vector3(direction.x, direction.y, 0) * distanceToMove;
    }
}