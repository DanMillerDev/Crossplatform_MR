using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;

public class PlaceContentOnPlane : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_ObjectsToPlace;

    [SerializeField]
    PoissonDiscSampling m_DiscSampler;

    [SerializeField]
    bool m_ShowObjectsOnCreation = true;
    
    [SerializeField]
    bool m_AllowPlacementOnUniquePlanesOnly = true;

    [SerializeField]
    PlaneAlignmentFlags m_ValidPlaneAlignments = PlaneAlignmentFlags.HorizontalUp | PlaneAlignmentFlags.Vertical;

    PlaneAlignment m_AlignmentFlags => GetValidPlaneAlignmentFlag(m_ValidPlaneAlignments);
    
    [System.Flags]
    enum PlaneAlignmentFlags
    {
        None = 0,
        Vertical = 1, 
        HorizontalDown = 1 << 1,
        HorizontalUp = 1 << 2,
        NotAxisAligned = 1 << 3
    }
   
    public UnityEvent<List<GameObject>, Vector3> OnObjectsPlaced;

    List<Vector2> m_PoissonOutputList = new List<Vector2>();
    List<GameObject> m_SpawnedObjectsList = new List<GameObject>();
    List<TrackableId> m_UniquePlaneIds = new List<TrackableId>();
    
    Vector2 m_PlaneSize;
    Vector3 m_PlaneNormal;
    Vector3 m_HitPosition;
    GameObject m_HitPlane;
    LayerMask m_PolyspatialLayermask;
#if UNITY_EDITOR
    [SerializeField]
    Camera m_XRCamera;
#endif

    void OnEnable()
    {
        m_PolyspatialLayermask = ~(1 << LayerMask.NameToLayer("PolySpatial"));
        PoissonCollisionManager.OnPlaneCollisionEvent += PlaceObjectsOnPlane; 
    }

    void OnDisable()
    {
         PoissonCollisionManager.OnPlaneCollisionEvent -= PlaceObjectsOnPlane;
    }
    void Update()
    {
#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(m_XRCamera.ScreenPointToRay(Mouse.current.position.value), out hitInfo, 100.0f, m_PolyspatialLayermask))
            {
                if (hitInfo.transform.TryGetComponent(out ARPlane plane))
                {
                    PlaceObjectsOnPlane(plane, hitInfo.point);
                }
            }
        }
#endif
    }
    
    public void PlaceObjectsOnPlane(ARPlane plane, Vector3? hitPosition = null)
    {
        // only allow objects to be placed on unique planes ie: can't spawn objects on the same plane multiple times
        if (m_AllowPlacementOnUniquePlanesOnly && m_UniquePlaneIds.Contains(plane.trackableId))
        {
            return;
        }

        if (!m_AlignmentFlags.HasFlag(plane.alignment)) 
        {
            return;
        }

        m_UniquePlaneIds.Add(plane.trackableId);
        
        m_PlaneSize = plane.size;
        m_PoissonOutputList = m_DiscSampler.GetPointsRelativeToPlane(m_PlaneSize.x, m_PlaneSize.y);
        m_PlaneNormal = plane.normal;
        m_HitPlane = plane.gameObject;
        m_HitPosition = hitPosition ?? Vector3.zero;
        
        PlaceObjectsAtPoints();
    }   
    
    void PlaceObjectsAtPoints()
    {
        m_SpawnedObjectsList.Clear();
        foreach (Vector2 point in m_PoissonOutputList)
        {
            var placementPOS = new Vector3(point.x, 0, point.y);
            var offsetPlanePOS = placementPOS - new Vector3(m_PlaneSize.x/2, 0, m_PlaneSize.y/2);
            var worldOffsetPlanePOS = m_HitPlane.transform.TransformPoint(offsetPlanePOS);
            // ignore point if it's not over plane
            if (Physics.Raycast(  worldOffsetPlanePOS + m_PlaneNormal, -m_PlaneNormal))
            {
                var randomObjectIndex = Random.Range(0, m_ObjectsToPlace.Count);
                var newObject = Instantiate(m_ObjectsToPlace[randomObjectIndex], m_HitPlane.transform);
                newObject.transform.localPosition = offsetPlanePOS;
                newObject.transform.parent = null;
                m_SpawnedObjectsList.Add(newObject);    
            }
        }
        
        // send event with object list and hit position
        OnObjectsPlaced.Invoke(m_SpawnedObjectsList, m_HitPosition);
        
        // turn off created objects
        if (!m_ShowObjectsOnCreation)
        {
            foreach (GameObject go in m_SpawnedObjectsList)
            {
                go.SetActive(false);
            }
        }
    }

    // Convert PlaneAlignmentFlags to PlaneAlignment bit mask
    PlaneAlignment GetValidPlaneAlignmentFlag(PlaneAlignmentFlags flags)
    {
        PlaneAlignment returnFlag = PlaneAlignment.None;

        if((flags & PlaneAlignmentFlags.HorizontalUp) != 0) 
        {
            returnFlag |= PlaneAlignment.HorizontalUp;
        }
        
        if((flags & PlaneAlignmentFlags.HorizontalDown) != 0)
        {
            returnFlag |= PlaneAlignment.HorizontalDown;
        }
        
        if((flags & PlaneAlignmentFlags.Vertical) != 0)
        {
            returnFlag |= PlaneAlignment.Vertical;
        }

        if((flags & PlaneAlignmentFlags.NotAxisAligned) != 0)
        {
            returnFlag |= PlaneAlignment.NotAxisAligned;
        }

        return returnFlag;
    }
}
