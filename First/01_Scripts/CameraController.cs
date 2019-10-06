using UnityEngine;

public class CameraController : MonoBehaviour
{
    public FloatObject SmoothRotationValue;
    public FloatObject SmoothPositionValue;
    public Transform CalculatedCamera;
    private Transform SelfTransform;
    public Transform PlayerTransform;
    [HideInInspector] public float SmoothRotation;
    [HideInInspector] public float SmoothPosition;

    private void Awake()
    {
        SelfTransform = GetComponent<Transform>();
    }

    public void NewRotation()
    {
        SelfTransform.position = Vector3.Lerp(SelfTransform.position, CalculatedCamera.position, SmoothPosition);
        SelfTransform.rotation = Quaternion.Lerp(SelfTransform.rotation, CalculatedCamera.rotation, SmoothRotation);
    }
}