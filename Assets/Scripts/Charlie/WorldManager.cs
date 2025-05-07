using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerPrefab;
    [SerializeField] private GameObject m_PlayerCamera;
    [SerializeField] private GameObject m_CinemachineCamera;

    [SerializeField] private GameObject m_SpectatorPlayerPrefab;
    [SerializeField] private GameObject m_OtherPlayerPrefab;

    [SerializeField] private GameObject m_DeathExplosion;

    [SerializeField] private PlayerDrawManager m_DrawManager;
    [SerializeField] private DrawManager m_OtherDrawManager;

    [SerializeField] private Goal m_Goal;

    public Animator FadeAnimator;

    private GameObject m_Player;
    private Vector3 m_StartPosition;
    private GameObject m_OtherPlayer;

    private GameObject m_PlayerCam;
    private GameObject m_Camera;
    private GameObject m_Explosion;

    private void Awake()
    {
        Client.Instance.WorldManager = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Disconnect();
        }
    }

    public void CreatePlayer(Vector3 position)
    {
        if (Client.Instance.FirstPlayer)
        {
            m_PlayerCam = Instantiate(m_PlayerCamera);
            m_Camera = Instantiate(m_CinemachineCamera);
            m_Explosion = Instantiate(m_DeathExplosion);

            ParticleSystem[] deathEffects = new ParticleSystem[3];
            deathEffects[0] = m_Explosion.transform.GetChild(0).GetComponent<ParticleSystem>();
            deathEffects[1] = m_Explosion.transform.GetChild(1).GetComponent<ParticleSystem>();
            deathEffects[2] = m_Explosion.transform.GetChild(2).GetComponent<ParticleSystem>();

            m_StartPosition = position;

            m_Player = Instantiate(m_PlayerPrefab);
            PlayerDeath death = m_Player.GetComponent<PlayerDeath>();
            death.deathLocation = m_Explosion;
            death.UIanimator = FadeAnimator;
            death.deathEffects = deathEffects;
            m_Player.transform.position = position;

            m_DrawManager._cam = m_PlayerCam.GetComponent<Camera>();
            m_OtherDrawManager._cam = m_Player.GetComponent<Camera>();
            m_Camera.GetComponent<CinemachineCamera>().Target.TrackingTarget = m_Player.transform;

            m_DrawManager.Active = true;
            m_OtherDrawManager.Active = false;
        }
        else
        {
            m_Player = Instantiate(m_SpectatorPlayerPrefab);
            m_StartPosition = m_Player.transform.position;
            m_DrawManager._cam = m_Player.GetComponent<Camera>();
            m_OtherDrawManager._cam = m_Player.GetComponent<Camera>();

            m_DrawManager.Active = false;
            m_OtherDrawManager.Active = true;
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

    public void PositionOtherPlayer(Vector3 newPosition, float rotation)
    {
        Vector3 rot = m_OtherPlayer.transform.localEulerAngles;
        rot.y = rotation > 0.0f ? 180.0f : 0.0f;
        m_OtherPlayer.transform.position = newPosition;
        m_OtherPlayer.transform.localEulerAngles = rot;
    }

    public void AddLine(List<Vector2> points)
    {
        if(Client.Instance.FirstPlayer)
        {
            m_OtherDrawManager.AddLine(points);
        }
        else
        {
            m_DrawManager.AddLine(points);
        }
    }

    public void RemoveLine(int lineIndex)
    {
        if(Client.Instance.FirstPlayer)
        {
            m_OtherDrawManager.RemoveLine(lineIndex);
        }
        else
        {
            m_DrawManager.RemoveLine(lineIndex);
        }
    }

    public void Win()
    {
        m_Goal.End();
    }

    public void Reset()
    {
        Destroy(m_Player);
        Destroy(m_PlayerCam);
        Destroy(m_Camera);
        Destroy(m_Explosion);

        if (!Client.Instance.FirstPlayer)
        {
            Destroy(m_OtherPlayer);
        }

        CreatePlayer(m_StartPosition);
        CreateOtherPlayer(m_StartPosition);
        m_OtherDrawManager.Clear();
        m_DrawManager.Clear();
    }

    public void Disconnect()
    {
        FadeAnimator.SetTrigger("FadeOut");
        StartCoroutine(AttemptDisconnect());
    }

    private IEnumerator AttemptDisconnect()
    {
        yield return new WaitForSeconds(1.0f);
        Client.Instance.Disconnect();
        Destroy(Client.Instance.gameObject);
        SceneManager.LoadScene("Addison's Start");
    }
}
