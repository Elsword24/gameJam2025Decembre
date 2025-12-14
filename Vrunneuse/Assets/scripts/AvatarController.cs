using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


//Goal of this is too make an avatar that will show in first the right way, then copies the player
public class AvatarController : MonoBehaviour
{

    public List<ActionData> actionToReplay;
    private int currentIndex = 0;
    private float replayStartTime;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        replayStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (actionToReplay == null || actionToReplay.Count == 0) return;
        if (currentIndex < actionToReplay.Count)
        {
            ActionData action = actionToReplay[currentIndex];
            float elapsedTime = Time.time - replayStartTime;

            if (elapsedTime >= action.timestamp)
            {
                transform.position = transform.position = Vector3.Lerp(
                                    transform.position,
                                    action.position,
                                    10f * Time.deltaTime
                                );
                transform.rotation = action.rotation;
                if (!string.IsNullOrEmpty(action.animationName))
                {
                    animator.Play(action.animationName);
                }
                currentIndex++;
            }
        }
    }

    public void StartReplay(List<ActionData> actions)
    {
        actionToReplay = actions;
        currentIndex = 0;
        replayStartTime = Time.time;
        Debug.Log($"Replay started {actions.Count} actions to do");
    }
}
