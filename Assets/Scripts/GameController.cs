using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static event Action<float> OnCarStoped;
    
    public static event Action OnCarMoved;
    
    public GameObject car;

    public List<Transform> points;
    
    public List<Vector3> path;

    public float speed;

    private Tween twn;

    private void OnEnable()
    {
        DialogueManager.OnVisibleDialog += Pause;

        BttnFreeController.OnStopCar += Stop;
        
        UIGameOverController.OnGoNext += Next;
        
        UIGameOverController.OnStopCar += Pause;
        
        DialogueManager.OnResumeMoveCar += Resume;
        
        DialogueManager.OnPauseCar += Pause;
        
        CameraController.OnResumeMoveCar += Resume;
    }

    private void OnDisable()
    {
        DialogueManager.OnVisibleDialog -= Pause;
        
        BttnFreeController.OnStopCar -= Stop;
        
        UIGameOverController.OnGoNext -= Next;
        
        UIGameOverController.OnStopCar -= Pause;
        
        DialogueManager.OnResumeMoveCar -= Resume;
        
        DialogueManager.OnPauseCar -= Pause;
        
        CameraController.OnResumeMoveCar -= Resume;
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
    
    private void Stop(int delay)
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
            
            OnCarMoved?.Invoke();
        }
    }
    
    private void Pause(float delay)
    {
        Stop(1);
        
        //DOVirtual.DelayedCall(1 + delay, Resume);
    }

    private void Pause(bool value)
    {
        DOVirtual.DelayedCall(0, () =>
        {
            if(!value)
                if (twn.IsPlaying())
                    twn.Pause();
        });
    }
}
