using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Unity.VisualScripting;

public class PlayersWinnerManager : MonoBehaviour
{
    public static PlayersWinnerManager Instance;

    [SerializeField] List<string> m_players = new List<string>();

    PhotonView m_PV;

    public List<string> Players { get => m_players; set => m_players = value; }

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

    void Update()
    {
        if ((PhotonNetwork.CurrentRoom.PlayerCount >= 2) && (LevelNetworkManager.Instance.PlayerCanMove))
        {
            CheckPlayerWhoWon();
        }
    }

    public void DeletePlayerFromWinnerList(string player)
    {
        m_PV.RPC("DeletePlayerFromWinnerListRPC", RpcTarget.AllBuffered, player);
    }

    [PunRPC]
    void DeletePlayerFromWinnerListRPC(string player)
    {
        Players.Remove(player);
    }

    void CheckPlayerWhoWon()
    {
        if((LevelNetworkManager.Instance.RemainingTime <= 0) && (Players.Count <= 1))
        {
            print(Players[0]);
            UIManager.Instance.getVictoryPlayer(Players[0]);
            Time.timeScale = 0.0f;
        }
    }
}
