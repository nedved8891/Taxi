using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
	public static event Action<bool> OnReceivedAnswer;
	
	public static event Action<bool> OnVisibleDialog;
	
	public static event Action<int> OnShowStatusAnswer;
	
	public static event Action<int> OnSelectAnswer;
	public static event Action<int> OnClearSelectAnswer;
	
	public static event Action<bool> OnStopCar;
	
	public ScrollRect scrollRect;
	public ButtonComponent[] buttonText; // первый элемент списка, всегда будет использоваться для вывода текста NPC, остальные элементы для ответов, соответственно, общее их количество должно быть достаточным
	public string folder = "Taxi"; // подпапка в Resources, для чтения
	public int offset = 20;

	private string fileName, lastName;
	private List<Dialogue> node;
	private Dialogue dialogue;
	private Answer answer;
	private float curY, height;
	public static DialogueManager _internal;
	private int id;
	private static bool _active;

	public void DialogueStart(string name)
	{
		if(name == string.Empty) return;
		
		OnVisibleDialog?.Invoke(true);
		
		OnStopCar?.Invoke(true);

		DOVirtual.DelayedCall(1, () =>
		{
			scrollRect.gameObject.SetActive(true);
		
			fileName = name;
		
			Load();
		});
	}

	public static DialogueManager Internal
	{
		get{ return _internal; }
	}

	public static bool isActive
	{
		get{ return _active; }
	}

	private void Awake()
	{
		_internal = this;
		
		//CloseWindow();
	}

	private void Load()
	{
		if(lastName == fileName) // проверка, чтобы не загружать уже загруженный файл
		{
			BuildDialogue(0);
			return;
		}

		node = new List<Dialogue>();

		try // чтение элементов XML и загрузка значений атрибутов в массивы
		{
			TextAsset binary = Resources.Load<TextAsset>(folder + "/" + fileName);
			XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));

			int index = 0;
			while(reader.Read())
			{
				if(reader.IsStartElement("node"))
				{
					dialogue = new Dialogue();
					dialogue.answer = new List<Answer>();
					dialogue.npcText = reader.GetAttribute("npcText");
					dialogue.id = GetINT(reader.GetAttribute("id"));
					node.Add(dialogue);

					XmlReader inner = reader.ReadSubtree();
					while(inner.ReadToFollowing("answer"))
					{
						answer = new Answer();
						answer.text = reader.GetAttribute("text");
						answer.toNode = GetINT(reader.GetAttribute("toNode"));
						answer.exit = GetBOOL(reader.GetAttribute("exit"));
						answer.status = GetBOOL(reader.GetAttribute("status"));
						answer.questStatus = GetINT(reader.GetAttribute("questStatus"));
						answer.questValue = GetINT(reader.GetAttribute("questValue"));
						answer.questValueGreater = GetINT(reader.GetAttribute("questValueGreater"));
						answer.questName = reader.GetAttribute("questName");
						node[index].answer.Add(answer);
					}
					inner.Close();

					index++;
				}
			}

			lastName = fileName;
			reader.Close();
		}
		catch(System.Exception error)
		{
			Debug.Log(this + " ошибка чтения файла диалога: " + fileName + ".xml | Error: " + error.Message);
			CloseWindow();
			lastName = string.Empty;
		}

		BuildDialogue(0);
	}

	private void AddToList(bool exit, int toNode, string text, int questStatus, string questName, bool isActive, bool answerStatus)
	{
		buttonText[id].text.text = text;
		buttonText[id].textMesh.text = text;
		buttonText[id].rect.sizeDelta = new Vector2(buttonText[id].rect.sizeDelta.x, buttonText[id].text.preferredHeight + offset);
		buttonText[id].button.interactable = isActive;
		height = buttonText[id].rect.sizeDelta.y;
		buttonText[id].rect.anchoredPosition = new Vector2(0, -height/2 - curY);

		if(exit)
		{
			SetExitDialogue(buttonText[id].button);
			if(questStatus != 0) SetQuestStatus(buttonText[id].button, questStatus, questName);
		}
		else
		{
			SetNextNode(buttonText[id].button, toNode);
			if(questStatus != 0) SetQuestStatus(buttonText[id].button, questStatus, questName);
		}

		if(isActive)
			SetAnswerStatus(buttonText[id].button, answerStatus);

		id++;

		curY += height + offset;
		RectContent();
	}

	private void RectContent()
	{
		scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, curY);
		scrollRect.content.anchoredPosition = Vector2.zero;
	}

	private void ClearDialogue()
	{
		id = 0;
		curY = offset;
		foreach(ButtonComponent b in buttonText)
		{
			b.text.text = string.Empty;
			b.rect.sizeDelta = new Vector2(b.rect.sizeDelta.x, 0);
			b.rect.anchoredPosition = new Vector2(b.rect.anchoredPosition.x, 0);
			b.button.onClick.RemoveAllListeners();
		}
		RectContent();
	}

	private void SetQuestStatus(Button button, int i, string name) // событие, для управлением статуса, текущего квеста
	{
		string t = name + "|" + i; // склейка имени квеста и значения, которое ему назначено
		button.onClick.AddListener(() => QuestStatus(t));
	}

	private void SetNextNode(Button button, int i) // событие, для перенаправления на другой узел диалога
	{
		button.onClick.AddListener(() => NextBuildDialogue(i, button.gameObject.GetInstanceID()));
	}

	private void SetExitDialogue(Button button) // событие, для выхода из диалога
	{
		button.onClick.AddListener(() => CloseWindow(button.gameObject.GetInstanceID()));
	}

	private void SetAnswerStatus(Button button, bool value) // событие, для управлением статуса, текущего квеста
	{
		button.onClick.AddListener(() => AnswerStatus(value));

		button.gameObject.GetComponent<ButtonComponent>().status = value;
	}

	private void AnswerStatus(bool value)
	{
		OnReceivedAnswer?.Invoke(value);
	}

	private void QuestStatus(string s) // меняем статус квеста
	{
		string[] t = s.Split(new char[]{'|'});

		if(t[1] == "1")
		{
			QuestManager.SetQuestStatus(t[0], QuestManager.Status.Active);
		}
		else if(t[1] == "2")
		{
			QuestManager.SetQuestStatus(t[0], QuestManager.Status.Disable);
		}
		else if(t[1] == "3")
		{
			QuestManager.SetQuestStatus(t[0], QuestManager.Status.Complete);
		}
	}

	private void CloseWindow(int id = -1) // закрываем окно диалога
	{
		_active = false;
		
		if(id != -1)
			OnSelectAnswer?.Invoke(id);
		
		DOVirtual.DelayedCall(4f, () =>
		{
			OnClearSelectAnswer?.Invoke(id);
			
			scrollRect.gameObject.SetActive(false);
			
			//OnVisibleDialog?.Invoke(false);
		});
	}

	private void ShowWindow() // показываем окно диалога
	{
		scrollRect.gameObject.SetActive(true);
		_active = true;
	}

	private int FindNodeByID(int i)
	{
		int j = 0;
		foreach(Dialogue d in node)
		{
			if(d.id == i) return j;
			j++;
		}

		return -1;
	}

	private void NextBuildDialogue(int current, int id)
	{
		DOVirtual.DelayedCall(3.5f, () =>
		{
			DOVirtual.DelayedCall(1.5f, () =>
			{
				BuildDialogue(current);

				OnClearSelectAnswer?.Invoke(id);
			});
		}).OnStart(() =>
		{
			OnShowStatusAnswer?.Invoke(id);
			
			OnSelectAnswer?.Invoke(id);
		});
	}

	private void BuildDialogue(int current)
	{
		ClearDialogue();

		var j = FindNodeByID(current);

		if(j < 0)
		{
			Debug.LogError(this + " в диалоге [" + fileName + ".xml] отсутствует или указан неверно идентификатор узла.");
			return;
		}

		AddToList(false, 0, node[j].npcText, 0, string.Empty, false, false); // добавление текста NPC

		for(var i = 0; i < node[j].answer.Count; i++)
		{
			var value = QuestManager.GetCurrentValue(node[j].answer[i].questName);
			// фильтр ответов, относительно текущего статуса квеста
			if(value >= node[j].answer[i].questValueGreater && node[j].answer[i].questValueGreater != 0 || 
			   node[j].answer[i].questValue == value && node[j].answer[i].questValueGreater == 0 || 
			   node[j].answer[i].questName == null)
			{
				AddToList(node[j].answer[i].exit, node[j].answer[i].toNode, node[j].answer[i].text, node[j].answer[i].questStatus, node[j].answer[i].questName, true, node[j].answer[i].status); // текст игрока
			}
		}

		EventSystem.current.SetSelectedGameObject(scrollRect.gameObject); // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
		ShowWindow();
	}

	private int GetINT(string text)
	{
		if(int.TryParse(text, out var value))
		{
			return value;
		}
		return 0;
	}

	private bool GetBOOL(string text)
	{
		bool value;
		if(bool.TryParse(text, out value))
		{
			return value;
		}
		return false;
	}
}

internal class Dialogue
{
	public int id;
	public string npcText;
	public List<Answer> answer;
}


internal class Answer
{
	public string text, questName;
	public int toNode, questValue, questValueGreater, questStatus;
	public bool exit;
	public bool status;
}