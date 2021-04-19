using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIGameOverController : MonoBehaviour
{
    public static event Action OnRestart;
    
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
        DOVirtual.DelayedCall(delay, () =>
        {
            fade.SetActive(true);
            complete.SetActive(PlayerPrefs.GetInt("DialogWin") == 1);
            faile.SetActive(PlayerPrefs.GetInt("DialogWin") == 0);
            
            OnChangeScore?.Invoke();
        });
    }
    
    private void Hide()
    {
        fade.SetActive(false);
        complete.SetActive(false);
        faile.SetActive(false);
    }

    public void Restart()
    {
        Hide();
        
        OnRestart?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Show(0);
        }
    }
}

