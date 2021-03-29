using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreController : MonoBehaviour
{
   private Text _text;

   public int score;

   public GameObject panel;

   private void OnEnable()
   {
      AddScore.OnAddScore += Change;
      
      UIGameOverController1.OnGoNext += UIGameOverController1OnOnGoNext;
      
      UISmilesController1.OnChangeSlider += UISmilesController1OnOnChangeSlider;
   }

   private void UISmilesController1OnOnChangeSlider(bool obj)
   {
      panel.SetActive(false);
   }

   private void UIGameOverController1OnOnGoNext()
   {
      score = 0;
      
      Change();
   }

   private void OnDisable()
   {
      AddScore.OnAddScore -= Change;
      
      UIGameOverController1.OnGoNext -= UIGameOverController1OnOnGoNext;
      
      UISmilesController1.OnChangeSlider -= UISmilesController1OnOnChangeSlider;
   }

   private void Awake()
   {
      _text = GetComponentInChildren<Text>();
   }

   private void Start()
   {
      score = 0;
      
      Change();
   }

   private void Change(int value = 0)
   {
      Debug.Log("333  " +value);
      score += value;

      if (score < 0)
         score = 0;

      UIChange();
   }

   private void UIChange()
   {
      _text.text = "SCORE: " + score.ToString();
   }
}
