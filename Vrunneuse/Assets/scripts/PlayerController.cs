using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    public List<ActionData> recordedActions = new List<ActionData>();
    private float startTime;
    private bool isRecording = true;
    public RespawnManager RespawnManager;
    private bool isRespawning = false;
    public float dashDuration = 0.15f;
    private bool isTouchingWall = false;
    private Vector3 wallNormal;
    public float wallSlideSpeed = -1.5f;

    public GameObject canvasPause;
    public bool IsPauseMenuOpen=false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasPause.SetActive(false);
        rigidbody = GetComponent<Rigidbody>();
        startTime = Time.time;
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
        float timestamp = Time.fixedTime - startTime;

        rigidbody.AddForce(Vector3.down * Gravity, ForceMode.Acceleration);


        if (isDashing || IsPauseMenuOpen) return;
        if (isDashing)
        {
            rigidbody.linearVelocity = new Vector3(
                   rigidbody.linearVelocity.x,
                   0f,
                   rigidbody.linearVelocity.z
                );
            return;
        }

        bool isWallSliding =
            !isGrounded &&
            isTouchingWall &&
            rigidbody.linearVelocity.y < 0f &&
            Mathf.Sign(move) == -Mathf.Sign(wallNormal.x);

        if (isWallSliding)
        {
            rigidbody.linearVelocity = new Vector3(
                0f, 
                Mathf.Max(rigidbody.linearVelocity.y, wallSlideSpeed),
                rigidbody.linearVelocity.z
            );

            return;
        }

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

        if (isRecording)
        {
            recordedActions.Add(new ActionData(
                transform.position,
                transform.rotation,
                rigidbody.linearVelocity,
                false,
                isDashing,
                isGrounded,
                timestamp
            ));

        }
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
        dashCharges--;

        Vector3 dashDirection = new Vector3(move, 0f, 0f);

        if (dashDirection == Vector3.zero) dashDirection = transform.right;

        rigidbody.useGravity = false;
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        rigidbody.AddForce(dashDirection.normalized * dashForce, ForceMode.Impulse);

        Invoke(nameof(EndDash), 0.2f);
    }

    void EndDash()
    {
        rigidbody.useGravity = true;
        isDashing = false;
    }

    private void OnCollisionStay(Collision other)
    {

        isTouchingWall = false;

        foreach(ContactPoint contact in other.contacts)
        {
            if (contact.normal.y > 0.7)
            {
                isGrounded = true;
                jumpCharge = 1;
                dashCharges = 1;
            }

            if (Math.Abs(contact.normal.x) > 0.7f)
            {
                isTouchingWall = true;
                wallNormal = contact.normal;
            }
        }

    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
        isTouchingWall = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("void"))
        {
            RespawnManager.RequestRespawn();
        }
    }
    public List<ActionData> StopRecording()
    {
        isRecording = false;
        Debug.Log($"Record stopped  send {recordedActions.Count} actions");
        return recordedActions;
    }

    public void StartRecording()
    {
        recordedActions.Clear();
        startTime = Time.time;
        isRecording = true;
        Debug.Log("StartRecording");
    }

    public void SaveActionToFile(string FilePath)
    {
        string json = JsonUtility.ToJson(new ActionWrapper { actions = recordedActions });
        File.WriteAllText(FilePath, json);
    }

    public void ResetRespawnState()
    {
        isRespawning = false;
    }

    [System.Serializable]
    public class ActionWrapper
    {
        public List<ActionData> actions;
    }


}

