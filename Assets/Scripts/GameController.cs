using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public GameObject car;

    public List<Transform> points;
    
    public List<Vector3> path;

    public float speed;

    private Tween twn;

    private void OnEnable()
    {
        DialogueManager.OnVisibleDialog += Pause;

        DialogueManager.OnStopCar += Stop;
        
        UIGameOverController.OnGoNext += Next;
        
        UIGameOverController.OnStopCar += Pause;
    }

    private void OnDisable()
    {
        DialogueManager.OnVisibleDialog -= Pause;
        
        DialogueManager.OnStopCar -= Stop;
        
        UIGameOverController.OnGoNext -= Next;
        
        UIGameOverController.OnStopCar -= Pause;
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
        twn = car.transform.DOPath(path.ToArray(), speed, PathType.Linear, PathMode.Full3D)
            .SetLoops(-1)
            .SetEase(Ease.Linear)
            .OnWaypointChange(Callback);
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
    
    private void Stop(bool value)
    {
            if (twn.IsPlaying())
            {
                twn.Pause();
                
                DOVirtual.DelayedCall(1, () =>
                {
                    if (!twn.IsPlaying())
                        twn.Play();
                });
            }
    }

    private void Pause(bool value)
    {
        Debug.Log("Payse");
        if(!value)
            if (twn.IsPlaying())
            {
                twn.Pause();
            }
    }
}
