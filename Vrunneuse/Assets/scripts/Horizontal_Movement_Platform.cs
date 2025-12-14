/*using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PlateformMover : MonoBehaviour
{
    //PUBLIC
    public float duration = 2.0f;
    public float platform_shutdown_duration = 2.0f;

    public float MinX = 0.5f; //changer par -0.5f
    public float MaxX = -4.0f; //changer par 4.0f

    //PRIVATE
    private float direction = 1.0f; // changer le sens pcq axe X à l'envers

    
    private float t;

    private bool isPaused = false;
    private float pauseStartTime;

    private Vector3 lastPosition;
    private PlayerController playerOnPlateform = null;

    private Vector3 plateformVelocity;
    private Rigidbody playerRB;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = 0.0f;
        pauseStartTime = 0.0f;
        lastPosition = transform.localPosition;
        plateformVelocity= Vector3.zero;
        

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerOnPlateform = other.gameObject.GetComponent<PlayerController>();
            playerRB = other.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerOnPlateform.externalVelocity = Vector3.zero;
            playerOnPlateform = null;
            playerRB = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPaused)
        {
            if (Time.time >= pauseStartTime + platform_shutdown_duration)
                isPaused = false;
            return;
        }
        t += (1.0f / duration) * direction * Time.fixedDeltaTime;

        if (t <= 0.0f || t >= 1.0f)
        {
            isPaused = true;
            direction *= -1.0f; // Sans doute changer
            t = Mathf.Clamp(t, 0.0f, 1.0f);
            pauseStartTime = Time.time;

        }

        float posX = Mathf.Lerp(MinX, MaxX, t);


        transform.localPosition = new Vector3(
            posX,
            transform.localPosition.y,
            transform.localPosition.z
        );

        //Debug.Log("t: " + t + " | posX: " + posX);

        Vector3 currenPosition = transform.localPosition;
        plateformVelocity = (currenPosition - lastPosition) / Time.fixedDeltaTime;


        lastPosition = currenPosition;


        if (playerOnPlateform != null)
        {
            float correctionFactor = 1.1f;
            playerOnPlateform.externalVelocity = plateformVelocity * correctionFactor;


            Debug.Log("Platform velocity: " + plateformVelocity.x +
                      " | Player velocity BEFORE: " + playerRB.linearVelocity.x);
        }
    }
}
*/