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
    }

    private void OnDisable()
    {
    }
    
    private void Awake()
    {
        panel = GetComponent<Image>();
    }

    private void Change()
    {
        panel.sprite = sprites[(int) SliderController.currentResult];
    }
}
