using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum PlatformType
{
    A,
    B, 
    C
}

public class PlatformCode : MonoBehaviour
{
    public PlatformType platformType;

    [SerializeField] float platformSpeed;
    Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(LevelNetworkManager.Instance.RemainingTime <= 0)
        {
            PlatformMovement();
        }
    }

    private void PlatformMovement()
    {
        float speed = platformSpeed * Time.deltaTime;
        rb2D.transform.Translate(0.0f, -speed, 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Death"))
        {
            /*Vector2 platformPos1 = new Vector2(-9, 15.0f);
            Vector2 platformPos2 = new Vector2(-2, 15.0f);
            Vector2 platformPos3 = new Vector2(4.5f, 15.0f);

            switch (platformType)
            {
                case PlatformType.A:
                    transform.position = platformPos1;
                    break;
                case PlatformType.B:
                    transform.position = platformPos2;
                    break;
                case PlatformType.C:
                    transform.position = platformPos3;
                    break;
            }*/

            //Destroy(gameObject);
        }
        if (collision.collider.CompareTag("InitialPlatform"))
        {
            //Destroy(collision.gameObject);
        }
    }
}
