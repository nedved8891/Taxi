using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyContriller : MonoBehaviour
{
   public Text txt;

   private void OnEnable()
   {
      UIGameOverController.OnGoNext += Change;
   }

   private void OnDisable()
   {
      UIGameOverController.OnGoNext -= Change;
   }

   private void Change()
   {
      txt.text = PlayerPrefs.GetInt("Dollars", 100).ToString();
   }
}
