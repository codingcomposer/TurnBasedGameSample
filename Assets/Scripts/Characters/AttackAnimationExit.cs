using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedRPG;

public class AttackAnimationExit : StateMachineBehaviour
{
    private UnitBehaviour unit;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(unit == null)
        {
            unit = animator.GetComponent<UnitBehaviour>();
        }
        unit.AttackAnimationFinished();
    }
}
