using UnityEditor;

[CustomEditor(typeof(Door), true)]
public class ScenePickerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var picker = target as Door;
		var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.ScenePath);

		serializedObject.Update();

		EditorGUI.BeginChangeCheck();
		var newScene = EditorGUILayout.ObjectField("Teleport Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

		if (EditorGUI.EndChangeCheck())
		{
			var newPath = AssetDatabase.GetAssetPath(newScene);
			var scenePathProperty = serializedObject.FindProperty("ScenePath");
			scenePathProperty.stringValue = newPath;
		}
		serializedObject.ApplyModifiedProperties();
	}
}
