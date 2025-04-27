using UnityEngine;
using System.Collections;
public class Platform : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public float moveDuration = 1f;

    private void Awake()
    {
        // Set the initial position of the platform to the start position
        transform.position = startPosition.position;
    }
    public void StartMovingPlatform()
    {
        StartCoroutine(MovePlatformCoroutine());
    }

    private IEnumerator MovePlatformCoroutine()
    {
        float elapsedTime = 0f;
        // Loop while the platform is moving
        while (elapsedTime < moveDuration)
        {
            // Smooth transition between start and end positions
            transform.position = Vector2.Lerp(startPosition.position, endPosition.position, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        Transform temp = startPosition;
        startPosition = endPosition;
        endPosition = temp;  
    }


}
