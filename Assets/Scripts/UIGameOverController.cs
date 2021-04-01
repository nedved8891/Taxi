using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIGameOverController : MonoBehaviour
{
    public static event Action OnGoNext;
    
    public static event Action<bool> OnStopCar;
    
    public static event Action OnChangeScore;
    
    public GameObject complete;

    public GameObject faile;

    public GameObject fade;
    
    private void OnEnable()
    {
        GameController.OnGameOver += Show;
    }

    private void OnDisable()
    {
        GameController.OnGameOver -= Show;
    }

    private void Show(float delay)
    {
        var result = (int)SliderController.currentResult;
        
        DOVirtual.DelayedCall(delay, () =>
        {
            fade.SetActive(true);
            complete.SetActive(result >= 0);
            faile.SetActive(result < 0);
            
            OnChangeScore?.Invoke();
            OnStopCar?.Invoke(false);
        });
    }
    
    private void Hide()
    {
        fade.SetActive(false);
        complete.SetActive(false);
        faile.SetActive(false);
    }

    public void GoNext()
    {
        Hide();
        
        OnGoNext?.Invoke();
    }
}

