using System;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class HandJointFollow : MonoBehaviour
{
    [SerializeField]
    XRHandJointID m_JointToTrack;
    
    public enum Handedness { Left, Right }

    public enum MovementType { SetPositionAndRotation, MovePosition }

    [SerializeField]
    Handedness m_Handedness;
    
    [SerializeField]
    MovementType m_MovementType;
    
    XRHandJoint m_TrackedJoint;
    XRHandSubsystem m_XRHandSubsystem;
    bool m_PoseSet = false;

    void Start()
    {
        GetHandsSubsystem();
    }
    
    void Update()
    {
        if (!CheckHandSubsystem())
            return;

        var updateSuccessFlags = m_XRHandSubsystem.TryUpdateHands(XRHandSubsystem.UpdateType.Dynamic);

        var rootPose = m_Handedness == Handedness.Right ? XRHandSubsystem.UpdateSuccessFlags.RightHandRootPose : XRHandSubsystem.UpdateSuccessFlags.LeftHandRootPose;

        if ((updateSuccessFlags & rootPose) != 0)
        {
            m_TrackedJoint = m_Handedness == Handedness.Right ? m_XRHandSubsystem.rightHand.GetJoint(m_JointToTrack) : m_XRHandSubsystem.leftHand.GetJoint(m_JointToTrack);
            
            if(m_TrackedJoint.TryGetPose(out Pose pose))
            {
                if (m_MovementType == MovementType.MovePosition)
                {
                    // object needs to have a ridigbody
                    if (transform.TryGetComponent<Rigidbody>(out var rb))
                    {
                        rb.MovePosition(pose.position);
                        rb.MoveRotation(pose.rotation);
                        m_PoseSet = true;
                    }
                    else 
                    {
                        var childenRB = transform.GetComponentInChildren<Rigidbody>();
                        if (childenRB != null)
                        {
                            childenRB.MovePosition(pose.position);
                            childenRB.MoveRotation(pose.rotation);
                            m_PoseSet = true;
                        }
                    }
                }
                
                // for objects without RB, will be used as fallback if no RB is found
                if (!m_PoseSet)
                {
                    transform.SetPositionAndRotation(pose.position, pose.rotation);
                }
            }
        }
    }

    void GetHandsSubsystem()
    {
        var xrGeneralSettings = XRGeneralSettings.Instance;
        if (xrGeneralSettings == null)
        {
            Debug.LogError("XR general settings not set");
            return;
        }

        var manager = xrGeneralSettings.Manager;
        if (manager != null)
        {
            var loader = manager.activeLoader;
            if (loader != null)
            {
                m_XRHandSubsystem = loader.GetLoadedSubsystem<XRHandSubsystem>();
                if (!CheckHandSubsystem())
                    return;

                m_XRHandSubsystem.Start();
            }
        }
    }
    
    bool CheckHandSubsystem()
    {
        if (m_XRHandSubsystem == null)
        {
#if !UNITY_EDITOR
                Debug.LogError("Could not find Hand Subsystem");
#endif
            enabled = false;
            return false;
        }

        return true;
    }
}
