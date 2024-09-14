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
    /// Brief: Código del jugador y sus distintos comportamientos, como lo es el movimiento.

    #region Variables
    public playerStates playerCurrentState;

    [SerializeField] int m_speed;
    [SerializeField] int m_jumpForce;

    Rigidbody2D m_rb2D;
    Animator myAnim;
    PhotonView m_PV;
    #endregion

    #region UnityMethods
    void Start()
    {
        m_PV = GetComponent<PhotonView>();
        m_rb2D = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerJump();
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
        if(m_PV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_rb2D.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
            }
        }
    }
    #endregion
}
