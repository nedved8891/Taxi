using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;

namespace SBabchuk
{
	static public class Utils
	{
#if UNITY_ANDROID || UNITY_IPHONE
        public static T GetAsset2<T>() where T : Object
        {
            return Resources.Load<T>("Databases/" + typeof(T).Name);
        }
#endif

#if UNITY_EDITOR
        public static T GetAsset<T>() where T : Object
		{
			string[] assets = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
			if (assets.Length > 0)
			{
				return (T)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(assets[0]), typeof(T));
			}

			return default(T);
		}

		public static T[] GetAssets<T>() where T : Object
		{
			string[] assetsPath = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
			if (assetsPath.Length > 0)
			{
				T[] assets = new T[assetsPath.Length];

				for (int i = 0; i < assets.Length; i++)
				{
					assets[i] = (T)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(assetsPath[i]), typeof(T));
				}

				return assets;
			}

			return null;
		}

		public static bool HasAsset<T>() where T : Object
		{
			string[] assets = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
			if (assets.Length > 0)
			{
				return true;
			}

			return false;
		}

		public static T CreateAsset<T>(System.Type type, string path, bool refresh = false) where T : ScriptableObject
		{
			T scriptableObject = (T)ScriptableObject.CreateInstance(type);

			string itemPath = path + ".asset";

			UnityEditor.AssetDatabase.CreateAsset(scriptableObject, itemPath);

			UnityEditor.AssetDatabase.SaveAssets();

			if (refresh)
				UnityEditor.AssetDatabase.Refresh();

			return scriptableObject;
		}

		public static T CreateAsset<T>(string path, bool refresh = false) where T : ScriptableObject
		{
			T scriptableObject = (T)ScriptableObject.CreateInstance(typeof(T));

			string itemPath = path + ".asset";

			UnityEditor.AssetDatabase.CreateAsset(scriptableObject, itemPath);

			UnityEditor.AssetDatabase.SaveAssets();

			if (refresh)
				UnityEditor.AssetDatabase.Refresh();

			return scriptableObject;
		}
#endif

		public static bool IsMobilePlatform ()
		{
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_WP8_1 || UNITY_WSA
			return true;
#else
			return false;
#endif
		}

        /// <summary>
        /// Зміга кольра стилю промальовки
        /// </summary>
        /// <param name="_color">Новий колір</param>
        public static void ChangeColor(Color _color)
        {
            GUI.color = _color;
        }

        /// <summary>
        /// Зміга кольра стилю промальовки
        /// </summary>
        /// <param name="_color">Новий колір</param>
        public static void CheckColor(float _field = -10, float defaultValue = 0)
        {
            if (_field == -10)
            {
                ChangeColor(Color.green);
            }
            else
            {
                if (_field == defaultValue)
                {
                    ChangeColor(Color.yellow);
                }
                else
                {
                    ChangeColor(Color.green);
                }
            }

        }

        /// <summary>
        /// Зміга кольра стилю промальовки
        /// </summary>
        /// <param name="_color">Новий колір</param>
        public static void CheckColor(Vector3 _field, Vector3 defaultValue = default(Vector3))
        {
                if (_field == defaultValue)
                {
                    ChangeColor(Color.yellow);
                }
                else
                {
                    ChangeColor(Color.green);
                }
        }

        /// <summary>
        /// Зміга кольра стилю промальовки
        /// </summary>
        /// <param name="_color">Новий колір</param>
        public static void CheckColor(int _field, int defaultValue)
        {
            if (_field == defaultValue)
            {
                ChangeColor(Color.yellow);
            }
            else
            {
                ChangeColor(Color.green);
            }
        }

        /// <summary>
        /// Зміга кольра стилю промальовки
        /// </summary>
        /// <param name="_color">Новий колір</param>
        public static void CheckColor(int _field, int defaultValue, bool isLess)
        {
            if (isLess)
            {
                if (_field < defaultValue)
                {
                    ChangeColor(Color.yellow);
                }
                else
                {
                    ChangeColor(Color.green);
                }
            }
            else
            {
                if (_field > defaultValue)
                {
                    ChangeColor(Color.yellow);
                }
                else
                {
                    ChangeColor(Color.green);
                }
            }
        }

        /// <summary>
        /// Зупиняєм твін
        /// </summary>
        /// <param name="_tween"></param>
        public static void StopTween(Tween _tween)
        {
            if (_tween != null)
            {
                _tween.Kill();

                _tween = null;
            }
        }

        /// <summary>
        /// Перетасування масива
        /// </summary>
        /// <param name="indexesList"></param>
        public static void Shuffle<T>(List<int> indexesList)
        {
            indexesList = indexesList.OrderBy(i => System.Guid.NewGuid()).ToList();
        }

        /// <summary>
        /// Перетасування масива
        /// </summary>
        /// <param name="indexesList"></param>
        public static List<T> Shuffle<T>(this List<T> indexesList)
        {
            indexesList = indexesList.OrderBy(i => System.Guid.NewGuid()).ToList();

            return indexesList;
        }

        /// <summary>
        /// Повертає наступне значення для enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException(System.String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])System.Enum.GetValues(src.GetType());
            int j = System.Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }

    }
}