using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBehvior : MonoBehaviour
{
    public float changeBehaviorTime;
    Ghost ghost;
    private void Start()
    {
        ghost = GetComponent<Ghost>();
    }

    public void ResetState()
    {
        Invoke("ChangeState", changeBehaviorTime);
    }
    void ChangeState()
    {
        if (ghost.currentBehaviours == GhostBehviours.Scared || ghost.currentBehaviours == GhostBehviours.Home) return;
        int Randomnum = Random.Range(0, 3);
        if (Randomnum == 0)
        {
            Debug.Log("ChangingToChase");
            ghost.currentBehaviours = GhostBehviours.Chase;
        }
        else
        {
            Debug.Log("ChangingTopatrol");

            ghost.currentBehaviours = GhostBehviours.Patrol;
        }

        ResetState();
    }
}
