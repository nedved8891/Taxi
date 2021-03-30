using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using DG.Tweening;
using TMPro;

public class ButtonComponent : MonoBehaviour
{
	public Button button;
	public Text text;
	public TextMeshProUGUI textMesh;
	public RectTransform rect;

	public AnswerStatus status;

	public Image img;
	
	public Color colorGood;

	public Color colorBad;
	
	public Color colorNone = Color.yellow;

	private Color defaultColor;
	
	private void Awake()
	{
		img = GetComponent<Image>();
	}

	private void OnEnable()
	{
		DialogueManager.OnSelectAnswer += ChangeColor;
		
		DialogueManager.OnClearSelectAnswer += ClearSelect;
	}
	
	private void OnDisable()
	{
		DialogueManager.OnSelectAnswer -= ChangeColor;
		
		DialogueManager.OnClearSelectAnswer -= ClearSelect;
	}

	private void ChangeColor(int id)
	{
		if(id != gameObject.GetInstanceID())
			return;
		
		if (button.interactable)
		{
			defaultColor = img.color;
			img.color = status == AnswerStatus.None? colorNone : status == AnswerStatus.Good ? colorGood : colorBad;
		}
	}

	private void ClearSelect(int id)
	{
		if(id != gameObject.GetInstanceID())
			return;
		
		if (button.interactable)
		{
			img.color = defaultColor;
		}
		
	}
}
