using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonController : MonoBehaviour
{
   public static event Action<ActionButtonController> OnAction;
   
   public Actions type;

   private Image _image;

   private void Awake()
   {
      _image = GetComponent<Image>();
   }

   public void Invoke()
   {
      OnAction?.Invoke ( this );
   }

   public void Show(ActionsSettings settings, float delay)
   {
      type = settings.type;

      _image.sprite = settings.image;
      
      transform.localScale = Vector3.zero;

      DOVirtual.DelayedCall(delay, () =>
      {
         transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear);
      });
   }
   
   public void Hide()
   {
      DOVirtual.DelayedCall(0, () =>
      {
         transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
         {
            gameObject.SetActive(false);
         });
      });
   }
}
