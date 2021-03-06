using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public enum TPoints
{
   None,
   Movement,
   Dialog,
   BoardingPassengerInCar,
   BoardingPassengerOutCar
}

public class CameraController : MonoBehaviour
{
   public static event Action<int> OnStartDialog;
   
   public static event Action OnResumeMoveCar;
   
   public static event Action OnPassengerSpawn;
   
   public static event Action OnPassengerGoInCar;
   
   public static event Action<float> OnBoardingPassengerOutCar;

   public float speed = 0.5f;
   
   public Transform startPoint;

   public Transform dialogPoint;

   public Transform endPoint;

   private Camera _camera;

   private Transform _transform;

   private Animator _animator;

   private void OnEnable()
   {
      GameController.OnCarStoped += StopCar;

      GameController.OnCloseDialog += CloseDialog;

      UIGameOverController.OnGoNext += Restart;

      PassengerController.OnMoveCar += BoardingPassengerInCar;
   }

   private void OnDisable()
   {
      GameController.OnCarStoped -= StopCar;

      GameController.OnCloseDialog -= CloseDialog;
      
      UIGameOverController.OnGoNext -= Restart;

      PassengerController.OnMoveCar -= BoardingPassengerInCar;
   }

   private void Awake()
   {
      _camera = GetComponent<Camera>();
      
      _animator = GetComponent<Animator>();

      _transform = _camera.gameObject.transform;
   }

   private void BoardingPassengerInCar()
   {
      Change(TPoints.Dialog);
   }

   private void Restart()
   {
      Change(TPoints.Movement);
   }

   private void StopCar(float delay)
   {
      DOVirtual.DelayedCall(delay + 1, () =>
      {
         Change(TPoints.BoardingPassengerInCar);
      });
   }
   
   private void CloseDialog(float delay)
   {
      DOVirtual.DelayedCall(delay + 1, () =>
      {
         Change(TPoints.BoardingPassengerOutCar);
      });
   }

   private void Change(TPoints tpoint)
   {
      Debug.Log(tpoint);
      switch (tpoint)
      {
         case TPoints.Movement:
            _animator.SetTrigger("Move");
            //_transform.DOMove(startPoint.position, speed).OnStart(() =>
            //{
            //   _transform.DORotate(startPoint.eulerAngles, speed);
            //}).OnComplete(() =>
            //{
            //   OnResumeMoveCar?.Invoke();
            //});
            break;
         case TPoints.Dialog:
            _animator.SetTrigger("Dialog");
            //_transform.DOMove(dialogPoint.position, speed).OnStart(() =>
            //{
            //   _transform.DORotate(dialogPoint.eulerAngles, speed);
            //}).OnComplete(() =>
            //{
            //   OnStartDialog?.Invoke(0);
            //});
            break;
         case TPoints.BoardingPassengerInCar:
            Debug.Log("Passenger");
            _animator.SetTrigger("Passenger");
            //_transform.DOMove(endPoint.position, speed)
             //  .OnStart(() =>
            //   {
                  OnPassengerSpawn?.Invoke();
                  
             //     _transform.DORotate(endPoint.eulerAngles, speed);
              // }).OnComplete(() =>
             //  {
              //    OnPassengerGoInCar?.Invoke();
              // });
            break;
         case TPoints.BoardingPassengerOutCar:
            _animator.SetTrigger("Out");
            //_transform.DOMove(endPoint.position, speed).OnStart(() =>
            //{
            //   _transform.DORotate(endPoint.eulerAngles, speed);
            //}).OnComplete(() =>
            //{
            //   OnBoardingPassengerOutCar?.Invoke(1);
            //});
            break;
      }
   }
   
   public void EventMovement()
   {
      Debug.Log("EventMovement");
      OnResumeMoveCar?.Invoke();
   }
   
   public void EventDialog()
   {
      Debug.Log("EventDialog");
      OnStartDialog?.Invoke(0);
   }

   public void EventBoardingPassengerInCar()
   {
      Debug.Log("EventBoardingPassengerInCar");
      OnPassengerGoInCar?.Invoke();
   }
   
   public void EventBoardingPassengerOutCar()
   {
      Debug.Log("EventBoardingPassengerOutCar");
      OnBoardingPassengerOutCar?.Invoke(1);
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.A))
      {
         Change(TPoints.Movement);
      }
      if (Input.GetKeyDown(KeyCode.D))
      {
         Change(TPoints.Dialog);
      }
      if (Input.GetKeyDown(KeyCode.S))
      {
         Change(TPoints.BoardingPassengerInCar);
      }
   }
}
