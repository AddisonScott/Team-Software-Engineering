using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    public Animator FadeAnimator;

    private bool m_Returning;

    private void Start()
    {
        m_Returning = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !m_Returning)
        {
            FadeAnimator.SetTrigger("FadeOut");
            m_Returning = true;
            StartCoroutine(Exit());
        }
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Addison's Start");
        Destroy(Client.Instance.gameObject);
    }
}
