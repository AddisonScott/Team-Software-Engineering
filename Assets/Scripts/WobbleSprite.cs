using UnityEngine;

public class WobbleSprite : MonoBehaviour
{
    [SerializeField]private float rotationAmount =20f;
    [SerializeField]private float rotationSpeed = 20f; 
    private float targetZRotation;
    private float rotationTimer;

    public Rigidbody2D playerRb;
    void Update()
    {
        // "Junks" the player sprite to add a more cartoonish movement
        if (playerRb.linearVelocity.x > 0.1f || playerRb.linearVelocity.x < -0.1f)
        {
            rotationTimer += Time.deltaTime * rotationSpeed;

            float randomJunk = Mathf.Sin(rotationTimer) * rotationAmount; // Oscillates between 1 and -1 as the timer increases
            randomJunk += Random.Range(-2f, 2f); //Randomness

            targetZRotation = randomJunk;
        }
        else
        {
            targetZRotation = 0f;
        }
        Quaternion targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, targetZRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Smoothly transitions between the two values.why 
    }
}
