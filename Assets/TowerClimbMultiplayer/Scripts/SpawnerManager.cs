using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] Transform m_spawner;

    private void Start()
    {
        int posNum = Random.Range(0, m_spawner.childCount);
        PhotonNetwork.Instantiate("Player", m_spawner.GetChild(posNum).position, Quaternion.identity);
    }
}
