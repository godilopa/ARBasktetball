
using UnityEngine;
using UnityEngine.EventSystems;

public class BallShoot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
  [SerializeField]
  private Rigidbody rg;

  [SerializeField]
  private float xforceMultiplier = 1;

  [SerializeField]
  private float yforceMultiplier = 1;

  [SerializeField]
  private float zforceMultiplier = 1;

  [SerializeField]
  private float maxForce = 300;

  [SerializeField]
  private float maxDragVelocity = 0.1f;

  [SerializeField]
  private AnimationCurve curve;

  [SerializeField]
  private float screenOffset = 2.5f;

  private bool dragStart;

  private Vector3 initPoint;

  private float dragForce;

  private Vector3 dragVector = new Vector3();

  private Vector3 dragScreenPosition = new Vector3();

  private Vector3 dragDirection = new Vector3();

  private Vector3 initDirection = new Vector3();

  private Vector3 finalDirection = new Vector3();

  private Camera mainCamera;

  private float dragTime = 0;

  private bool canShoot = false;

  public bool CanShoot { set => canShoot = value; }

  private void Awake()
  {
    mainCamera = Camera.main;
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    if (!canShoot)
      return;

    dragTime = Time.time;

    initPoint = Input.mousePosition;

    rg.useGravity = false;
    rg.velocity = Vector3.zero;
    rg.angularVelocity = Vector3.zero;
    rg.transform.rotation = Quaternion.identity;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    if (!canShoot)
      return;

    //canShoot = false;

    dragTime = Mathf.Abs(dragTime - Time.time);

    float screenDragSize = Mathf.Abs(Input.mousePosition.y - initPoint.y);

    initDirection = mainCamera.ScreenToWorldPoint(new Vector3(initPoint.x, initPoint.y, 1f)) - mainCamera.transform.position;
    finalDirection = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2f)) - mainCamera.transform.position;

    float angleBetWeenDirections = Mathf.Acos(Vector3.Dot(initDirection.normalized, finalDirection.normalized)) * Mathf.Rad2Deg;

    dragDirection = finalDirection - initDirection;
    //One when screendragDize and speed is the maximum accordign parameters
    float maxForceCoefficient = (screenDragSize / Screen.height) * (maxDragVelocity / dragTime);

    dragForce = maxForce * curve.Evaluate(maxForceCoefficient);
    //Mientras mas fuerza tiene el tiro mas arqueado vas
    dragVector.Set(dragDirection.x * xforceMultiplier, dragDirection.y * (yforceMultiplier + maxForceCoefficient), dragDirection.z * zforceMultiplier);

    rg.AddForce(dragVector * dragForce);
    rg.AddTorque(-Vector3.right * 5, ForceMode.Impulse);
    rg.useGravity = true;

    PointsManager.Instance.InitPoints();

#if UNITY_EDITOR
    Debug.DrawLine(mainCamera.transform.position, mainCamera.ScreenToWorldPoint(new Vector3(initPoint.x, initPoint.y, 1f)), Color.red, 2);
    Debug.DrawLine(mainCamera.transform.position, mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2f)), Color.yellow, 2);
    Debug.DrawLine(rg.transform.position, rg.transform.position + dragVector, Color.green, 2);
    // Debug.Log("Angle" + angleBetWeenDirections);
    // Debug.Log("Force coefficient" + angleBetWeenDirections);
    // Debug.Log("Force" + dragForce);
    // Debug.Log("Time %" + maxDragVelocity / dragTime);
    // Debug.Log("Screen %" + screenDragSize / Screen.height);
#endif
  }

  public void OnDrag(PointerEventData eventData)
  {
    if (!canShoot)
      return;

    dragScreenPosition.Set(Input.mousePosition.x, Input.mousePosition.y, 1f);
    rg.transform.position = mainCamera.ScreenToWorldPoint(dragScreenPosition + mainCamera.transform.forward * screenOffset);
  }

  private void OnDrawGizmos()
  {
    if (mainCamera != null)
    {
      Gizmos.DrawSphere(mainCamera.transform.position + initDirection, .1f);
      Gizmos.DrawSphere(mainCamera.transform.position + finalDirection, .1f);
      Gizmos.DrawLine(mainCamera.transform.position + finalDirection, mainCamera.transform.position + initDirection);
    }
  }
}