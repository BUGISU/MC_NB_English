using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Animator CharacterAnimator;
    public void SetAnimatorTrigger(string parameter)
    {
        CharacterAnimator.SetTrigger(parameter);
    }
    public void HideNpcCharacter()
    {
        CharacterAnimator.SetTrigger("Idle");
        this.gameObject.transform.DORotate(new Vector3(0,0,0), 0.2f) // 2~3초 동안 이동
          .SetEase(Ease.InOutSine);
    }
   
}
