using UnityEditor;

[CustomEditor(typeof(Door), true)]
public class ScenePickerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var picker = target as Door;
		var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.scenePath);

		serializedObject.Update();

		EditorGUI.BeginChangeCheck();
		var newScene = EditorGUILayout.ObjectField("scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

		if (EditorGUI.EndChangeCheck())
		{
			var newPath = AssetDatabase.GetAssetPath(newScene);
			var scenePathProperty = serializedObject.FindProperty("scenePath");
			scenePathProperty.stringValue = newPath;
		}
		serializedObject.ApplyModifiedProperties();
	}
}
