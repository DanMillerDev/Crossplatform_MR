#if UNITY_ANDROID
using UnityEngine.Android;
#endif // UNITY_ANDROID
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

namespace UnityEngine.XR.OpenXR.Features.Meta.Tests
{
    public class PermissionsCheck : MonoBehaviour
    {
        const string k_MetaScenePermission = "com.oculus.permission.USE_SCENE";
        const string k_ScenePermission = "android.permission.SCENE_UNDERSTANDING";
        const string k_HandTrackingPermission = "android.permission.HAND_TRACKING";
        const string k_AvatarEyesPermission = "android.permission.EYE_TRACKING";
        const string k_EyeGazeFinePermisson = "android.permission.EYE_TRACKING_FINE";

        [SerializeField]
        ARPlaneManager m_ARPlaneManager;



#if UNITY_ANDROID
        void Start()
        {
            var permissions = new List<string>();

            // add permissions that have not been granted by the user
            if (!Permission.HasUserAuthorizedPermission(k_MetaScenePermission))
            {
                permissions.Add(k_MetaScenePermission);
            }
            else
            {
                // enable the AR Plane Manager component if permission is already granted
                m_ARPlaneManager.enabled = true;
            }

            if (!Permission.HasUserAuthorizedPermission(k_HandTrackingPermission))
            {
                permissions.Add(k_HandTrackingPermission);
            }

            if (!Permission.HasUserAuthorizedPermission(k_EyeGazeFinePermisson))
            {
                permissions.Add(k_EyeGazeFinePermisson);
            }

            if (!Permission.HasUserAuthorizedPermission(k_AvatarEyesPermission))
            {
                permissions.Add(k_AvatarEyesPermission);
            }

            // setup callbacks to be called depending on whether permission is granted
            var callbacks = new PermissionCallbacks();
            callbacks.PermissionDenied += OnPermissionDenied;
            callbacks.PermissionGranted += OnPermissionGranted;

            Permission.RequestUserPermissions(permissions.ToArray(), callbacks);
        }

        void OnPermissionDenied(string permission)
        {
            // TODO handle denied permission
        }

        void OnPermissionGranted(string permission)
        {
            // enable the corresponding AR Manager component after required permission is granted
            if (permission == k_MetaScenePermission)
            {
                m_ARPlaneManager.enabled = true;
            }
        }
#endif // UNITY_ANDROID
    }
}