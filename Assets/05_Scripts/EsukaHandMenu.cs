using UnityEngine;

namespace OculusIntegration.HandMenu
{
    [AddComponentMenu("XR/Hand Menu", 22)]
    public class HandMenu : MonoBehaviour
    {
        public enum UpDirection
        {
            WorldUp,
            TransformUp,
            CameraUp,
        }

        [SerializeField]
        GameObject m_HandMenuUIGameObject;

        public GameObject handMenuUIGameObject
        {
            get => m_HandMenuUIGameObject;
            set => m_HandMenuUIGameObject = value;
        }

        [Header("Hand alignment")]
        [SerializeField]
        UpDirection m_HandMenuUpDirection = UpDirection.TransformUp;

        public UpDirection handMenuUpDirection
        {
            get => m_HandMenuUpDirection;
            set => m_HandMenuUpDirection = value;
        }

        [Header("OVR Hand References")]
        [SerializeField]
        OVRHand m_LeftOVRHand;

        [Header("Position follow config.")]
        [SerializeField]
        float m_MinFollowDistance = 0.005f;

        public float minFollowDistance
        {
            get => m_MinFollowDistance;
            set => m_MinFollowDistance = value;
        }

        [SerializeField]
        float m_MaxFollowDistance = 0.03f;

        public float maxFollowDistance
        {
            get => m_MaxFollowDistance;
            set => m_MaxFollowDistance = value;
        }

        [SerializeField]
        float m_MinToMaxDelaySeconds = 1f;

        public float minToMaxDelaySeconds
        {
            get => m_MinToMaxDelaySeconds;
            set => m_MinToMaxDelaySeconds = value;
        }

        [Header("Gaze Alignment Config")]
        [SerializeField]
        bool m_HideMenuWhenGazeDiverges = true;

        public bool hideMenuWhenGazeDiverges
        {
            get => m_HideMenuWhenGazeDiverges;
            set => m_HideMenuWhenGazeDiverges = value;
        }

        [SerializeField]
        float m_MenuVisibleGazeAngleDivergenceThreshold = 35f;

        float m_MenuVisibilityDotThreshold;

        public float menuVisibleGazeDivergenceThreshold
        {
            get => m_MenuVisibleGazeAngleDivergenceThreshold;
            set => m_MenuVisibleGazeAngleDivergenceThreshold = value;
        }

        [Header("Offset Settings")]
        [SerializeField]
        Vector3 m_HandOffset = new Vector3(0.1f, 0, 0); // Offset for left hand

        Transform m_CameraTransform;

        [Header("Animation Settings")]
        [SerializeField]
        bool m_AnimateMenuHideAndReveal = true;

        public bool animateMenuHideAndRevel
        {
            get => m_AnimateMenuHideAndReveal;
            set => m_AnimateMenuHideAndReveal = value;
        }

        [SerializeField]
        float m_RevealHideAnimationDuration = 0.15f;

        public float revealHideAnimationDuration
        {
            get => m_RevealHideAnimationDuration;
            set => m_RevealHideAnimationDuration = value;
        }

        protected void Awake()
        {
            m_MenuVisibilityDotThreshold = AngleToDot(m_MenuVisibleGazeAngleDivergenceThreshold);
            m_CameraTransform = Camera.main?.transform;
        }

        protected void OnEnable()
        {
            if (m_HandMenuUIGameObject == null)
            {
                Debug.LogError($"Missing Hand Menu UI GameObject reference. Disabling {this} component.", this);
                enabled = false;
                return;
            }

            if (m_LeftOVRHand == null)
            {
                Debug.LogError($"Missing OVRHand reference for the left hand. Disabling {this} component.", this);
                enabled = false;
                return;
            }
        }

        protected void LateUpdate()
        {
            bool showMenu = false;
            Transform handTransform = null;
            Vector3 handOffset = Vector3.zero;

            if (m_LeftOVRHand.IsTracked)
            {
                handTransform = m_LeftOVRHand.transform;
                handOffset = m_HandOffset;
                showMenu = true;
            }

            if (showMenu && handTransform != null)
            {
                Vector3 targetPos = handTransform.position + handTransform.rotation * handOffset;
                Quaternion targetRot = handTransform.rotation;

                float distance = Vector3.Distance(m_HandMenuUIGameObject.transform.position, targetPos);
                if (distance > m_MinFollowDistance)
                {
                    m_HandMenuUIGameObject.transform.position = Vector3.Lerp(m_HandMenuUIGameObject.transform.position, targetPos, Time.deltaTime * 5f);
                    m_HandMenuUIGameObject.transform.rotation = Quaternion.Lerp(m_HandMenuUIGameObject.transform.rotation, targetRot, Time.deltaTime * 5f);
                }

                if (m_CameraTransform != null)
                {
                    Vector3 handToCamera = (m_CameraTransform.position - handTransform.position).normalized;
                    float dot = Vector3.Dot(handTransform.up, handToCamera);
                    showMenu = dot > 0.5f; // Only show menu if palm is facing towards the camera
                }
            }

            m_HandMenuUIGameObject.SetActive(showMenu);
        }

        static float AngleToDot(float angleDeg)
        {
            return Mathf.Cos(Mathf.Deg2Rad * angleDeg);
        }
    }
}
