using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] Transform m_spawner;

    PhotonView m_PV;

    private void Start()
    {
        m_PV = GetComponent<PhotonView>();

        int posNum = Random.Range(0, m_spawner.childCount);
        PhotonNetwork.Instantiate("Player", m_spawner.GetChild(posNum).position, Quaternion.identity);

        if (m_PV.IsMine)
        {
            PhotonNetwork.Instantiate("SpawnPlatforms", transform.position, Quaternion.identity);
            PhotonNetwork.Instantiate("PlayersWinnerManager", transform.position, Quaternion.identity);
        }
    }
}
