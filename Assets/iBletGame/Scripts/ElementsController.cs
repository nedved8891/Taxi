using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ElementsController : MonoBehaviour
{
   public bool isGood;

   public bool isExit;

   public void Move(Vector3 pos)
   {
      transform.DOJump(pos, 1, 6).SetEase(Ease.Linear)
         .OnStart(() =>
         {
            DOVirtual.DelayedCall(4, () =>
            {
               Destroy(gameObject);
            });
         })
         .OnComplete(() =>
         {
            Destroy(gameObject);
         });
   }

   private void Update()
   {
      if (isGood)
      {
         transform.Rotate(new Vector3(-300, 50,100) * Time.deltaTime);
      }
      else
      {
         transform.Rotate(new Vector3(100, 50,-300) * Time.deltaTime);
      }
   }
}
