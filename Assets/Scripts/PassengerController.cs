using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public enum PassengerAnimations
{
    Angry,
    ExitCar,
    OpenDoor
}

public class PassengerController : MonoBehaviour
{
    public static event Action OnOpenDoor;
    
    public static event Action OnCloseDoor;
    
    public static event Action OnMoveCar;
    
    public static event Action<float> OnOutCar;
    
    private Animator _animator;
    
    private void OnEnable()
    {
        CameraController.OnBoardingPassengerOutCar += OutCar;
    }

    private void OnDisable()
    {
        CameraController.OnBoardingPassengerOutCar -= OutCar;
    }
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetAnimation(PassengerAnimations.Angry);
    }

    private void OutCar(float delay)
    {
        DOVirtual.DelayedCall(delay, ()=>
        {
            SetAnimation(PassengerAnimations.ExitCar);
        });
    }

    private void SetAnimation(PassengerAnimations animation)
    {
        _animator.Play(animation.ToString());
    }

    public void EventOpenDoor()
    {
        OnOpenDoor?.Invoke();
    }
    
    public void EventCloseDoor()
    {
        OnCloseDoor?.Invoke();
    }
    
    public void EventInCar()
    {
        OnMoveCar?.Invoke();
    }
    
    public void EventOutCar()
    {
        OnOutCar?.Invoke(0);
        
        Destroy(gameObject);
    }
}
