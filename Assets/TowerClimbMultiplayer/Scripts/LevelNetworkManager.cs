using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LevelNetworkManager : MonoBehaviourPunCallbacks
{
    /// Date: 14/09/2024
    /// Author: Miguel Angel Garcia Elizalde y Alan Elias Carpinteyro Gastelum.
    /// Brief: C�digo del la interfaz de usuario (UI).

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

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
        {
            Timer();

            if ((RemainingTime <= 0.0f) && (!PlayerCanMove))
            {
                m_PV.RPC("ActivateMovements", RpcTarget.AllBuffered);
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
        print("Entr� nuevo player: " + newPlayer.NickName);
    }

    [PunRPC]
    void ActivateMovements()
    {
        print("Ya entr� el segundo player y ya se pueden mover");
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
        print("Sali� el player: " + otherPlayer.NickName);
    }
}
