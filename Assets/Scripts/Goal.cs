using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Goal : MonoBehaviour
{
    public Animator UIanimator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            StartCoroutine(endScreenTransition());
            
        }
    }

    private IEnumerator endScreenTransition()
    {
        UIanimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.0f);
       
    }
}
