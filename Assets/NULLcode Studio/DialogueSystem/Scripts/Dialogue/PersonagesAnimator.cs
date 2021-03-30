using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PersonagesAnimator : MonoBehaviour
{
   public static event Action<AnswerStatus> OnPassengerReaction;
   
   private Animator _animator;

   private AnswerStatus reaction;
   
   private void OnEnable()
   {
      DialogueManager.OnSelectAnswer += SetAnimation;
      
      DialogueManager.OnReceivedAnswer += Change;
   }

   private void OnDisable()
   {
      DialogueManager.OnSelectAnswer -= SetAnimation;
      
      DialogueManager.OnReceivedAnswer += Change;
   }

   private void Awake()
   {
      _animator = GetComponent<Animator>();
   }

   private void SetAnimation(int value)
   {
      _animator.SetTrigger("Rotate");

      DOVirtual.DelayedCall(1.5f, () =>
      {
         OnPassengerReaction?.Invoke(reaction);
      });
   }

   private void Change(AnswerStatus value)
   {
      reaction = value;
   }
}
