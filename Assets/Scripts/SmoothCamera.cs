using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
  public Transform target;

  // Smoothing factor for the camera follow (adjust as needed)
  [SerializeField] private float smoothness = 0.5f;

  private void LateUpdate()
  {
      if (target != null)
      {
          // Calculate the desired position for the camera
          Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

          // Use Vector3.Lerp for smooth camera follow
          transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothness * Time.deltaTime);
      }
  }
}
