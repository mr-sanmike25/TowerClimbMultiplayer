using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LevelNetworkManager : MonoBehaviourPunCallbacks
{
    /// Date: 14/09/2024
    /// Author: Miguel Angel Garcia Elizalde y Alan Elias Carpinteyro Gastelum.
    /// Brief: Código del la interfaz de usuario (UI).

    #region Knobs

    [SerializeField] private bool playerCanMove;
    [SerializeField] float remainingTimeToStart = 3.0f;

    [SerializeField] GameObject[] platforms;

    #endregion

    #region RuntimeVariables

    public static LevelNetworkManager Instance;

    PhotonView m_PV;

    #endregion

    public bool PlayerCanMove { get => playerCanMove; set => playerCanMove = value; }
    public float RemainingTime { get => remainingTimeToStart; set => remainingTimeToStart = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            m_PV = GetComponent<PhotonView>();
        }
        else
        {
            Destroy(Instance);
        }
    }
    private void Start()
    {
        //InvokeRepeating("PlatformsSpawnFunction", 1.25f, 1.25f);
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
        {
            Timer();

            if (RemainingTime <= 0.0f)
            {
                if (!PlayerCanMove)
                {
                    m_PV.RPC("ActivateMovements", RpcTarget.AllBuffered);
                }
            }
        }
    }

    public void disconnectFromCurrentRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("Entró nuevo player: " + newPlayer.NickName);
    }

    [PunRPC]
    void ActivateMovements()
    {
        print("Ya entró el segundo player y ya se pueden mover");
        PlayerCanMove = true;
    }

    void Timer()
    {
        if (remainingTimeToStart >= 0.0f)
        {
            RemainingTime -= Time.deltaTime;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print("Salió el player: " + otherPlayer.NickName);
    }

    //[PunRPC]
    //void PlatformsFunction()
    //{
        /*int randomPlatform = UnityEngine.Random.Range(0, platforms.Length);
        int randomXPos = UnityEngine.Random.Range(-9, 7);
        Vector2 platformSpawnPos = new Vector2(randomXPos, 15.0f);*/

        /*Vector2 platformPos1 = new Vector2(-9, 15.0f);
        Vector2 platformPos2 = new Vector2(-2, 15.0f);
        Vector2 platformPos3 = new Vector2(4.5f, 15.0f);*/

        //Instantiate(platforms[randomPlatform], platformSpawnPos, Quaternion.identity);

        /*Instantiate(platforms[0], platformPos1, Quaternion.identity);
        Instantiate(platforms[1], platformPos2, Quaternion.identity);
        Instantiate(platforms[2], platformPos3, Quaternion.identity);*/
    //}

    /*void PlatformsSpawnFunction()
    {
        if ((PhotonNetwork.CurrentRoom.PlayerCount >= 4) && remainingTimeToStart <= 0.0f)
        {
            print("PlatformsSpawnFunction");
            m_PV.RPC("PlatformsFunction", RpcTarget.AllBuffered);
        }
    }*/
}
