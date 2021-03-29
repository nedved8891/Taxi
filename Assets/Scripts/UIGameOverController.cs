using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIGameOverController : MonoBehaviour
{
    public static event Action OnGoNext;
    
    public static event Action<bool> OnStopCar;
    
    public GameObject complete;

    public GameObject faile;

    public GameObject fade;
    
    private void OnEnable()
    {
        SliderController.OnGameOver += Show;
    }

    private void OnDisable()
    {
        SliderController.OnGameOver -= Show;
    }

    private void Show(bool value)
    {
        DOVirtual.DelayedCall(0f, () =>
        {
            fade.SetActive(true);
            complete.SetActive(value);
            faile.SetActive(!value);
            
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
        PlayerPrefs.SetInt("Dollars", 300);
        
        Hide();
        
        OnGoNext?.Invoke();
    }
}

