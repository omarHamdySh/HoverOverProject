using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HOVEditor : EditorWindow {

	[MenuItem("HOV/Save Load/Delete All Data")]
	public static void DeleteAllData()
	{
		if (EditorUtility.DisplayDialog ("Delete All Data","Are you sure you want to delete all data?","Yes","No")) {
			SaveLoad.DeleteAllData ();
		}
	}

	[MenuItem("HOV/Save Load/Delete Settings Data")]
	public static void DeleteSettingsData()
	{
		if (EditorUtility.DisplayDialog ("Delete Settings","Are you sure you want to delete data settings\nlike audio, music and quality?","Yes","No")) {
			SaveLoad.DeleteSettingsData ();
		}
	}

	[MenuItem("HOV/Save Load/Delete Current Level Data")]
	public static void DeleteCurrentLevelData()
	{
		LevelInfo levelInfo = FindObjectOfType<LevelInfo> ();

		if (levelInfo == null) {
			EditorUtility.DisplayDialog ("Delete Settings", "Open any level except the (main menu) to delete its data", "Ok");

		} else {
			if (EditorUtility.DisplayDialog ("Delete Level Progress","Do you want to delet this level data?","Yes","No")) {
				levelInfo.DeleteAllProgress ();
			}
		}
	}
}
