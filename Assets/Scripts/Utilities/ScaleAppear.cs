using UnityEngine;

public class ScaleAppear : MonoBehaviour
{
   [SerializeField]
   float m_MinScale = 0.1f;
   
   [SerializeField]
   float m_MaxScale = 1.0f;
   
   [SerializeField]
   float m_TimeToScale = 0.2f;
   
   float m_TargetScale;
   Transform m_Transform;
   
   float m_ElapsedTime = 0.0f;
   Vector3 m_TargetScaleVector; 
   void Start()
   { 
       m_Transform = transform;
       m_Transform.localScale = Vector3.zero;
       m_TargetScale = Random.Range(m_MinScale, m_MaxScale);
       m_TargetScaleVector = new Vector3(m_TargetScale, m_TargetScale, m_TargetScale);
   }

   void Update()
   {
      if(m_ElapsedTime < m_TimeToScale)
      {
         m_ElapsedTime += Time.deltaTime;
         float time = Mathf.Clamp01(m_ElapsedTime/m_TimeToScale);
         float inQuartEasedTime = Mathf.Pow(time, 4);
         transform.localScale = Vector3.LerpUnclamped(Vector3.zero, m_TargetScaleVector, inQuartEasedTime);
      } 
   }
}
