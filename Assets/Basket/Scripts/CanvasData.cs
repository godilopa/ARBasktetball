using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasData : MonoBehaviour
{
  [SerializeField]
  TMP_Text totalScoreText;

  [SerializeField]
  TMP_Text shootText;

  [SerializeField]
  TMP_Text adviceText;

  [SerializeField]
  TMP_Text scoreTextFeedback;

  private const string scoreString = "Score: ";

  private const string shootsString = "Shoots: ";

  private int numberOfShoots = 0;

  private int totalScore = 0;

  private Vector2 textFeedbackOriginalPosition;

  private Vector2 textFeedbackNewPosition;

  public void UpdateShoots()
  {
    numberOfShoots++;
    shootText.text = string.Concat(shootsString, numberOfShoots.ToString());
  }

  public void UpdateScore(int score)
  {
    UpdateFeedback(score);

    totalScore += score;
    totalScoreText.text = string.Concat(scoreString, totalScore.ToString());
  }

  public void UpdateFeedback(int score)
  {
    scoreTextFeedback.enabled = true;
    scoreTextFeedback.text = score.ToString();

    LeanTween.value(scoreTextFeedback.gameObject, scoreTextFeedback.rectTransform.anchoredPosition, textFeedbackNewPosition, 1f)
    .setOnUpdate((Vector2 val) => { scoreTextFeedback.rectTransform.anchoredPosition = val; })
    .setOnComplete(() => { scoreTextFeedback.rectTransform.anchoredPosition = textFeedbackOriginalPosition; });

    LeanTween.value(0, 1, 1f).setOnUpdate((float val) => { scoreTextFeedback.alpha = val; })
    .setOnComplete(() => { scoreTextFeedback.enabled = false; scoreTextFeedback.alpha = 1; });
  }

  public void ChanngeAdviceTextOnBasketAppear()
  {
    adviceText.text = "Hoop found, move away some distance to see it";
  }

  private void Awake()
  {
    textFeedbackOriginalPosition = scoreTextFeedback.rectTransform.anchoredPosition;
    textFeedbackNewPosition = scoreTextFeedback.rectTransform.anchoredPosition + new Vector2(0, 100);
  }
}
