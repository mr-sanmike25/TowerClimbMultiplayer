using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatforms : MonoBehaviour
{
    public static SpawnPlatforms Instance;

    [SerializeField] GameObject m_newPlatform;

    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            int newRandomPositionX = Random.Range(0, 10);
            int newRandomPositionY = Random.Range(0, 10);
            Instantiate(m_newPlatform, new Vector2(newRandomPositionX, newRandomPositionY), Quaternion.identity);
        }
    }
}
