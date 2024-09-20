using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

#region Enums
public enum playerStates
{
    IDLE,
    MOVING,
}
#endregion

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    /// Date: 11/09/2024
    /// Author: Miguel Angel Garcia Elizalde y Alan Elias Carpinteyro Gastelum.
    /// Brief: C�digo del jugador y sus distintos comportamientos, como lo es el movimiento.

    #region Variables
    public playerStates playerCurrentState;

    [SerializeField] int m_speed;
    [SerializeField] int m_jumpForce;
    [SerializeField] bool canJump;

    Rigidbody2D m_rb2D;
    Animator myAnim;
    PhotonView m_PV;
    SpriteRenderer m_spriteRenderer;
    #endregion

    #region UnityMethods

    void Start()
    {
        m_PV = GetComponent<PhotonView>();
        m_rb2D = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        //m_PV.Owner.NickName = PhotonNetwork.NickName; // NO PEDIRLO NUNCA MÁS DE UNA VEZ.
        gameObject.name = m_PV.Owner.NickName;
        PlayersWinnerManager.Instance.Players.Add(m_PV.Owner.NickName);
        PlayersWinnerManager.Instance.PlayersCount++;
    }

    void Update()
    {
        PlayerJump();

        /*switch (playerCurrentState)
        {
            case playerStates.IDLE:
                PlayerMovement();
                break;
            case playerStates.MOVING:
                PlayerMovement();
                break;
        }*/
    }

    private void FixedUpdate()
    {
        switch (playerCurrentState)
        {
            case playerStates.IDLE:
                PlayerMovement();
                break;
            case playerStates.MOVING:
                PlayerMovement();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Death"))
        {
            UIManager.Instance.getNewInfoGame(m_PV.Owner.NickName);
            m_speed = 0;
            canJump = false;
            m_spriteRenderer.enabled = false;
            PlayersWinnerManager.Instance.DeletePlayerFromWinnerList(m_PV.Owner.NickName);
        }
        if (collision.collider.CompareTag("Platform"))
        {
            canJump = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.collider.CompareTag("InitialPlatform")) && (LevelNetworkManager.Instance.PlayerCanMove))
        {
            canJump = true;
        }
    }

    #endregion

    #region LocalMethods
    private void PlayerMovement()
    {
        if (m_PV.IsMine && LevelNetworkManager.Instance.PlayerCanMove)
        {
            float m_movementX = Input.GetAxisRaw("Horizontal");

            m_rb2D.transform.Translate(m_movementX * m_speed * Time.fixedDeltaTime, 0.0f, 0.0f);

            if (m_movementX != 0)
            {
                playerCurrentState = playerStates.MOVING;
                myAnim.SetFloat("horMovement", m_movementX);
                myAnim.SetBool("moveBool", true);
            }
            else
            {
                playerCurrentState = playerStates.IDLE;
                myAnim.SetBool("moveBool", false);
            }
        }
    }

    private void PlayerJump()
    {
        if(m_PV.IsMine && LevelNetworkManager.Instance.PlayerCanMove && canJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_rb2D.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
                canJump = false;
            }
        }
    }
    #endregion
}
