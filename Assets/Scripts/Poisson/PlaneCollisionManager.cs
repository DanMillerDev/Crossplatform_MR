using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
public class PlaneCollisionManager : MonoBehaviour
{
    public static event Action<ARPlane, Vector3?> OnPlaneCollisionEvent;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out ARPlane plane))
        {
            OnPlaneCollisionEvent?.Invoke(plane, transform.position);
        }
    }
}
