using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class PlateformMover : MonoBehaviour
{
    public float duration = 0.5f;

    private float direction = 1.0f;

    private float MinX = 0.5f;
    private float MaxX = -4.0f ;

    private float t;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        t += (1.0f / duration) * direction * Time.deltaTime;
       
        if (t <= 0.0f || t >= 1.0f)
        {
            direction *= -1.0f;
            t = Mathf.Clamp(t,0.0f, 1.0f);
        }

        float posX = Mathf.Lerp(MinX, MaxX, t);

        transform.localPosition = new Vector3(
            posX,
            transform.localPosition.y,
            transform.localPosition.z
        );
        Debug.Log("t: " + t + " | posX: " + posX);
    }
}
