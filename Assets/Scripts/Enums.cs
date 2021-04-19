using UnityEngine;

#region ENUMS

[System.Serializable]
public enum Actions
{
    None,
    Expel, //Вигнати
    Accelerate, //Прискоритись
    CallThePolice //Викликати поліцію
}

#endregion

#region CLASSES

[System.Serializable]
public class ActionsSettings
{
    [Header("Тип")]
    public Actions type;
    
    [Header("Іконка")]
    public Sprite image;
}

#endregion
