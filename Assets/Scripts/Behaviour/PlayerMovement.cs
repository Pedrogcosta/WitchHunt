using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    private CharacterController controller;
    public Rigidbody2D rb;
    float direction;
    bool jumpinput;
    bool jumpreleased;
    [SerializeField] private Transform Groundcheck;
    [SerializeField] private float Groundcheckradius = 0.05f;
    [SerializeField] float walkspeed = 2f;
    [SerializeField] float jumpforce = 2f;
    [SerializeField] private LayerMask collisionmask;

    [Header("Dash")]
    [SerializeField] private float VelocidadeDash = 15f;
    [SerializeField] private float DashingTime = 0.5f;
    bool dashInput;
    private Vector2 DirecaoDash;
    private bool IsDashing;
    private bool CanDash;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CanDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");
        jumpinput = Input.GetButtonDown("Jump");
        jumpreleased = Input.GetButtonUp("Jump");

        dashInput = Input.GetButtonDown("Dash");


        if(jumpinput && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }

        if(jumpreleased && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (CanDash)
        {
            Debug.Log("Candash");
        }

        if(dashInput && CanDash)
        {
            IsDashing = true;
            CanDash = false;

            DirecaoDash = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if(DirecaoDash == Vector2.zero)
            {
                DirecaoDash = new Vector2(transform.localScale.x, 0);
            }
        }

        if (IsDashing)
        {
            rb.velocity = DirecaoDash.normalized * VelocidadeDash;
        }

        if(IsGrounded())
        {
            CanDash = true;
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(direction * walkspeed, rb.velocity.y);

    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(Groundcheck.position, Groundcheckradius, collisionmask) ;
    }
}
