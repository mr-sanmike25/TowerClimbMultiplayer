using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class UIManager : MonoBehaviour
{
    /// Date: 11/09/2024
    /// Author: Miguel Angel Garcia Elizalde y Alan Elias Carpinteyro Gastelum.
    /// Brief: C�digo del la interfaz de usuario (UI).

    public static UIManager Instance;

    [SerializeField] TextMeshProUGUI m_TimerText;

    [SerializeField] TextMeshProUGUI m_gameInfo;

    float remainingTime;

    PhotonView m_PV;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    private void Start()
    {
        m_PV = GetComponent<PhotonView>();
        remainingTime = LevelNetworkManager.Instance.RemainingTime;
        m_TimerText.text = "Time to start: " + remainingTime.ToString("0");
    }

    private void Update()
    {
        remainingTime = LevelNetworkManager.Instance.RemainingTime;
        m_TimerText.text = "Time to start: " + remainingTime.ToString("0");
    }

    public void leaveCurrentRoomFromEditor()
    {
        LevelNetworkManager.Instance.disconnectFromCurrentRoom();
    }

    public void addPoints()
    {
        m_PV.RPC("addPointsInUI", RpcTarget.AllBuffered, 5);
    }

    public void getNewInfoGame(string p_playerInfo)
    {
        m_PV.RPC("showNewGameInfo", RpcTarget.All, p_playerInfo);
    }

    [PunRPC]
    void showNewGameInfo(string p_name)
    {
        m_gameInfo.text = "El jugador: " + p_name + " ha quedado eliminado";
    }
}
