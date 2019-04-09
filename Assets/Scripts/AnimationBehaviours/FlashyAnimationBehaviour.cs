using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class FlashyAnimationBehaviour : StateMachineBehaviour
{
    private int _updateCount = 0;
    
    private void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _updateCount++;

        if (_updateCount % 60 == 0)
        {
            animator.SetFloat("Rand", MathHelper.GetRandomFloat(0,1));
        }
    }
}
