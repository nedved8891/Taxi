using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public static event Action<bool> OnGameOver;
    
    [Header("На скільки змінюється")]
    public float offset = 0.25f;

    [Header("Курсор")]
    public Image handle;
    
    [Header("Іконки")]
    public List<Sprite> icons;

    [Header("Слайдер")]
    public Slider _slider;

    public static TResults currentResult;
    
    private void OnEnable()
    {
        UISmilesController.OnChangeSlider += Change;

        DialogueManager.OnVisibleDialog += Activate;
        
        UIGameOverController.OnGoNext += Restart;
        
        GameController.OnGameOver += Hide;
    }

    private void OnDisable()
    {
        UISmilesController.OnChangeSlider -= Change;
        
        DialogueManager.OnVisibleDialog -= Activate;
        
        UIGameOverController.OnGoNext += Restart;
        
        GameController.OnGameOver -= Hide;
    }
    
    private void Restart()
    {
        _slider.value = .5f;
        
        CheckHandle();
        
        _slider.gameObject.SetActive(false);
    }
    
    private void Awake()
    {
        CheckHandle();
    }

    private void Hide(float delay)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            _slider.gameObject.SetActive(false);
        });
    }

    private void Activate(bool value)
    {
        DOVirtual.DelayedCall(0, () =>
        {
            _slider.gameObject.SetActive(value);
        });
    }

    private void Change(AnswerStatus value)
    {
        if(value == AnswerStatus.None)
            return;
        
        _slider.DOValue(_slider.value + offset * (value == AnswerStatus.Good? -1 : 1), 0.3f)
            .OnComplete(()=>
            {
                var index = CheckHandle();
                
                if (_slider.value == 0 || _slider.value == 1)
                {
                    DOVirtual.DelayedCall(0.0f, () =>
                    {
                        //OnGameOver?.Invoke(_slider.value == 0);
                    });
                }
            });
    }

    private int CheckHandle()
    {
        var indx = 0;
        currentResult = TResults.Perfect;
        switch (_slider.value)
        {
            case 0.25f:
                indx = 1;
                break;
            case 0.5f:
                indx = 2;
                break;
            case 0.75f:
                indx = 3;
                break;
            case 1f:
                indx = 4;
                break;
        }
        
        handle.sprite = icons[indx];

        currentResult = (TResults)indx;

        return indx;
    }
}

[System.Serializable]
public enum TResults
{
    Perfect = 0,
    Good = 1,
    Normal = 2,
    Bad = 3,
    Poor = 4,
}
