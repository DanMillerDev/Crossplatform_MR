using DG.Tweening;
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
   
   void Start()
   { 
       m_Transform = transform;
       m_Transform.localScale = Vector3.zero;
       m_TargetScale = Random.Range(m_MinScale, m_MaxScale);

       m_Transform.DOScale(m_TargetScale, m_TimeToScale).SetEase(Ease.InQuart);
   }
}
