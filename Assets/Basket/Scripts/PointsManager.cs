
using UnityEngine;

public class PointsManager : Singleton<PointsManager>
{
  [SerializeField]
  private int basePoints = 500;

  [SerializeField]
  private int bumpMinusPoints = 50;

  [SerializeField]
  private CanvasData canvasData;

  [SerializeField]
  private string throwAudio = "Throw";

  [SerializeField]
  private string scoreAudio = "Swish";

  [SerializeField]
  private string clapsAudio = "Claps";

  private int actualPoints;

  public void InitPoints()
  {
    actualPoints = basePoints;
    canvasData.UpdateShoots();
    AudioManager.AudioManager.Instance.PlayEffect(throwAudio);
  }

  public void AddPoints()
  {
    canvasData.UpdateScore(actualPoints);
    AudioManager.AudioManager.Instance.PlayEffect(scoreAudio);

    if (actualPoints == basePoints)
    {
      Debug.Log("Tiro perfecto !");
      AudioManager.AudioManager.Instance.PlayEffect(clapsAudio);
    }
  }

  public void RemovePoints()
  {
    actualPoints -= bumpMinusPoints;
  }
}