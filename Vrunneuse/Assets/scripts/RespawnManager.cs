using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameController Controller;
    public PlayerController PlayerController;
    public AvatarController Avatar;
    public Transform respawnPoint;

    private bool isRespawning = false;

    public void RequestRespawn()
    {
        if (isRespawning) return;

        isRespawning = true;
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        // S�curit� : une frame de latence
        yield return null;

        // D�sactiver temporairement les colliders
        SetColliders(false);

        // T�l�portations
        PlayerController.transform.position = respawnPoint.position;
        Avatar.transform.position = respawnPoint.position;

        ResetRigidBody(PlayerController);
        ResetRigidBody(Avatar);

        Controller.OnPlayerRespawn();

        // Petite latence pour sortir du trigger
        yield return new WaitForSeconds(0.1f);

        SetColliders(true);

        PlayerController.ResetRespawnState();
        isRespawning = false;
    }

    void ResetRigidBody(MonoBehaviour obj)
    {
        var rb = obj.GetComponent<Rigidbody>();
        if (!rb) return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void SetColliders(bool enabled)
    {
        foreach (var c in PlayerController.GetComponentsInChildren<Collider>())
            c.enabled = enabled;

        foreach (var c in Avatar.GetComponentsInChildren<Collider>())
            c.enabled = enabled;
    }
}
