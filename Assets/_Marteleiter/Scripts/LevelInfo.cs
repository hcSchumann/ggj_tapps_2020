using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelInfo : ScriptableObject
{
    public GameObject InicialObject;
    public Sprite targetObjectSprite;

#if UNITY_EDITOR
    [MenuItem("Level/Create new level")]
    public static void CreateLevel()
	{
		var asset = ScriptableObject.CreateInstance<LevelInfo>();

		string path = "Assets/_Marteleiter/Levels/";

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(LevelInfo).ToString() + ".asset");

		AssetDatabase.CreateAsset(asset, assetPathAndName);

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}
#endif
}
