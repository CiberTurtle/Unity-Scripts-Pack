#if UNITY_EDITOR
#pragma warning disable 649
using UnityEngine;
using UnityEditor;

namespace Ciber_Turtle.Internal
{
	public class Create
	{
		public static void CreateObjAtSelection(GameObject gameObject, string name)
		{
			if (gameObject)
			{
				GameObject obj = MonoBehaviour.Instantiate(gameObject);
				obj.name = name;
				if (Selection.activeTransform) obj.transform.SetParent(Selection.activeTransform, false);
				// if (Settings.settings.unpackPrefabWhenCreate) PrefabUtility.UnpackPrefabInstance(obj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
				Selection.activeTransform = obj.transform;
				Undo.RegisterCreatedObjectUndo(obj, $"Created {name}");
			}
		}
	}
}
#endif