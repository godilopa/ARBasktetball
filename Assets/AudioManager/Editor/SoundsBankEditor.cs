
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AudioManager
{
  /// <summary>
  /// Editor de banco de sonidos.
  /// </summary>
  [CustomEditor(typeof(SoundBank))]
  public class SoundBankEditor : Editor
  {
    SceneAsset sceneAsset;

    private ReorderableList list;

    private void OnEnable()
    {
      list = new ReorderableList(serializedObject, serializedObject.FindProperty("soundsToPlay"), true, true, true, true);

      list.drawHeaderCallback = (Rect rect) =>
      {
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 150, EditorGUIUtility.singleLineHeight), "AudioName");
        EditorGUI.LabelField(new Rect(rect.x + 150, rect.y, 150, EditorGUIUtility.singleLineHeight), "AudioClips");
        EditorGUI.LabelField(new Rect(rect.x + 300, rect.y, 70, EditorGUIUtility.singleLineHeight), "Type");
        EditorGUI.LabelField(new Rect(rect.x + 380, rect.y, 100, EditorGUIUtility.singleLineHeight), "Volume");
      };

      list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
      {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

        EditorGUI.PropertyField(new Rect(rect.x, rect.y, 150, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("audioName"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(rect.x + 150, rect.y, 150, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("audioClip"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(rect.x + 300, rect.y, 70, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("soundType"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(rect.x + 380, rect.y, 150, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("volume"), GUIContent.none);
      };
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      EditorGUILayout.PropertyField(serializedObject.FindProperty("audioBankName"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("playMusicMethod"));

      EditorGUILayout.LabelField("Sounds to play");
      list.DoLayoutList();

      EditorGUILayout.LabelField("Scenes associated");

      EditorGUILayout.BeginHorizontal();
      {
        AddLevelsButton();
        RemoveLevelsButton();
      }
      EditorGUILayout.EndHorizontal();

      sceneAsset = (SceneAsset)EditorGUILayout.ObjectField(sceneAsset, typeof(SceneAsset), false);

      PrintLevelsGUI();

      serializedObject.ApplyModifiedProperties();
    }

    private void AddLevelsButton()
    {
      if (GUILayout.Button(@"Add scene", GUILayout.MaxWidth(250f)))
      {
        SerializedProperty levels = serializedObject.FindProperty("levels");

        if (sceneAsset != null)
        {
          string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
          string sceneName = scenePath.Substring(scenePath.LastIndexOf('/') + 1, (scenePath.LastIndexOf('.') - scenePath.LastIndexOf('/')) - 1);

          if (CheckDuplicateScene(levels, sceneName) == false)
          {
            int index = levels.arraySize;
            levels.InsertArrayElementAtIndex(index);
            levels.GetArrayElementAtIndex(levels.arraySize - 1).stringValue = sceneName;
          }
        }
      }
    }

    private void RemoveLevelsButton()
    {
      if (GUILayout.Button(@"Remove scene", GUILayout.MaxWidth(250f)))
      {
        SerializedProperty levels = serializedObject.FindProperty("levels");

        if (sceneAsset != null)
        {
          string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
          string sceneName = scenePath.Substring(scenePath.LastIndexOf('/') + 1, (scenePath.LastIndexOf('.') - scenePath.LastIndexOf('/')) - 1);

          for (int i = 0; i < levels.arraySize; i++)
          {
            string st = levels.GetArrayElementAtIndex(i).stringValue;

            if (string.Equals(st, sceneName))
            {
              levels.DeleteArrayElementAtIndex(i);
              break;
            }
          }
        }
      }
    }

    private bool CheckDuplicateScene(SerializedProperty array, string name)
    {
      for (int i = 0; i < array.arraySize; i++)
      {
        SerializedProperty property = array.GetArrayElementAtIndex(i);

        if (string.Equals(property.stringValue, name))
          return true;
      }
      return false;
    }

    private void PrintLevelsGUI()
    {
      SerializedProperty levels = serializedObject.FindProperty("levels");

      for (int i = 0; i < levels.arraySize; i++)
      {
        SerializedProperty property = levels.GetArrayElementAtIndex(i);
        EditorGUI.indentLevel++;
        EditorGUILayout.BeginVertical();
        {
          EditorGUILayout.LabelField(string.Format("Escena {1}: {0}", property.stringValue, i.ToString()));
        }
        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel--;
      }
    }
  }
}
