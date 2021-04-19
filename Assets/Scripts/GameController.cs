using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static event Action<float> OnCarStoped;
    
    public static event Action<float> OnCarResumed;
    
    public static event Action<float> OnGameOver;
    
    public static event Action<float> OnCloseDialog;
    
    public static event Action OnRestart;
    
    [Header("Автомобіль")]
    public GameObject car;

    [Header("Точки з яких буде складений шлях")]
    public List<Transform> points;

    [Header("Точки спавна")]
    public List<Transform> spawnPoints;
    
    [Header("Точки шляху")]
    public List<Vector3> path;

    [Header("Швидксть авто")]
    public float speed;

    private Tween twn;

    private void OnEnable()
    {
        CameraController.OnResumeMoveCar += Resume;
        
        PassengerController.OnOutCar += OutCar;

        UIGameOverController.OnRestart += Restart;
    }

    private void OnDisable()
    {
        CameraController.OnResumeMoveCar -= Resume;
        
        PassengerController.OnOutCar -= OutCar;
        
        UIGameOverController.OnRestart -= Restart;
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
        Spawn();
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
    
    private void OutCar(float delay)
    {
        //OnGameOver?.Invoke(delay);

        Move(2);
    }

    private void Restart()
    {
        OnRestart?.Invoke();

        Spawn();
    }

    private void Spawn()
    {
        Debug.Log("Spawn");
        
        car.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
    }

    private void Move(float duration)
    {
        twn = car.transform.DOPath(path.ToArray(), speed, PathType.Linear, PathMode.Full3D)
            .OnStart(() =>
            {
                Stop(duration);
            })
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
