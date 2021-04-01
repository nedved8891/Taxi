using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CarAnimations
{
    Idle,
    IdleOpenBackRightDoor,
    CloseDoor,
    OpenDoor
}

public class CarController : MonoBehaviour
{
    private Animator _animator;

    private void OnEnable()
    {
        PassengerController.OnOpenDoor += OpenDoor;
        
        PassengerController.OnCloseDoor += CloseDoor;
    }

    private void OnDisable()
    {
        PassengerController.OnOpenDoor -= OpenDoor;
        
        PassengerController.OnCloseDoor -= CloseDoor;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void OpenDoor()
    {
        _animator.SetTrigger(CarAnimations.OpenDoor.ToString());
    }
    
    private void CloseDoor()
    {
        _animator.SetTrigger(CarAnimations.CloseDoor.ToString());
    }
}
