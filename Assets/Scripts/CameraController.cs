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
   BoardingPassenger
}

public class CameraController : MonoBehaviour
{
   public static event Action<int> OnStartDialog;
   
   public static event Action OnResumeMoveCar;
   
   public Transform startPoint;

   public Transform dialogPoint;

   public Transform endPoint;

   private Camera _camera;

   private Transform _transform;
   
   private void OnEnable()
   {
      GameController.OnCarStoped += StopCar;
   }

   private void OnDisable()
   {
      GameController.OnCarStoped -= StopCar;
   }

   private void Awake()
   {
      _camera = GetComponent<Camera>();

      _transform = _camera.gameObject.transform;
   }

   private void StopCar(float delay)
   {
      DOVirtual.DelayedCall(delay + 1, () =>
      {
         Change(TPoints.BoardingPassenger);
      });
   }

   private void Change(TPoints tpoint)
   {
      switch (tpoint)
      {
         case TPoints.Movement:
            _transform.DOMove(startPoint.position, 1).OnStart(() =>
            {
               _transform.DORotate(startPoint.eulerAngles, 1);
            }).OnComplete(() =>
            {
               OnResumeMoveCar?.Invoke();
            });
            break;
         case TPoints.Dialog:
            _transform.DOMove(dialogPoint.position, 1).OnStart(() =>
            {
               _transform.DORotate(dialogPoint.eulerAngles, 1);
            }).OnComplete(() =>
            {
               OnStartDialog?.Invoke(0);
            });
            break;
         case TPoints.BoardingPassenger:
            _transform.DOMove(endPoint.position, 1).OnStart(() =>
            {
               _transform.DORotate(endPoint.eulerAngles, 1);
            });;
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
         Change(TPoints.BoardingPassenger);
      }
   }
}
