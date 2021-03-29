using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace SBabchuk.Databases
{
    public class MessagesDrawer
    {
        private static Color defaultColor;

        private static int selected = 0;
        
        private static int selectedMode = 0;

        private static MessagesDatabase database;

        public static void Draw(MessagesDatabase _database, int selectedMode)
        {
            if (database == null)
                database = _database;

            defaultColor = GUI.color;
            
            GUI.color = Color.grey;
            
            DrawNavigation(); //Промалбовуєм навігацію

            GUILayout.BeginVertical("box");
            {
                if (database)
                {
                    if (database.messages != null)
                    {
                        if (database.messages.Count > 0)
                        {
                            if (selectedMode == 0)
                            {
                                foreach (var _weapon in database.messages)
                                {
                                    if (DrawMessage(_weapon))
                                        break;
                                }
                            }
                            else
                            {
                                DrawMessage(database.messages[selected]);
                            }
                        }
                    }
                }
            }
            GUILayout.EndVertical();
        }
        
        private static void DrawNavigation()
        {
            Utils.ChangeColor(Color.grey);
            GUILayout.BeginVertical("box");
            {
                GUI.color = defaultColor;
                EditorGUILayout.LabelField("Налаштування:");

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Добавити новий запис"))
                    {
                        database.messages.Add(new Message(database.messages.Count));
                        selected = database.messages.Count - 1;
                    }

                    if (GUILayout.Button("Видалити всі записи", GUILayout.Width(150)))
                    {
                        database.messages.Clear();
                        selected = 0;
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    if (selectedMode == 1)
                    {
                        if (GUILayout.Button("<--", GUILayout.Width(50)))
                        {
                            selected = Mathf.Max(0, selected - 1);
                        }
                        if (GUILayout.Button("-->", GUILayout.Width(50)))
                        {
                            selected = Mathf.Min(database.messages.Count == 0 ? 0 : database.messages.Count - 1, selected + 1);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

               
            }
            GUILayout.EndVertical();
        }
        
        private static bool DrawMessage(Message _message)
        {
            GUILayout.BeginHorizontal("box");
            {
                GUILayout.BeginVertical();
                {
                    //Малюєм іконку
                    //_message.ico = (Sprite)EditorGUILayout.ObjectField(_message.ico, typeof(Sprite), false, GUILayout.Width(75), GUILayout.Height(75));

                    //Кнопка видалення поточного поля
                    if (GUILayout.Button("Видалити", GUILayout.Width(75), GUILayout.Height(20)))
                    {
                        database.messages.Remove(_message);
                        selected = Mathf.Max(0, selected - 1);
                        return true;
                    }
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    _message.id = EditorGUILayout.IntField("ID повідомлення: ", _message.id);
                    
                    //Utils.CheckColor((int)_message.type, -1);
                    //_message.type = ((TMessages)EditorGUILayout.EnumPopup("Тип повідомлення", (TMessages)_message.type));
                    //Utils.ChangeColor(defaultColor);
                    
                    Utils.CheckColor(_message.txt.Length, 0);
                    _message.txt = EditorGUILayout.TextField("Повідомлення: ", _message.txt);
                    Utils.ChangeColor(defaultColor);

                    Utils.CheckColor(_message.nextMessageID, -1);
                    _message.nextMessageID = EditorGUILayout.IntField("ID наступного повідомлення: ", _message.nextMessageID);
                    Utils.ChangeColor(defaultColor);

                    if (_message.nextMessageID != -1)
                        DrawNextMessage(_message.nextMessageID);

                    //if (_message.type == TMessages.Question)
                        //DraweAnswers();

                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            return false;
        }
       
         private static void DrawNextMessage(int _id)
         {
             if (database.messages.Count > 0)
             {
                 var nextMessage = database.messages.Find(x => x.id == _id);

                 if (nextMessage != null)
                 {
                     nextMessage.txt = EditorGUILayout.TextField("Наступне повідомлення: ", nextMessage.txt);
                 }
             }
             else
             {
                 EditorGUILayout.LabelField("Немає записів");
             }
         }
         
         private static void DraweAnswers(int index)
         {
             
         }
    }
}
