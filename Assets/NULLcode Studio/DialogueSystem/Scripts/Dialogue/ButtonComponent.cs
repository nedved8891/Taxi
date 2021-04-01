using System;
using UnityEngine;
using UnityEngine.UI;
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
		
		defaultColor = img.color;
	}

	private void OnEnable()
	{
		DialogueManager.OnSelectAnswer += ChangeColor;
	}
	
	private void OnDisable()
	{
		DialogueManager.OnSelectAnswer -= ChangeColor;
	}

	private void ChangeColor(int id)
	{
		if(button.interactable)
			button.interactable = false;
		
		if(id != gameObject.GetInstanceID())
			return;
		
		img.color = status == AnswerStatus.None? colorNone : status == AnswerStatus.Good ? colorGood : colorBad;
		
	}

	public void ClearSelect(bool value)
	{
		button.interactable = value;
		
		img.color = defaultColor;
	}
}
