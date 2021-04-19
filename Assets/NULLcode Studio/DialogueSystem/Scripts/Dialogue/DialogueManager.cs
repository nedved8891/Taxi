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
	public static event Action<int> OnSelectAnswer; //Вибрана відповідь і потрібно показати правильна чи ні
	
	public static event Action<float> OnTalkingPassangers; //Включити, щоб пасажир озвучив репліку
	
	public static event Action<List<Actions>> OnShowCooseActionPanel; //Показати панель дій
	
	public static event Action<Actions, bool> OnCompleteDialogue; //Завершення діалогу

	public static event Action OnDialogStarted; //Діалог стартував, потрібно налаштувати іконку для персонажа
	
	[Header("Діалог")]
	public NDialogs dialogName;
	
	[Header("Скролл")]
	public ScrollRect scrollRect;
	
	[Header("Кнопки")]
	public ButtonComponent[] buttonText;
	
	[Header("Папка з діалогами")]
	public string folder = "Taxi";
	
	[Header("Зміщення")]
	public int offset = 20;

	public static DialogueManager _internal;

	[Header("Затримка показа відповідей після тексту пажажира")] [SerializeField]
	private float _delayShowAnswers = 1f;
	
	private string fileName, lastName;
	private List<Dialogue> node;
	private Dialogue dialogue;
	private Dialogue _currentDialogue;
	private Answer answer;
	private float curY, height;
	private int id;
	private static bool _active;

	private void OnEnable()
	{
		CameraController.OnStartDialog += DialogueStart;
		
		ChooseActionPanelController.OnChooseAction += OnChooseActionHandle;
	}

	private void OnDisable()
	{
		CameraController.OnStartDialog -= DialogueStart;
		
		ChooseActionPanelController.OnChooseAction -= OnChooseActionHandle;
	}
	
	private void OnChooseActionHandle (Actions action)
	{
		var isRightAction = _currentDialogue.completeActions.Contains (action);

		PlayerPrefs.SetInt("DialogWin", isRightAction? 1 : 0);
		PlayerPrefs.Save();
		
		OnCompleteDialogue?.Invoke (action, isRightAction);
		
		Debug.Log("Завершений діалог: " + isRightAction);
	}

	private void DialogueStart(int delay = 0)
	{
		if(dialogName == NDialogs.None) return;
		
		DOVirtual.DelayedCall(delay, () =>
		{
			scrollRect.gameObject.SetActive(true);

			dialogName = GetDialogName();
		
			fileName = dialogName.ToString();
		
			Load();
			
			OnDialogStarted?.Invoke();
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

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			DialogueStart();
		}
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
					
					dialogue.isFinalyPhrase = GetBOOL ( reader.GetAttribute ( "isFinalyPhrase" ) );
					
					XmlReader inner = reader.ReadSubtree ( );
					
					if ( dialogue.isFinalyPhrase )
					{
						int actionsCount = 0;
						if ( inner.ReadToFollowing ( "completeActions" ) )
						{
							actionsCount = GetINT ( reader.GetAttribute ( "length" ) );
							dialogue.completeActions = new List<Actions> ( );

							for ( var i = 0; i < actionsCount; i++ )
							{
								dialogue.completeActions.Add ( ( Actions )
									Enum.Parse ( typeof ( Actions ), reader.GetAttribute ( "completeAction_" + i ), true ) );
							}
						}

						if ( inner.ReadToFollowing ( "looseActions" ) )
						{
							actionsCount = GetINT ( reader.GetAttribute ( "length" ) );
							dialogue.looseActions = new List<Actions> ( );

							for ( var i = 0; i < actionsCount; i++ )
							{
								dialogue.looseActions.Add ( ( Actions )
									Enum.Parse ( typeof ( Actions ), reader.GetAttribute ( "looseAction_" + i ), true ) );
							}
						}
					}
					
					node.Add(dialogue);

					//XmlReader inner = reader.ReadSubtree();
					
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

	private void AddToList(int toNode, string text, bool isActive, AnswerStatus answerStatus)
	{
		buttonText[id].gameObject.SetActive(true);
		buttonText[id].ClearSelect(isActive);
		buttonText[id].text.text = text;
		buttonText[id].textMesh.text = text;
		buttonText[id].rect.sizeDelta = new Vector2(buttonText[id].rect.sizeDelta.x, buttonText[id].text.preferredHeight + offset);
		buttonText[id].button.interactable = isActive;
		height = buttonText[id].rect.sizeDelta.y;
		buttonText[id].rect.anchoredPosition = new Vector2(0, -height/2 - curY);

		SetNextNode(buttonText[id].button, toNode);

		if (isActive)
			SetAnswerStatus(buttonText[id].button, answerStatus);
		
		id++;

		curY += height + offset;
		RectContent();
	}

	private void SetEndedDialog(float delay, int dialogueID )
	{
		DOVirtual.DelayedCall(delay, () =>
		{
			_active = false;
			
			var actionsList = new List<Actions> ( );

			_currentDialogue = node [ dialogueID ];

			if ( node [ dialogueID ].completeActions != null && node [ dialogueID ].completeActions.Count > 0 )
				actionsList.AddRange ( node [ dialogueID ].completeActions );

			if ( node [ dialogueID ].looseActions != null && node [ dialogueID ].looseActions.Count > 0 )
				actionsList.AddRange ( node [ dialogueID ].looseActions );
			
			//_chooseActionPanel.Show ( actionsList );
			OnShowCooseActionPanel?.Invoke(actionsList);
		
			scrollRect.gameObject.SetActive(false);
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

	private void SetNextNode(Button button, int i) // событие, для перенаправления на другой узел диалога
	{
		button.onClick.AddListener(() => NextBuildDialogue(i, button.gameObject.GetInstanceID()));
	}

	private void SetAnswerStatus(Button button, AnswerStatus value) // событие, для управлением статуса, текущего квеста
	{
		button.gameObject.GetComponent<ButtonComponent>().status = value;
	}

	private void CloseWindow(int id = -1) // закрываем окно диалога
	{
		_active = false;
		
		if(id != -1)
			OnSelectAnswer?.Invoke(id);
		
		DOVirtual.DelayedCall(4f, () =>
		{
			scrollRect.gameObject.SetActive(false);
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
		OnSelectAnswer?.Invoke(id);
		
		DOVirtual.DelayedCall(1f, () =>
		{
			BuildDialogue(current);
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
		
		OnTalkingPassangers?.Invoke(0.5f);

		AddToList ( current, node [ j ].npcText, false, global::AnswerStatus.None ); // добавление текста NPC

		if ( !node [ j ].isFinalyPhrase )
		{
			DOVirtual.DelayedCall ( _delayShowAnswers, ( ) =>
			{
				for ( var i = 0; i < node [ j ].answer.Count; i++ )
				{
					AddToList ( node [ j ].answer [ i ].toNode, node [ j ].answer [ i ].text,
						true, node [ j ].answer [ i ].status ); // текст игрока
				}
			}, false );
		}
		else
		{
			SetEndedDialog (2, j );
		}

		EventSystem.current.SetSelectedGameObject(scrollRect.gameObject); // выбор окна диалога как активного, чтобы снять выделение с кнопок диалога
		ShowWindow();
	}

	private NDialogs GetDialogName()
	{
		return (NDialogs)PlayerPrefs.GetInt("DialogID");
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
	public bool isFinalyPhrase = false;
	public List<Actions> completeActions, looseActions;
}

internal class Answer
{
	public string text, questName;
	public int toNode, questValue, questValueGreater, questStatus;
	public bool exit;
	public AnswerStatus status;
}