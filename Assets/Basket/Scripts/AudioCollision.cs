
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AudioCollision : MonoBehaviour
{
  [SerializeField]
  private string audioToPlay;

  private Collider myCollider;

  private const string ballTag = "Ball";

  private void OnCollisionEnter(Collision other)
  {
    if (other.collider.CompareTag(ballTag))
      AudioManager.AudioManager.Instance.PlayEffect(audioToPlay);
  }

  void Start()
  {
    myCollider = GetComponent<Collider>();
  }
}
