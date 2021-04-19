using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChooseActionPanelController : MonoBehaviour
{
    public static event Action<Actions> OnChooseAction;
    
    [Header("Налаштування для кнопок вибору дій")]
    [SerializeField] private ActionsSettings [] _actionsSettingses;
    
    [Header("Список кнопок")]
    [SerializeField] private List<ActionButtonController> _actionButtons;

    private void OnEnable()
    {
        ActionButtonController.OnAction += ActionView;
        
        DialogueManager.OnShowCooseActionPanel += Show;
    }

    private void OnDisable()
    {
        ActionButtonController.OnAction += ActionView;
        
        DialogueManager.OnShowCooseActionPanel -= Show;
    }

    private void Show(List<Actions> actionsList)
    {
        for (var i = 0; i < _actionButtons.Count; i++)
        {
            _actionButtons[i].gameObject.SetActive(true);
            
            _actionButtons[i].Show(_actionsSettingses.First(x => x.type == actionsList[i]), i * 0.2f);
        }
    }

    private void Hide()
    {
        foreach (var t in _actionButtons)
        {
            t.Hide();
        }
    }
    
    private void ActionView ( ActionButtonController actionButton)
    {
        Hide();
        
        OnChooseAction?.Invoke ( actionButton.type );
    }
}

