using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNicknameCode : MonoBehaviour
{
    PhotonView m_PV;

    private void Start()
    {
        m_PV = GetComponent<PhotonView>();
        m_PV.Owner.NickName = PhotonNetwork.NickName;
        ShowNickname();
    }

    private void Update()
    {
        ShowNickname();
    }

    void ShowNickname()
    {
        if (m_PV.IsMine)
        {
            gameObject.GetComponentInChildren<TextMeshPro>().text = m_PV.Owner.NickName;
        }
    }
}
