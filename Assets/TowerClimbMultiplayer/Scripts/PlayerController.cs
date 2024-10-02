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

    [SerializeField] GameObject punchTrigger;
    [SerializeField] float punchForce;

    [SerializeField] int m_speed;
    [SerializeField] int m_jumpForce;
    [SerializeField] bool canJump, receiveDamageBool, canPunch;

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
        punchTrigger.name = m_PV.Owner.NickName;
        PlayersWinnerManager.Instance.Players.Add(m_PV.Owner.NickName);
        canPunch = true;
    }

    void Update()
    {
        PlayerJump();
        PlayerAttack();
        //m_PV.RPC("PlayerAttack", RpcTarget.AllBuffered);

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerPunch"))
        {
            if (m_PV.IsMine)
            {
                ReceiveDamage(punchForce, collision);
                Punching(collision.name);
                UIManager.Instance.recivedDamageToPlayer(m_PV.Owner.NickName);
            }
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
        if (m_PV.IsMine && LevelNetworkManager.Instance.PlayerCanMove && canJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_rb2D.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
                canJump = false;
            }
        }
    }

    public void ReceiveDamage(float punchForce, Collider2D enemyPlayer)
    {
        if (m_PV.IsMine)
        {
            Vector2 backwardMovementPos = transform.position - enemyPlayer.transform.position;

            m_rb2D.AddForce(backwardMovementPos.normalized * punchForce, ForceMode2D.Impulse);
        }
    }

    void Punching(string enemyPlayerName)
    {
        print(enemyPlayerName + " ha golpeado a " + m_PV.Owner.NickName);
    }

    void PlayerAttack()
    {
        if (m_PV.IsMine && LevelNetworkManager.Instance.PlayerCanMove)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                punchTrigger.transform.localPosition = new Vector2(1.5f, 0.0f);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                punchTrigger.transform.localPosition = new Vector2(-1.5f, 0.0f);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                canPunch = false;
                punchTrigger.SetActive(true);
            }

            if (!canPunch)
            {
                StartCoroutine(PunchTimer());
            }
        }
    }

    IEnumerator PunchTimer()
    {
        yield return new WaitForSeconds(0.5f);
        punchTrigger.SetActive(false);
        canPunch = true;
    }
    #endregion
}
