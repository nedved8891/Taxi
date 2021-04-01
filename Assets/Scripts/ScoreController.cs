using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    private Text _text;
    
    private void OnEnable()
    {
        UIGameOverController.OnGoNext += Change;
    }

    private void OnDisable()
    {
        UIGameOverController.OnGoNext -= Change;
    }

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void Start()
    {
        Change();
    }

    private void Change()
    {
        if(_text)
            _text.text = PlayerPrefs.GetInt("Dollars", 0).ToString();
    }
}
