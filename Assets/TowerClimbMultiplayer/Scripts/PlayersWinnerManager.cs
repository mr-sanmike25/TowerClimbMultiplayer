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

    [SerializeField] int m_playersCount = 0;

    /*[SerializeField] bool addPlayersBool = false;
    [SerializeField] bool removePlayersBool = false;*/

    PhotonView m_PV;

    public List<string> Players { get => m_players; set => m_players = value; }
    public int PlayersCount { get => m_playersCount; set => m_playersCount = value; }

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
        /*addPlayersBool = false;
        removePlayersBool = false;*/
    }

    void Update()
    {
        if ((PhotonNetwork.CurrentRoom.PlayerCount >= 2) && (LevelNetworkManager.Instance.PlayerCanMove))
        {
            CheckPlayerWhoWon();
            /*if (!addPlayersBool)
            {
                addPlayersBool = true;

                m_PV.RPC("callAddPlayersCoroutine", RpcTarget.AllBuffered);
            }

            if(Players.Count == 1)
            {
                print("Ganó el player " + Players[0]);
                UIManager.Instance.getVictoryPlayer(Players[0]);
            }*/
        }
    }

    /*[PunRPC]
    void callAddPlayersCoroutine()
    {
        StartCoroutine(AddPlayersCoroutine());
    }

    IEnumerator AddPlayersCoroutine()
    {
        yield return null;

        GameObject[] playersInGame = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in playersInGame)
        {
            Players.Add(player.name);
        }

        if (Players[0] == Players[1])
        {
            Players.Remove(Players[0]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("Detectó al player " + collision.gameObject.name);

            if (!removePlayersBool)
            {
                StartCoroutine(DeletePlayerFromWinnerListCoroutine(collision));
            }
            //m_PV.RPC("DeletePlayerFromWinnerList", RpcTarget.AllBuffered, collision);
        }
    }*/



    /*IEnumerator DeletePlayerFromWinnerListCoroutine(Collider2D player)
    {
        yield return null;

        removePlayersBool = true;
        Players.Remove(player.gameObject.name);
        removePlayersBool = false;
    }*/

    public void DeletePlayerFromWinnerList(string player)
    {
        m_PV.RPC("DeletePlayerFromWinnerListRPC", RpcTarget.AllBuffered, player);
    }

    [PunRPC]
    void DeletePlayerFromWinnerListRPC(string player)
    {
        PlayersCount--;
        Players.Remove(player);
    }

    void CheckPlayerWhoWon()
    {
        if((LevelNetworkManager.Instance.RemainingTime <= 0) && (Players.Count <= 1) && (PlayersCount <= 1) && (LevelNetworkManager.Instance.PlayerCanMove))
        {
            print(Players[0]);
            UIManager.Instance.getVictoryPlayer(Players[0]);
            Time.timeScale = 0.0f;
        }
    }
}
