using System.Collections;
using System.Collections.Generic;
using Unity.MARS;
using UnityEngine;
using UnityEngine.Events;



public class MARSCameraDistanceCondition : MonoBehaviour
{
  [SerializeField]
  private Transform basket;

  [SerializeField]
  private float distance;

  [SerializeField]
  private UnityEvent OnAppearEvent;

  [SerializeField]
  private UnityEvent OnDisappearEvent;

  private Transform mainCameraTransform;

  private WaitForSeconds wait = new WaitForSeconds(0.5f);

  private IEnumerator coroutine;

  public void BasketAppear()
  {
    //Mensaje de aviso de separarte de la canasta
    StartCoroutine(coroutine);
  }

  private void Awake()
  {
    mainCameraTransform = MarsRuntimeUtils.GetActiveCamera().transform;
    coroutine = CheckDistance();
  }

  IEnumerator CheckDistance()
  {
    while (true)
    {
      if ((basket.position - mainCameraTransform.position).sqrMagnitude < distance * distance)
      {
        OnDisappearEvent.Invoke();
        basket.gameObject.SetActive(false);
      }
      else
      {
        OnAppearEvent.Invoke();
        basket.gameObject.SetActive(true);
      }

      yield return wait;
    }
  }
}
