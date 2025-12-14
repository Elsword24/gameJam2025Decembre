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
    private Animator animator;

    public GameObject canvasPause;
    public bool IsPauseMenuOpen = false;

    enum PlayerAnimState
    {
        Idle,
        Run,
        Jump
    }

    PlayerAnimState currentAnim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasPause.SetActive(false);
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        startTime = Time.time;
    }

    // Update is called once per frame

    private float move;

    void Update()
    {
        Debug.Log($"Grounded={isGrounded} | Y={rigidbody.linearVelocity.y} | X={rigidbody.linearVelocity.x}");
        Debug.Log(currentAnim);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape Press");
            IsPauseMenuOpen = !IsPauseMenuOpen;
            canvasPause.SetActive(IsPauseMenuOpen);
            Time.timeScale=IsPauseMenuOpen ? 0 : 1;
        }
        if (!IsPauseMenuOpen)
        {
            move = -Input.GetAxis("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space) && jumpCharge >0)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                Dash();
            }
        }
        else
        {
            move = 0f;
        }

        if (isGrounded)
        {
            Debug.Log("Landed");
        }

        if (!isGrounded)
        {
            PlayAnim(PlayerAnimState.Jump);
        }
        else if (Mathf.Abs(move) > 0.1f)
        {
            PlayAnim(PlayerAnimState.Run);
        }
        else
        {
            PlayAnim(PlayerAnimState.Idle);
        }



        animator.SetFloat("speed", Mathf.Abs(move));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalSpeed", rigidbody.linearVelocity.y);
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            jumpCharge = 1;
            dashCharges = 1;
        }
        float timestamp = Time.fixedTime - startTime;

        if (!isTouchingWall)
        {
            rigidbody.AddForce(Vector3.down * Gravity, ForceMode.Acceleration);
        }


        if (IsPauseMenuOpen) return;
        if (isDashing)
        {
            rigidbody.linearVelocity = new Vector3(
                   rigidbody.linearVelocity.x,
                   0f,
                   rigidbody.linearVelocity.z
                );
            return;

                }

        float control = isGrounded ? 1f : airControl;

        Vector3 targetVelocity = new Vector3(
            move * speed + externalVelocity.x,
            rigidbody.linearVelocity.y,
            rigidbody.linearVelocity.z
        );
       

        if (externalVelocity != Vector3.zero)
        {
            rigidbody.linearVelocity = new Vector3(
                targetVelocity.x,
                externalVelocity.y !=0.0f ? externalVelocity.y : rigidbody.linearVelocity.y,
                rigidbody.linearVelocity.z
            );
        }
        else
        {
            rigidbody.linearVelocity = Vector3.Lerp(
                rigidbody.linearVelocity,
                targetVelocity,
                control
            );
        }

        Debug.Log("Player velocity AFTER set: " + rigidbody.linearVelocity.y +
                  " | Target was: " + targetVelocity.y);
        if (isRecording)
        {
            recordedActions.Add(new ActionData(
                    transform.position,
                    transform.rotation,
                    rigidbody.linearVelocity,
                    false,
                    isDashing,
                    isGrounded,
                    Time.time - startTime
                ));
        }
    }

    void Jump()
    {
        if (jumpCharge != 0)
        {
            externalVelocity = Vector3.zero;
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

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
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

    void PlayAnim(PlayerAnimState newState)
    {
        if (currentAnim == newState) return;

        currentAnim = newState;

        animator.CrossFade(
            newState.ToString(),
            0.15f,  
            0        
        );
    }



    [System.Serializable]
    public class ActionWrapper
    {
        public List<ActionData> actions;
    }



}

