using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DriverController : MonoBehaviour
{
    public static event Action<AnswerStatus> OnPassengerReaction;
   
    private Animator _animator;

    private AnswerStatus reaction;
   
    private void OnEnable()
    {
        DialogueManager.OnSelectAnswer += SetAnimation;
      
        DialogueManager.OnReceivedAnswer += Change;

        GameController.OnCarStoped += Stop;
        
        GameController.OnCarResumed += Drive;

        DialogueManager.OnSelectAnswer += Talking;
    }

    private void OnDisable()
    {
        DialogueManager.OnSelectAnswer -= SetAnimation;
      
        DialogueManager.OnReceivedAnswer += Change;
        
        GameController.OnCarStoped -= Stop;
        
        GameController.OnCarResumed -= Drive;
        
        DialogueManager.OnSelectAnswer += Talking;
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

    private void Stop(float delay)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            _animator.Play("Stoping");
        });
    }

    private void Talking(int value)
    {
        _animator.Play("Talking");
    }
    
    private void Drive(float delay)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            _animator.Play("Driving");
        });
    }

    private void Change(AnswerStatus value)
    {
        reaction = value;
    }
}
