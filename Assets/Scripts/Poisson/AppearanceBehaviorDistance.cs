using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceBehaviorDistance : MonoBehaviour
{
    [SerializeField]
    PlaceContentOnPlane m_PlaceContentOnPlane;
    
    [SerializeField]
    PoissonDiscSampling m_DiscSampler;

    [SerializeField]
    float m_SpawnDelay = 0.1f;
    
    [SerializeField]
    bool m_RandomizeRotation = true;
    
    List<GameObject> m_SpawnedObjectsList;
    List<List<GameObject>> m_SpawnRings;
    const int k_MaxSpawnRings = 20;
    
    void Start()
    {
        m_PlaceContentOnPlane.OnObjectsPlaced.AddListener(ShowContentSequence);

        m_SpawnRings = new List<List<GameObject>>();
        // init ring list
        for (int i = 0; i < k_MaxSpawnRings; i++)
        {
            var newList = new List<GameObject>();
            m_SpawnRings.Add(newList);
        }
    }

    void ShowContentSequence(List<GameObject> placedObjects, Vector3 spawnOrigin)
    {
        m_SpawnedObjectsList = placedObjects;
        OrganizePointsByDistance(spawnOrigin);
        StartCoroutine(ShowRings());
    }

    void OrganizePointsByDistance(Vector3 origin)
    {
        // clear list
        foreach (List<GameObject> ringList in m_SpawnRings)
        {
            ringList.Clear();
        }
    
        for (int i = 0; i < m_SpawnedObjectsList.Count; i++)
        {
            var pointDistance = Vector3.Distance(origin, m_SpawnedObjectsList[i].transform.position);
            var listIndex = (int)(pointDistance / m_DiscSampler.MinDistance);
            if (listIndex >= k_MaxSpawnRings) { listIndex = k_MaxSpawnRings-1;}
            m_SpawnRings[listIndex].Add(m_SpawnedObjectsList[i]);
        } 
    }

    IEnumerator ShowRings()
    {
        // wait for end of frame to make sure ring list is populated
        yield return new WaitForEndOfFrame();
        
        for (int i = 0; i < k_MaxSpawnRings; i++)
        {
            if (m_SpawnRings[i].Count > 0)
            {
                foreach (GameObject go in m_SpawnRings[i])
                {
                    go.SetActive(true);
                    
                    // TODO: fix bug with visionOS plane rotation for vertical objects
                    if (m_RandomizeRotation)
                    {
                        if (go.transform.up != Vector3.up)
                        {
                            // maintain rotation for vertical objects
                            go.transform.localEulerAngles += new Vector3(Random.Range(0.0f, 360.0f), 0, 0);
                        }
                        else
                        {
                            go.transform.localEulerAngles = new Vector3(0, Random.Range(0.0f, 360.0f), 0);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(m_SpawnDelay);
        }
    }
}