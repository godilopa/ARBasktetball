
using System.Collections.Generic;

using UnityEngine;

namespace AudioManager
{
  /// <summary>
  /// Banco de sonidos.
  /// </summary>
  [CreateAssetMenu(fileName = "SoundBank", menuName = "ScriptableObjects/SoundBank")]
  public class SoundBank : ScriptableObject
  {
    public string AudioBankName { get => audioBankName; }
    public string[] Levels { get => levels; }
    public List<Soundconfig> SoundsToPlay { get => soundsToPlay; }
    public AudioManager.PlayMusicMethod PlayMusicMethod { get => playMusicMethod; }

    [System.Serializable]
    public struct Soundconfig
    {
      public AudioClip audioClip;
      public AudioManager.SoundType soundType;
      public string audioName;
      [Range(0, 1)]
      public float volume;
    }

    [SerializeField]
    private string audioBankName;

    [SerializeField]
    private List<Soundconfig> soundsToPlay = new List<Soundconfig>();

    [SerializeField]
    private string[] levels;

    [SerializeField]
    private AudioManager.PlayMusicMethod playMusicMethod;
  }
}