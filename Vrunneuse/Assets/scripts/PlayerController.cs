using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    public float jumpForce = 1.5f;
    Vector3 jump = new Vector3(0.0f, 2.0f, 0.0f);
    public int jumpCharge = 1;
    public float speed = 1.5f;
    public float dashCharges = 1;
    public float dashForce = 3;
    private bool isDashing = false;
    private bool isGrounded = false;
    public float airControl = 0.2f;
    public float Gravity = 20f;

    public GameObject canvasPause;
    public bool IsPauseMenuOpen=false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasPause.SetActive(false);
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    private float move;


    void Update()
    {
        // Gestion du menu pause avec Échap
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPauseMenuOpen = !IsPauseMenuOpen;  // Toggle l'état du menu
            canvasPause.SetActive(IsPauseMenuOpen);  // Active/désactive le canvas selon l'état
            Time.timeScale = IsPauseMenuOpen ? 0f : 1f;  // Met en pause (0) ou reprend (1) le jeu
        }
        // Désactive les inputs de mouvement si le menu est ouvert
        if (!IsPauseMenuOpen)
        {
            move = -Input.GetAxis("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Dash();
            }
        }
        else
        {
            // Réinitialise move à 0 pour éviter tout mouvement résiduel
            move = 0f;
        }
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(Vector3.down * Gravity, ForceMode.Acceleration);


        if (isDashing || IsPauseMenuOpen) return;

        float control = isGrounded ? 1f : airControl;

        Vector3 targetVelocity = new Vector3(
            move * speed,
            rigidbody.linearVelocity.y,
            rigidbody.linearVelocity.z
        );

        rigidbody.linearVelocity = Vector3.Lerp(
            rigidbody.linearVelocity,
            targetVelocity,
            control
        );

    }

    void Jump()
    {
        if (jumpCharge != 0)
        {
            rigidbody.AddForce(jump * jumpForce, ForceMode.Impulse);
            jumpCharge --;

        }
    }

    void Dash()
    {
        if (dashCharges == 0 || isDashing) return;

        isDashing = true;

        Vector3 dashDirection = new Vector3(move, 0f, 0f);

        if (dashDirection == Vector3.zero) dashDirection = transform.right;

        rigidbody.AddForce(dashDirection.normalized * dashForce, ForceMode.Impulse);
        dashCharges --;

        Invoke(nameof(EndDash), 0.2f);
    }

    void EndDash()
    {
        isDashing = false;
    }

    private void OnCollisionStay(Collision other)
    {
        isGrounded = true;
        jumpCharge = 1;
        dashCharges = 1;
    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }
}

