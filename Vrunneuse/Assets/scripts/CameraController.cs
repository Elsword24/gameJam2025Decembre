using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;
    public Vector3 closeOffset;
    public Vector3 globalOffset;
    public float transitionSpeed = 5f;

    private bool globalView = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            globalView = !globalView;
        }
    }

    private void LateUpdate()
    {
        Vector3 targetOffset = globalView ? globalOffset : closeOffset;
        Vector3 targetPosition = player.position + targetOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            transitionSpeed * Time.deltaTime
            );
    }
}
