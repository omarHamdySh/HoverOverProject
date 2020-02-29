using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveLoad))]
public class SaveLoadEditor : Editor {

	private SaveLoad saveLoad;

	void OnEnable()
	{
		saveLoad = target as SaveLoad;
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if (GUILayout.Button ("Delete All Data")) {
			if (EditorUtility.DisplayDialog ("Delete All Data","Are you sure you want to delete all data?","Yes","No")) {
				SaveLoad.DeleteAllData ();
			}
		}

		if (GUILayout.Button ("Delete Settings")) {
			if (EditorUtility.DisplayDialog ("Delete Settings","Are you sure you want to delete data settings\nlike audio, music and quality?","Yes","No")) {
				SaveLoad.DeleteSettingsData ();
			}
		}

	}
}
