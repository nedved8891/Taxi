using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBabchuk
{
    [CreateAssetMenu(menuName = "Settings/Create Messages", fileName = "Messages")]
    public class MessagesDatabase : ScriptableObjectBase
    {
        public List<Message> messages = new List<Message>();
    }

    [System.Serializable]
    public class Message
    {
        public int id;

            //public TMessages type;

        public string txt;

        public int nextMessageID;

        public List<int> answers;

        public Message(int _id)
        {
            id = _id;
            //type = TMessages.None;
            txt = "текст для повідомлення " + _id;
            nextMessageID = -1;
            answers = new List<int>();
        }

        public Message(Message _message)
        {
            id = _message.id;
           // type = _message.type;
            txt = _message.txt;
            nextMessageID = _message.nextMessageID;
            answers = _message.answers;
        }
    }
}
