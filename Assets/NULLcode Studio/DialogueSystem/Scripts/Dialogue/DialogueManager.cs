using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using DG.Tweening;

[System.Serializable]
public enum NDialogs
{
	None = -1,
	Dialog_1 = 0,
	Dialog_2 = 1,
	Dialog_3 = 2,
	Dialog_4 = 3,
}

public class DialogueManager : MonoBehaviour
{
	public static event Action<AnswerStatus> OnReceivedAnswer;
	
	public static event Action<bool> OnVisibleDialog;
	
	public static event Action OnCompleteDialog;
	
	public static event Action<int> OnSelectAnswer;
	
	public static event Action<int> OnClearSelectAnswer;//Повертаєм колір після показу правильності відповіді
	
	public static event Action OnResumeMoveCar;
	
	public static event Action<float> OnPauseCar;
	
	[Header("Діалог")]
	public NDialogs dialogName;
	
	[Header("Скролл")]
	public ScrollRect scrollRect;
	
	[Header("Кнопки")]
	public ButtonComponent[] buttonText; // первый элемент списка, всегда будет использоваться для вывода текста NPC, остальные элементы для ответов, соответственно, общее их количество должно быть достаточным
	
	[Header("Папка з діалогами")]
	public string folder = "Taxi"; // подпапка в Resources, для чтения
	
	[Header("Зміщення")]
	public int offset = 20;

	public static DialogueManager _internal;
	
	private string fileName, lastName;
	private List<Dialogue> node;
	private Dialogue dialogue;
	private Answer answer;
	private float curY, height;
	private int id;
	private static bool _active;

	private void OnEnable()
	{
		CameraController.OnStartDialog += DialogueStart;
	}

	private void OnDisable()
	{
		CameraController.OnStartDialog -= DialogueStart;
	}

	private void DialogueStart(int delay = 0)
	{
		if(dialogName == NDialogs.None) return;
		
		OnVisibleDialog?.Invoke(true);
		
		DOVirtual.DelayedCall(delay, () =>
		{
			scrollRect.gameObject.SetActive(true);

			dialogName = GetDialogName();
		
			fileName = dialogName.ToString();
		
			Load();
			
			OnResumeMoveCar?.Invoke();
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
						answer.status = reader.GetAttribute("status") == global::AnswerStatus.None.ToString()? global::AnswerStatus.None : reader.GetAttribute("status") == global::AnswerStatus.Good.ToString()? global::AnswerStatus.Good : global::AnswerStatus.Bad;
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

	private void AddToList(bool exit, int toNode, string text, int questStatus, string questName, bool isActive, AnswerStatus answerStatus)
	{
		buttonText[id].gameObject.SetActive(true);
		buttonText[id].ClearSelect(isActive);
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

		if (isActive)
			SetAnswerStatus(buttonText[id].button, answerStatus);
		
		id++;

		curY += height + offset;
		RectContent();
	}

	private void SetEndedDialog(float delay)
	{
		DOVirtual.DelayedCall(delay, () =>
		{
			_active = false;
		
			//OnClearSelectAnswer?.Invoke(id);
			
			scrollRect.gameObject.SetActive(false);
			
			OnPauseCar?.Invoke(1.0f);
			
			OnCompleteDialog?.Invoke();
		});
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
			b.gameObject.SetActive(false);
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

	private void SetAnswerStatus(Button button, AnswerStatus value) // событие, для управлением статуса, текущего квеста
	{
		button.onClick.AddListener(() => AnswerStatus(value));

		button.gameObject.GetComponent<ButtonComponent>().status = value;
	}

	private void AnswerStatus(AnswerStatus value)
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
			//OnClearSelectAnswer?.Invoke(id);
			
			scrollRect.gameObject.SetActive(false);
			
			OnCompleteDialog?.Invoke();
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

				//OnClearSelectAnswer?.Invoke(id);
			});
		}).OnStart(() =>
		{
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

		AddToList(false, 0, node[j].npcText, 0, string.Empty, false, global::AnswerStatus.None); // добавление текста NPC

		if (node[j].answer.Count > 0)
		{
			for (var i = 0; i < node[j].answer.Count; i++)
			{
				var value = QuestManager.GetCurrentValue(node[j].answer[i].questName);
				// фильтр ответов, относительно текущего статуса квеста
				if (value >= node[j].answer[i].questValueGreater && node[j].answer[i].questValueGreater != 0 ||
				    node[j].answer[i].questValue == value && node[j].answer[i].questValueGreater == 0 ||
				    node[j].answer[i].questName == null)
				{
					AddToList(node[j].answer[i].exit, node[j].answer[i].toNode, node[j].answer[i].text,
						node[j].answer[i].questStatus, node[j].answer[i].questName, true,
						node[j].answer[i].status); // текст игрока
				}
			}
		}
		else
		{
			SetEndedDialog(2);
		}

		EventSystem.current.SetSelectedGameObject(scrollRect.gameObject); // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
		ShowWindow();
	}

	private NDialogs GetDialogName()
	{
		int indx = (int)NDialogs.Dialog_1;
		
		if (PlayerPrefs.HasKey("DialogID"))
		{
			indx = PlayerPrefs.GetInt("DialogID") == 3 ? 0 : PlayerPrefs.GetInt("DialogID") + 1;
		}
		
		PlayerPrefs.SetInt("DialogID", indx);
		PlayerPrefs.Save();
		
		return (NDialogs)indx;
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
	public ResultStatus result;
	public List<Answer> answer;
}

internal class Answer
{
	public string text, questName;
	public int toNode, questValue, questValueGreater, questStatus;
	public bool exit;
	public AnswerStatus status;
}