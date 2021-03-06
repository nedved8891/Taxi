using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static event Action<float> OnCarStoped;
    
    public static event Action<float> OnCarResumed;
    
    public static event Action<float> OnGameOver;
    
    public static event Action<float> OnCloseDialog;
    
    [Header("Автомобіль")]
    public GameObject car;

    [Header("Точки з яких буде складений шлях")]
    public List<Transform> points;
    
    [Header("Точки шляху")]
    public List<Vector3> path;

    [Header("Швидксть авто")]
    public float speed;

    private Tween twn;

    private void OnEnable()
    {
        DialogueManager.OnVisibleDialog += Pause;
        
        DialogueManager.OnResumeMoveCar += Resume;
        
        DialogueManager.OnPauseCar += Stop;
        
        DialogueManager.OnCompleteDialog += CompleteDialog;
        
        UIGameOverController.OnStopCar += Stop;
        
        CameraController.OnResumeMoveCar += Resume;
        
        PassengerController.OnOutCar += GameOver;
        
        BttnFreeController.OnStopCar += Stop;
    }

    private void OnDisable()
    {
        DialogueManager.OnVisibleDialog -= Pause;
        
        DialogueManager.OnResumeMoveCar -= Resume;
        
        DialogueManager.OnPauseCar -= Stop;
        
        DialogueManager.OnCompleteDialog -= CompleteDialog;
        
        UIGameOverController.OnStopCar -= Stop;
        
        CameraController.OnResumeMoveCar -= Resume;
        
        PassengerController.OnOutCar -= GameOver;
        
        BttnFreeController.OnStopCar -= Stop;
    }

    private void Awake()
    {
        foreach (var point in points)
        {
            path.Add(point.position);
        }
    }

    private void Start()
    {
        Move();
    }

    private void Callback(int waypointIndex)
    {
        car.transform.DOLocalRotate(points[waypointIndex].localRotation.eulerAngles, 0.7f)
            .SetEase(Ease.Linear);
    }

    private void CompleteDialog()
    {
        //OnGameOver?.Invoke(0.75f);

        if (twn.IsPlaying())
        {
            twn.Pause();
        
            OnCloseDialog?.Invoke(0.75f);
        }
    } 
    
    private void GameOver(float delay)
    {
        OnGameOver?.Invoke(delay);
    }

    private void Next()
    {
        DOVirtual.DelayedCall(0, () =>
        {
            if (!twn.IsPlaying())
                twn.Play();
        });
    }

    private void Move()
    {
        twn = car.transform.DOPath(path.ToArray(), speed, PathType.Linear, PathMode.Full3D)
            .SetLoops(-1)
            .SetEase(Ease.Linear)
            .OnWaypointChange(Callback);
    }
    
    private void Stop(float delay)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            if (twn.IsPlaying())
            {
                twn.Pause();
            
                OnCarStoped?.Invoke(0.5f);
            }
        });
    }

    private void Resume()
    {
        if (!twn.IsPlaying())
        {
            twn.Play();
            
            OnCarResumed?.Invoke(0.5f);
        }
    }
    
    private void Pause(bool value)
    {
        DOVirtual.DelayedCall(0, () =>
        {
            if(!value)
                if (twn.IsPlaying())
                {
                    twn.Pause();
                    
                    OnCarStoped?.Invoke(0.5f);
                }
        });
    }
}
