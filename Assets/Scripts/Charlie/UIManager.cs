using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InputField m_UsernameField;
    [SerializeField] private InputField m_IPField;

    [SerializeField] private Client m_Client;

    public void Connect()
    {
        string username = m_UsernameField.text;
        string ip = m_IPField.text;

        if (username == "")
        {
            Debug.Log("Please enter a username");
            return;
        }
        else if (ip == "")
        {
            Debug.Log("Please enter an IP");
            return;
        }

        m_Client.Connect(ip, username);
    }
}
