using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UISmilesController1 : MonoBehaviour
{
   public static event Action<bool> OnChangeSlider;
   
   public List<Transform> smiles;

   public List<Sprite> sprites;
   
   private Sprite type;
   
   private bool isGameOver;
   
   public void Init(bool value, bool isGood)
   {
      isGameOver = isGood;
      
      type = value? sprites[0] : sprites[1];

      StartCoroutine(ExampleCoroutine());
   }

   private IEnumerator ExampleCoroutine()
   {
      yield return new WaitForSeconds(.5f);
   
      foreach (var smile in smiles)
      {
         smile.DOLocalMoveY(5, 1f)
            .OnStart(() =>
            {
               smile.DOLocalMoveX(Random.Range(-2, 2), 1f);
               
               smile.gameObject.SetActive(true);

               var img = smile.GetComponent<SpriteRenderer>();
               
               img.sprite = type;

               var color1 = img.color;
               var color = new Color(color1.r, color1.g, color1.b, 1);
               color1 = color;
               img.color = color1;

               img.DOFade(0f, 1).SetEase(Ease.OutQuad).OnComplete(() =>
               {
                  smile.localPosition = Vector3.zero;
               });
            });
         
         yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
      }
      
      DOVirtual.DelayedCall(2.5f, () =>
      {
         if(isGameOver)
                OnChangeSlider?.Invoke(isGameOver);
         //Destroy(gameObject);
      });
   }
}
