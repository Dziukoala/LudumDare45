using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public FloatObject MaxSideSpeed;
    public FloatObject DashSpeed;
    public FloatObject DashCooldownValue;

    private Rigidbody SelfRigidbody;
    public Transform SelfCameraTransform;
    private Transform PlayerTransform;
    private InputManager SelfInput;

    private Vector3 HorizontalVelocity;
    private Vector3 VerticallVelocity;
    [HideInInspector] public Vector3 CalculedVelocity;
    private float DashSpeedValue;
    private float DashCooldown;

    public Transform VisualLookAt;
    public Transform VisualTransform;

    public FloatObject CollisionSpeedReduce;
    private PlayerAcceleration SelfPlayerAcceleration;

    public AudioSource playerSonorEffectsAS;
    public AudioClip dashAC;
    public AudioClip collisionAC;

    private void Awake()
    {
        SelfRigidbody = GetComponent<Rigidbody>();
        SelfInput = GetComponent<InputManager>();
        PlayerTransform = GetComponentInParent<PlayerAcceleration>().GetComponent<Transform>();
        SelfPlayerAcceleration = GetComponentInParent<PlayerAcceleration>();
    }

    private void Update()
    {
        HorizontalVelocity = PlayerTransform.right * MaxSideSpeed.Value * SelfInput.XAxis() * Time.deltaTime * (1 + DashSpeedValue);
        VerticallVelocity = PlayerTransform.up * MaxSideSpeed.Value * SelfInput.YAxis() * Time.deltaTime * (1 + DashSpeedValue);
        CalculedVelocity = HorizontalVelocity + VerticallVelocity;

        SelfRigidbody.velocity = Vector3.Lerp(SelfRigidbody.velocity, CalculedVelocity, .3f);
        
        if(DashCooldown > 0)
        {
            DashCooldown -= Time.deltaTime;
        }

        if(DashSpeedValue > 0)
        {
            DashSpeedValue -= DashSpeedValue * .25f;
        }

        if (SelfInput.Dash() && DashCooldown <= 0)
        {
            DashSpeedValue = DashSpeed.Value;
            DashCooldown = DashCooldownValue.Value;

            playerSonorEffectsAS.clip = dashAC;
            playerSonorEffectsAS.pitch = 1f;
            playerSonorEffectsAS.Play();
        }

        VisualLookAt.localPosition = Vector3.Lerp(VisualLookAt.localPosition, new Vector3(-SelfInput.XAxis() * 2, -SelfInput.YAxis() * 2, -3), .1f);
        VisualTransform.LookAt(VisualLookAt);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerMove collisionMove = collision.gameObject.GetComponent<PlayerMove>();
        PlayerAcceleration collisionAcceleration = collision.gameObject.GetComponentInParent<PlayerAcceleration>();
        if (collision.gameObject.CompareTag("Player"))
        {
            playerSonorEffectsAS.clip = collisionAC;
            playerSonorEffectsAS.pitch = Random.Range(0.9f, 1.1f);
            playerSonorEffectsAS.Play();


            float selfSideSpeed = Mathf.Abs(CalculedVelocity.x) + Mathf.Abs(CalculedVelocity.y) + Mathf.Abs(CalculedVelocity.z);
            float collisionSideSpeed = Mathf.Abs(collisionMove.CalculedVelocity.x) + Mathf.Abs(collisionMove.CalculedVelocity.y) + Mathf.Abs(collisionMove.CalculedVelocity.z);
            float sideSpeedDif = selfSideSpeed - collisionSideSpeed;
            if(sideSpeedDif > 5)
            {
                collisionAcceleration.ForwardSpeed -= CollisionSpeedReduce.Value;
            }
            else if(sideSpeedDif < -5)
            {
                SelfPlayerAcceleration.ForwardSpeed -= CollisionSpeedReduce.Value;
            }
            else
            {
                float forwardSpeedDif = SelfPlayerAcceleration.ForwardSpeed - collisionAcceleration.ForwardSpeed;
                if(forwardSpeedDif > 0)
                {
                    collisionAcceleration.ForwardSpeed -= CollisionSpeedReduce.Value;
                }
                else
                {
                    SelfPlayerAcceleration.ForwardSpeed -= CollisionSpeedReduce.Value;
                }
            }
        }
    }
}