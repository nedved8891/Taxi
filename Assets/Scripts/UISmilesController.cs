﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UISmilesController : MonoBehaviour
{
   public static event Action<bool> OnChangeSlider;
   
   public List<Transform> smiles;

   public List<Sprite> sprites;
   
   private Sprite type;
   
   private bool isGoodAnswer;
   
   public void Init(bool value, bool isGood)
   {
      isGoodAnswer = isGood;
      
      type = value? sprites[0] : sprites[1];

      StartCoroutine(ExampleCoroutine());
   }

   private IEnumerator ExampleCoroutine()
   {
      yield return new WaitForSeconds(.5f);
   
      foreach (var smile in smiles)
      {
         smile.DOLocalMoveY(300, 2f)
            .OnStart(() =>
            {
               smile.DOLocalMoveX(Random.Range(-20, 20), 2f);
               
               smile.gameObject.SetActive(true);

               var img = smile.GetComponent<Image>();
               
               img.sprite = type;

               img.DOFade(0f, 2).SetEase(Ease.OutQuad);
            });
         
         yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
      }
      
      OnChangeSlider?.Invoke(isGoodAnswer);
      
      DOVirtual.DelayedCall(2, () =>
      {
         Destroy(gameObject);
      });
   }
}
