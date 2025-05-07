using UnityEngine;

public class PlayerSync : MonoBehaviour
{
    [SerializeField] private float m_UpdateMovementThreshold = 0.01f; // NOTE: This is the largest possible distance the player can travel before its position is updated

    private Vector3 m_PreviousPosition;

    private void Awake()
    {
        m_PreviousPosition = transform.position;
    }

    private void Update()
    {
        if (!Client.Instance.Connected())
            return;

        if(Vector3.Distance(m_PreviousPosition, transform.position) > m_UpdateMovementThreshold)
        {
            ClientSend.PlayerSync(transform.position, transform.rotation.y);
            m_PreviousPosition = transform.position;
        }
    }
}
