using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurrentScore : MonoBehaviour
{
   public Text score;

   public Text scoreCurrnet;

   private void Start()
   {
      score.text = scoreCurrnet.text;
   }
}
