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
   
   public static event Action<float> OnBoardingPassengerOutCar;

   public float speed = 0.5f;
   
   public Transform startPoint;

   public Transform dialogPoint;

   public Transform endPoint;

   private Camera _camera;

   private Transform _transform;
   
   private void OnEnable()
   {
      GameController.OnCarStoped += StopCar;

      GameController.OnCloseDialog += CloseDialog;

      UIGameOverController.OnGoNext += Restart;
   }

   private void OnDisable()
   {
      GameController.OnCarStoped -= StopCar;

      GameController.OnCloseDialog -= CloseDialog;
      
      UIGameOverController.OnGoNext -= Restart;
   }

   private void Awake()
   {
      _camera = GetComponent<Camera>();

      _transform = _camera.gameObject.transform;
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
      switch (tpoint)
      {
         case TPoints.Movement:
            _transform.DOMove(startPoint.position, speed).OnStart(() =>
            {
               _transform.DORotate(startPoint.eulerAngles, speed);
            }).OnComplete(() =>
            {
               OnResumeMoveCar?.Invoke();
            });
            break;
         case TPoints.Dialog:
            _transform.DOMove(dialogPoint.position, speed).OnStart(() =>
            {
               _transform.DORotate(dialogPoint.eulerAngles, speed);
            }).OnComplete(() =>
            {
               OnStartDialog?.Invoke(0);
            });
            break;
         case TPoints.BoardingPassengerInCar:
            _transform.DOMove(endPoint.position, speed).OnStart(() =>
            {
               _transform.DORotate(endPoint.eulerAngles, speed);
            }).OnComplete(() =>
            {
               Change(TPoints.Dialog); // поки тут
            });
            break;
         case TPoints.BoardingPassengerOutCar:
            _transform.DOMove(endPoint.position, speed).OnStart(() =>
            {
               _transform.DORotate(endPoint.eulerAngles, speed);
            }).OnComplete(() =>
            {
               OnBoardingPassengerOutCar?.Invoke(1);
            });
            break;
      }
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
