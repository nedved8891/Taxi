using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SBabchuk
{
    public class ScriptableObjectBase : ScriptableObject
    {
        /// <summary>
        /// Згенерувати
        /// </summary>
        public virtual void Generate()
        {
           
        }

        /// <summary>
        /// Зберегти
        /// </summary>
        public void Save()
        {
            SaveSO(this);
        }

        /// <summary>
        /// Збереження ScriptableObject
        /// </summary>
        /// <param name="_objectsToPersist"></param>
        public void SaveSO(ScriptableObject _objectsToPersist)
        {
            string path = Application.persistentDataPath;

            //if (!Directory.Exists(path))
            //    Directory.CreateDirectory(path);

            //Debug.Log("SaveSO " + _objectsToPersist);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path + string.Format("/{0}_{1}.pso", "Main", _objectsToPersist.name));
            var json = JsonUtility.ToJson(_objectsToPersist);
            bf.Serialize(file, json);
            bf.Serialize(file, json);
            file.Close();
        }
    }
}
