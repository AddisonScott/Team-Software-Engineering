using UnityEngine;
using System.Collections;
public class Platform : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public float moveDuration = 1f;
    public float waitDuration = 1f;
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

        // Move the platform from start to end position
        yield return StartCoroutine(MovePlatform(startPosition.position, endPosition.position));

        // Wait for the specified duration at the end position
        yield return new WaitForSeconds(waitDuration);

        // Move the platform back to the start position
        yield return StartCoroutine(MovePlatform(endPosition.position, startPosition.position));
    }

    private IEnumerator MovePlatform(Vector2 fromPos, Vector2 toPos)
    {
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(fromPos, toPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = toPos;
    }


}
