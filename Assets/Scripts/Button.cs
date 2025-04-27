using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public class Button : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite normalSprite;           
    public Sprite pressedSprite;           
    public GameObject player;
    public GameObject platform;
    public bool isSafe;

    bool isPressed;
    bool wasPressed;
    private void Awake()
    {
        wasPressed = false; // Initialize wasPressed to false
        isPressed = false; // Initialize the button as not pressed
        spriteRenderer.sprite = normalSprite; // Set the initial sprite to normal
    }

    void Update()
    {
        if (isPressed)
            spriteRenderer.sprite = pressedSprite;
        else
            spriteRenderer.sprite = normalSprite;

        if(!isSafe && isPressed && !wasPressed)
        {
            player.GetComponent<PlayerDeath>().KillPlayer();
            wasPressed = true; // Set wasPressed to true when the button is pressed
        }
        if (isSafe && isPressed && !wasPressed)
        {          
            platform.GetComponent<Platform>().StartMovingPlatform();
            wasPressed = true; // Set wasPressed to true when the button is pressed
            StartCoroutine(ResetButton());
        }
    }


    private IEnumerator ResetButton()
    {
        yield return new WaitForSeconds(1f); // Wait for 0.5 seconds
        isPressed = false; // Reset the button state
        wasPressed = false; // Reset wasPressed to false
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            isPressed = true; 
            Debug.Log("Button Pressed");
        }
    }
}

