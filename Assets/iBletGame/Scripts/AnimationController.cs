using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _animator;

    private string reation;
    
    private void OnEnable()
    {
        PersonageController.ChangeMouseState += Rot;
        
        AddScore.OnReation += Reation;
    }

    private void Reation(bool obj)
    {
        reation = obj ? "Smile" : "Sad";
    }

    private void OnDisable()
    {
        PersonageController.ChangeMouseState -= Rot;
        
        AddScore.OnReation -= Reation;
    }
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Rot(bool value)
    {
        if (value)
        {
            //_animator.SetTrigger("Open");

            _animator.Play("OpenMouse");
        }
        else
        {
            //_animator.SetTrigger("Close");
            
            _animator.Play("CloseMouse");
        }
    }

    private void SetAnimation(int value)
    {
        _animator.SetTrigger("Open");
    }

    public void EClose()
    {
        _animator.Play(reation);

        GetComponentInParent<PersonageController>().isOpenMouse = false;
    }
}
