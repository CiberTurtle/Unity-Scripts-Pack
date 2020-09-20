#pragma warning disable 649
using UnityEngine;
using System.Diagnostics;
using UnityEditor;

public class OpenProjectInCode
{
	[MenuItem("Tools/Open Project In Code")]
	static void Open()
	{
		// > -r to reuse open window (defualt) <
		// Process.Start("code", $"\"{Application.dataPath}/../\"".Replace("/", "\\"));
		Process.Start("code", "-r" + $"\"{Application.dataPath}/../\"".Replace("/", "\\"));
	}
}