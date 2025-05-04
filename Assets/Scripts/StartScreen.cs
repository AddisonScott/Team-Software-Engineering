using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScreen : MonoBehaviour
{
    public void PressStart()
    {
        SceneManager.LoadScene("Addison's Scene - Character");
    }
}
