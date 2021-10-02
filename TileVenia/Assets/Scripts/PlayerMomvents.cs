using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMomvents : MonoBehaviour
{

    //Const
    const string IS_RUNNING_TRANSMISSION = "IsRunning";
    const string IS_JUMPING_TRANSMISSION = "IsJumping";

    const string GROUND_LAYER = "Ground";


    Vector2 moveInput;
    Rigidbody2D m_rigidbody2D;
    CapsuleCollider2D m_capsuleCollider2D;

    // Movemnts Config
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
 
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {

        GetObjectsComponents();
    }


    // Find the existed object components.
    private void GetObjectsComponents()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        m_capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
            Run();
            FlipSide();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }


    void OnJump(InputValue value)
    {
        if (!m_capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER)))
            return;

        if(value.isPressed)
        {
            m_rigidbody2D.velocity += new Vector2(0f, jumpSpeed);

            animator.SetBool(IS_RUNNING_TRANSMISSION, false);

            animator.SetBool(IS_JUMPING_TRANSMISSION, true);
        }

    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x* runSpeed, m_rigidbody2D.velocity.y);
        m_rigidbody2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(m_rigidbody2D.velocity.x) > Mathf.Epsilon;

        if(!animator.GetBool(IS_JUMPING_TRANSMISSION))
            animator.SetBool(IS_RUNNING_TRANSMISSION, playerHasHorizontalSpeed);

        if(m_rigidbody2D.velocity.y == 0)
            animator.SetBool(IS_JUMPING_TRANSMISSION, false);
        
    }


    void FlipSide()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(m_rigidbody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(m_rigidbody2D.velocity.x), 1f);
        }

    }
}
