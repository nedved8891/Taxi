using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICompleteController : MonoBehaviour
{
    public List<Sprite> sprites;

    private Image panel;
    
    private void OnEnable()
    {
        UIGameOverController.OnChangeScore += Change;
    }

    private void OnDisable()
    {
        UIGameOverController.OnChangeScore -= Change;
    }
    
    private void Awake()
    {
        panel = GetComponent<Image>();
    }

    private void Change()
    {
        Debug.Log("SliderController.currentResult: " + SliderController.currentResult);
        panel.sprite = sprites[(int) SliderController.currentResult];
    }
}
