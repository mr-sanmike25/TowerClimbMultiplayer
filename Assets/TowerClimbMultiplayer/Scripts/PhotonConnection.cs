using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    /// Date: 11/09/2024
    /// Author: Miguel Angel Garcia Elizalde y Alan Elias Carpinteyro Gastelum.
    /// Brief: Código para conectar a cuartos y servidores de Photon.

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Se ha conectado al Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Se ha entrado al Lobby Abstracto");
    }

    public override void OnJoinedRoom()
    {
        print("Se entró al room");
        PhotonNetwork.LoadLevel("Gameplay");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Hubo un error al crear el room: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Hubo un error al entrar el room: " + message);
    }

    RoomOptions NewRoomInfo()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        return roomOptions;
    }

    public void joinRoom()
    {
        //PhotonNetwork.JoinRoom(m_newInputField.text);
    }

    public void createRoom()
    {
        //PhotonNetwork.CreateRoom(m_newInputField.text, NewRoomInfo(), null);
    }
}
