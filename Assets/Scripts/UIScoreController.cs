using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreController : MonoBehaviour
{
    /// <summary>
    /// Пле для отображения
    /// </summary>
    protected Text _score;
    
    /// <summary>
    /// Переменная очков
    /// </summary>
    protected int newScore = 0;

    /// <summary>
    /// Промежуточная переменная
    /// </summary>
    protected int score = 0;
        
    private void OnEnable()
    {
        UIGameOverController.OnChangeScore += Change;
    }

    private void OnDisable()
    {
        UIGameOverController.OnChangeScore -= Change;
    }
    
    protected void Awake()
    {
        _score = GetComponent<Text>();
    }

    private void Change()
    {
        var value = SliderController.currentResult == TResults.Perfect ? 100 :
            SliderController.currentResult == TResults.Good ? 50 :
            SliderController.currentResult == TResults.Good ? 10 : 0;
        
        PlayerPrefs.SetInt("Dollars", PlayerPrefs.GetInt("Dollars", 0) + value);
        PlayerPrefs.Save();
        
        DOVirtual.Float(0, value, 0.5f, f =>
        {
            score = (int)f;
            _score.text = "+" + score.ToString() + "$";
        }).OnStart(Started).OnComplete(Completed);
    }
  
    public virtual void Completed()
    {
       
    }
    
    public virtual void Started()
    {
        
    }
}
