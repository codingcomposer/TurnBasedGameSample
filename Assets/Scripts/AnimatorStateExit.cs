using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorStateExit : StateMachineBehaviour
{
    public static UnityEvent exit = new UnityEvent();
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        exit.Invoke();
    }
}
