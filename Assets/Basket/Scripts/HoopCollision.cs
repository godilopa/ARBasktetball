
using UnityEngine;

public class HoopCollision : MonoBehaviour
{
  private const string ballTag = "Ball";

  private PointsManager pointsManager;

  private void OnCollisionEnter(Collision other)
  {
    if (other.collider.CompareTag(ballTag))
    {
      //Debug.Log("Bola chocando");
      pointsManager.RemovePoints();
    }
  }

  private void Awake()
  {
    pointsManager = PointsManager.Instance;
  }
}
