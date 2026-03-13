using UnityEngine;

public class GrayHorseController : StateMachineBehaviour
{
    public int numberOfIdles = 8;
    private int lastPick = -1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int pick = Random.Range(1, numberOfIdles + 1);

        if (pick == lastPick)
        {
            pick = (pick % numberOfIdles) + 1;
        }
        lastPick = pick;
        Debug.Log("Idle " + pick);
        animator.SetInteger("IdleState", pick);
    }
}