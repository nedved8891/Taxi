using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangePersonage : MonoBehaviour
{
   public Image ico;

   public Sprite girl;

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
      ico.sprite = girl;
   }
}
