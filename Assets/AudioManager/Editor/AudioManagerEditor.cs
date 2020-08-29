
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AudioManager
{
  /// <summary>
  /// Editor del audio manager.
  /// </summary>
  [CustomEditor(typeof(AudioManager))]
  public class AudioManagerEditor : Editor
  {
    AudioManager audioManager;

    private bool showInfo;

    private bool showSoundBanks;

    private bool showAddSoundBanks;

    private List<bool> showSoundBandClipsList = new List<bool>();

    private GUIStyle style = new GUIStyle();

    public override void OnInspectorGUI()
    {
      GUIStyle foldoutStyle = CreateFoldoutGUI();

      EditorGUI.indentLevel++;
      EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true));
      {
        EditorGUILayout.BeginHorizontal();
        {
          GUI.color = Color.white;
          showInfo = EditorGUILayout.Foldout(showInfo, new GUIContent(@"Info", @"Escena actual y canciones que se estan reproduciendo."), foldoutStyle);
        }
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndVertical();
      EditorGUI.indentLevel--;

      EditorPrefs.SetBool("showInfo", showInfo);
      if (showInfo == true)
      {
        EditorGUILayout.BeginVertical();
        {
          ShowInfo();
        }
        EditorGUILayout.EndVertical();
      }
      EditorGUILayout.Separator();

      EditorGUI.indentLevel++;
      EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true));
      {
        EditorGUILayout.BeginHorizontal();
        {
          GUI.color = Color.white;
          showSoundBanks = EditorGUILayout.Foldout(showSoundBanks, new GUIContent(@"Ver Bancos de sonidos", @"Bancos de sonidos del juego."), foldoutStyle);
        }
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndVertical();
      EditorGUI.indentLevel--;

      EditorPrefs.SetBool("showSoundBanks", showSoundBanks);
      if (showSoundBanks == true)
      {
        EditorGUILayout.BeginVertical();
        {
          ShowAddSoundBankList();
        }
        EditorGUILayout.EndVertical();
      }
      EditorGUILayout.Separator();

      EditorGUI.indentLevel++;
      EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true));
      {
        EditorGUILayout.BeginHorizontal();
        {
          GUI.color = Color.white;
          showAddSoundBanks = EditorGUILayout.Foldout(showAddSoundBanks, new GUIContent(@"Añadir SoundBank"), foldoutStyle);
        }
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndVertical();
      EditorGUI.indentLevel--;

      EditorPrefs.SetBool("showAddSoundBanks", showAddSoundBanks);
      if (showAddSoundBanks == true)
      {
        EditorGUILayout.BeginVertical();
        {
          AddSoundBanks();
        }
        EditorGUILayout.EndVertical();
      }
      EditorGUILayout.Separator();

      EditorGUILayout.ObjectField(serializedObject.FindProperty("audioEffectsPool"));

      serializedObject.ApplyModifiedProperties();
      EditorUtility.SetDirty(target);
    }

    private GUIStyle CreateFoldoutGUI()
    {
      GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
      Color myStyleColor = Color.white;
      myFoldoutStyle.fontStyle = FontStyle.Bold;
      myFoldoutStyle.normal.textColor = myStyleColor;
      myFoldoutStyle.onNormal.textColor = myStyleColor;
      myFoldoutStyle.hover.textColor = myStyleColor;
      myFoldoutStyle.onHover.textColor = myStyleColor;
      myFoldoutStyle.focused.textColor = myStyleColor;
      myFoldoutStyle.onFocused.textColor = myStyleColor;
      myFoldoutStyle.active.textColor = myStyleColor;
      myFoldoutStyle.onActive.textColor = myStyleColor;

      return myFoldoutStyle;
    }

    private void ShowInfo()
    {
      EditorGUI.indentLevel++;
      {
        EditorGUILayout.BeginHorizontal();
        {
          EditorGUILayout.LabelField(@"<b>Escena actual:</b>", style);
          EditorGUILayout.LabelField(EditorSceneManager.GetActiveScene().name);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
          EditorGUILayout.LabelField(@"<b>Duracion del fade:</b>", style);
          audioManager.FadeDuration = EditorGUILayout.FloatField(audioManager.FadeDuration);
        }
        EditorGUILayout.EndHorizontal();

        if (audioManager.FadeDuration < 0)
          audioManager.FadeDuration = 0f;

        int audioSize = 0;

        if (audioManager.MusicAudioSources != null && audioManager.MusicAudioSources[0] != null && audioManager.MusicAudioSources[1] != null)
          audioSize = audioManager.MusicAudioSources.Length;

        style.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.LabelField(@"<b>Musica reproduciendose</b>", style);
        //Musica reproduciendo
        if (audioSize != 0)
        {
          string name1, name2;
          name1 = name2 = @"No Song";

          GUI.color = Color.green;
          if (audioManager.MusicAudioSources[0] != null && audioManager.MusicAudioSources[0].clip != null)
            name1 = audioManager.MusicAudioSources[0].clip.name;
          else
            GUI.color = Color.red;

          Rect rect = GUILayoutUtility.GetRect(28, 28, @"TextField");
          if (name1 != @"No Song")
            name1 += "\n" + System.TimeSpan.FromSeconds(audioManager.MusicAudioSources[0].time).ToString().Split('.')[0] + @"/" + System.TimeSpan.FromSeconds(audioManager.MusicAudioSources[0].clip.length).ToString().Split('.')[0];
          EditorGUI.ProgressBar(rect, audioManager.MusicAudioSources[0].volume, name1);

          GUI.color = Color.green;
          if (audioManager.MusicAudioSources[1] != null && audioManager.MusicAudioSources[1].clip != null)
            name2 = audioManager.MusicAudioSources[1].clip.name;
          else
            GUI.color = Color.red;

          Rect rect2 = GUILayoutUtility.GetRect(28, 28, @"TextField");
          if (name2 != @"No Song")
            name2 += @"\n" + System.TimeSpan.FromSeconds(audioManager.MusicAudioSources[1].time).ToString().Split('.')[0] + @"/" + System.TimeSpan.FromSeconds(audioManager.MusicAudioSources[1].clip.length).ToString().Split('.')[0];
          EditorGUI.ProgressBar(rect2, audioManager.MusicAudioSources[1].volume, name2);

          GUI.color = Color.white;
          Repaint();
        }
        else
        {
          GUI.color = Color.red;
          Rect rect = GUILayoutUtility.GetRect(28, 28, @"TextField");
          EditorGUI.ProgressBar(rect, 0f, @"Standing By...");

          Rect rect2 = GUILayoutUtility.GetRect(28, 28, @"TextField");
          EditorGUI.ProgressBar(rect2, 0f, @"Standing By...");
          GUI.color = Color.white;
        }
      }

      EditorGUILayout.LabelField(@"<b>Efectos reproduciendose</b>", style);
      //Efectos reproduciendo
      if (audioManager.EffectsActive.Count != 0)
      {
        int length = audioManager.EffectsActive.Count;
        for (int i = 0; i < length; i++)
        {
          string name1 = audioManager.EffectsActive[i].clip.name;
          GUI.color = Color.green;
          Rect effectRect = GUILayoutUtility.GetRect(28, 28, @"TextField");
          name1 += "\n" + System.TimeSpan.FromSeconds(audioManager.EffectsActive[i].time).ToString().Split('.')[0] + @"/" + System.TimeSpan.FromSeconds(audioManager.EffectsActive[i].clip.length).ToString().Split('.')[0];
          EditorGUI.ProgressBar(effectRect, audioManager.EffectsActive[i].volume, name1);
        }
        GUI.color = Color.white;
        Repaint();
      }

      style.alignment = TextAnchor.MiddleLeft;
      EditorGUI.indentLevel--;
      EditorGUILayout.Space();
    }

    private void ShowAddSoundBankList()
    {
      EditorGUI.indentLevel++;
      {
        for (int i = 0; i < audioManager.SoundsBank.Count; i++)
        {
          EditorGUILayout.BeginVertical(new GUIStyle(EditorStyles.helpBox));
          {
            ShowSoundBank(audioManager.SoundsBank[i], i);
          }
          EditorGUILayout.EndVertical();
        }
      }
      EditorGUI.indentLevel--;

      EditorGUILayout.Space();
    }

    private void ShowSoundBank(SoundBank soundBank, int index)
    {
      EditorGUILayout.BeginHorizontal();
      {
        EditorGUILayout.LabelField(string.Format("<b>Nombre: </b> {0}", soundBank.AudioBankName), style);
      }
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.LabelField(string.Format("<b>Modo de reproduccion:</b> {0}", soundBank.PlayMusicMethod), style);

      for (int i = 0; i < soundBank.Levels.Length; i++)
      {
        EditorGUI.indentLevel++;
        EditorGUILayout.BeginVertical();
        {
          EditorGUILayout.LabelField(string.Format("Escena {1}: {0}", soundBank.Levels[i], i.ToString()));
        }
        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel--;
      }

      EditorGUILayout.Separator();

      GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
      GUIStyle textStyle = new GUIStyle(EditorStyles.textField);
      foldoutStyle.font = EditorStyles.boldFont;

      GUI.color = Color.white;

      if (showSoundBandClipsList.Count <= index)
        showSoundBandClipsList.Add(true);

      showSoundBandClipsList[index] = EditorGUILayout.Foldout(showSoundBandClipsList[index], new GUIContent(@"Ver audio clips del banco"), foldoutStyle);

      EditorPrefs.SetBool("showList" + index, showSoundBandClipsList[index]);
      if (showSoundBandClipsList[index] == true)
      {
        for (int i = 0; i < soundBank.SoundsToPlay.Count; i++)
        {
          EditorGUI.indentLevel++;
          EditorGUILayout.BeginVertical();
          {
            EditorGUILayout.LabelField(string.Format("Nombre: {0}", soundBank.SoundsToPlay[i].audioName), textStyle);
            EditorGUILayout.LabelField(string.Format("Volumen: {0}", soundBank.SoundsToPlay[i].volume.ToString()));
            if (soundBank.SoundsToPlay[i].audioClip != null)
              EditorGUILayout.LabelField(string.Format("Clip: {0}", soundBank.SoundsToPlay[i].audioClip.name));
            EditorGUILayout.LabelField(string.Format("Tipo de sonido: {0}", soundBank.SoundsToPlay[i].soundType.ToString()));
          }
          EditorGUILayout.EndVertical();
          EditorGUI.indentLevel--;
        }
      }
    }

    private void AddSoundBanks()
    {
      DropAreaGUI();

      EditorGUILayout.Space();

      EditorGUI.indentLevel++;
      EditorGUILayout.PropertyField(serializedObject.FindProperty("soundsBank"));
      EditorGUI.indentLevel--;

      EditorGUILayout.Space();
    }

    private void DropAreaGUI()
    {
      GUI.color = Color.gray;
      EditorGUILayout.BeginVertical();
      {
        Event evt = Event.current;

        Rect dropArea = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, @"Drag AudioBanks Here ");

        switch (evt.type)
        {
          case EventType.DragUpdated:
          case EventType.DragPerform:
            if (!dropArea.Contains(evt.mousePosition))
              break;

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
              DragAndDrop.AcceptDrag();

              for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
              {
                SoundBank audioBank = DragAndDrop.objectReferences[i] as SoundBank;
                if (!audioBank || audioBank.GetType() != typeof(SoundBank))
                  continue;

                bool addAudio = true;

                for (int j = 0; j < audioManager.SoundsBank.Count; j++)
                {
                  if (string.Equals(audioManager.SoundsBank[j].name, audioBank.name))
                  {
                    addAudio = false;
                    break;
                  }
                }

                if (addAudio)
                  audioManager.SoundsBank.Add(audioBank);
              }
            }

            Event.current.Use();
            break;
        }
      }

      EditorGUILayout.EndVertical();
      GUI.color = Color.white;
    }

    private void OnEnable()
    {
      audioManager = target as AudioManager;

      style.richText = true;

      showInfo = EditorPrefs.GetBool("showInfo");
      showSoundBanks = EditorPrefs.GetBool("showSoundBanks");
      showAddSoundBanks = EditorPrefs.GetBool("showAddSoundBanks");

      for (int i = 0; i < showSoundBandClipsList.Count; i++)
        showSoundBandClipsList[i] = EditorPrefs.GetBool("showList" + i);
    }
  }
}