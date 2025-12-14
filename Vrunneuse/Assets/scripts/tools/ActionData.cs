using UnityEngine;

[System.Serializable]
public class ActionData
{
    public Vector3 position;
    public Quaternion rotation;
    public string animationName;
    public float timestamp;
    public Vector3 velocity;
    public bool isJumping;
    public bool isDashing;
    public bool isGrounded;

    public ActionData(Vector3 pos, Quaternion rot, Vector3 vel, bool jumping, bool dashing, bool grounded, float time)
    {
        position = pos;
        rotation = rot;
        velocity = vel;
        isJumping = jumping;
        isDashing = dashing;
        isGrounded = grounded;
        timestamp = time;
    }
}
