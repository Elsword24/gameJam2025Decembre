using UnityEngine;

public class Vertical_Movement_Platform : MonoBehaviour
{
    //PUBLIC
    public float duration = 2.0f;
    public float platform_shutdown_duration = 2.0f;

    public float MinY = 0.5f; 
    public float MaxY = -4.0f; 

    //PRIVATE
    private float direction = 1.0f; 


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
        plateformVelocity = Vector3.zero;


    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ContactPoint contact = other.contacts[0];
            Rigidbody playerRB_temp = other.gameObject.GetComponent<Rigidbody>();
            if (contact.normal.y > 0.5f && playerRB_temp.linearVelocity.y <= 0.0f)
            {
                playerOnPlateform = other.gameObject.GetComponent<PlayerController>();
                playerRB = playerRB_temp;
                
            }
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

        float posY = Mathf.Lerp(MinY, MaxY, t);


        transform.localPosition = new Vector3(
            transform.localPosition.x,
            posY,
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


            Debug.Log("Platform velocity: " + plateformVelocity.y +
                      " | Player velocity BEFORE: " + playerRB.linearVelocity.y);
        }
    }
}
