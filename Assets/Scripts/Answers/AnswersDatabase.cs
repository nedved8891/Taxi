using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBabchuk
{
    
    [CreateAssetMenu(menuName = "Settings/Create Answers", fileName = "Answers")]
    public class AnswersDatabase : ScriptableObjectBase
    {
        public List<Answer> answers;
    }
    
    [System.Serializable]
    public class Answer
    {
        public int ID;

        public string txt;
    }
}
