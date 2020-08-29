using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ScoreCollision : MonoBehaviour
{
  private BoxCollider myCollider;

  private Vector3 closestPoint;

  private Vector3 maxExtent;

  private const string ballTag = "Ball";

  private PointsManager pointsManager;

  [SerializeField]
  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag(ballTag))
    {
      closestPoint = myCollider.ClosestPoint(other.transform.position);

      Debug.Log(closestPoint);
      Debug.Log(transform.TransformDirection(maxExtent));

      if (Mathf.Approximately((transform.TransformDirection(maxExtent)).y, closestPoint.y))
      {
        pointsManager.AddPoints();
      }
    }
  }

  private void Awake()
  {
    myCollider = GetComponent<BoxCollider>();
  }

  private void Start()
  {
    maxExtent = myCollider.bounds.max;
    pointsManager = PointsManager.Instance;
  }

  private void OnDrawGizmos()
  {
    Gizmos.DrawSphere(closestPoint, 0.1f);
  }
}