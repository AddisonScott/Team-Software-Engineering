using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Goal : MonoBehaviour
{
    public Animator UIanimator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(endScreenTransition());
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private IEnumerator endScreenTransition()
    {
        UIanimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.0f);
        Client.Instance.Disconnect();
        SceneManager.LoadScene("EndScreen");
    }

    public void End()
    {
        StartCoroutine(endScreenTransition());
    }
}
