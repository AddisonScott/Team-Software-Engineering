using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    private Renderer playerRenderer;
    private PlayerMovement playerMovement;

    public Transform playerTransform;
    public GameObject deathLocation;
    public ParticleSystem[] deathEffects;
    public Animator UIanimator;
    private void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        playerMovement = GetComponent<PlayerMovement>();
        if (deathEffects.Length == 0)
        {
            Debug.LogWarning("No death effects assigned to PlayerDeath script.");
        }
    }
    public void KillPlayer()
    {
        UIanimator.SetTrigger("FadeOut");
        foreach (var ps in playerMovement.dustSystem)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting); // Stops emitting new particles but lets old ones finish
        }
        deathLocation.transform.position = playerTransform.position; // Set the death location to the player's position
        foreach (ParticleSystem ps in deathEffects)
        {
            playerRenderer.enabled = false; // Hide the player
            playerMovement.enabled = false; // Disable player movement
            ps.Play(); // Play death effects
        }
        // Start the respawn coroutine
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {

        yield return new WaitForSeconds(1.0f);

        // Stop all death effects
        foreach (ParticleSystem ps in deathEffects)
        {
            ps.Stop();  // Stop the particle system
            ps.Clear(); // Clear the particles
        }


        SceneManager.LoadScene("Addison's Scene - Character"); // Load scene
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Deadly"))
        {
            KillPlayer();
        }
    }
}

