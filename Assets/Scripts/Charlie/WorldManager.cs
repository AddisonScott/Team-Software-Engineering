using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerPrefab;
    [SerializeField] private GameObject m_PlayerCamera;
    [SerializeField] private GameObject m_CinemachineCamera;

    [SerializeField] private GameObject m_SpectatorPlayerPrefab;
    [SerializeField] private GameObject m_OtherPlayerPrefab;

    [SerializeField] private GameObject m_DeathExplosion;

    [SerializeField] private PlayerDrawManager m_DrawManager;
    [SerializeField] private OtherPlayerLineManager m_OtherLines;

    private GameObject m_Player;
    private GameObject m_OtherPlayer;

    private void Awake()
    {
        Client.Instance.WorldManager = this;
    }

    public void CreatePlayer(Vector3 position)
    {
        if (Client.Instance.FirstPlayer)
        {
            GameObject playerCam = Instantiate(m_PlayerCamera);
            GameObject cam = Instantiate(m_CinemachineCamera);
            GameObject explosion = Instantiate(m_DeathExplosion);

            m_Player = Instantiate(m_PlayerPrefab);
            m_Player.GetComponent<PlayerDeath>().deathLocation = explosion;
            m_Player.transform.position = position;

            m_DrawManager._cam = playerCam.GetComponent<Camera>();
            cam.GetComponent<CinemachineCamera>().Target.TrackingTarget = m_Player.transform;
        }
        else
        {
            // Spawn in as spectator
            m_Player = Instantiate(m_SpectatorPlayerPrefab);
            m_DrawManager._cam = m_Player.GetComponent<Camera>();
        }
    }

    public void CreateOtherPlayer(Vector3 position)
    {
        if (!Client.Instance.FirstPlayer)
        {
            m_OtherPlayer = Instantiate(m_OtherPlayerPrefab);
            m_OtherPlayer.transform.position = position;
        }
    }

    public void PositionOtherPlayer(Vector3 newPosition)
    {
        m_OtherPlayer.transform.position = newPosition;
    }

    public void AddLine(List<Vector2> points)
    {
        if(Client.Instance.FirstPlayer)
        {
            m_OtherLines.AddLine(points);
        }
        else
        {
            m_DrawManager.AddLine(points);
        }
    }
}
