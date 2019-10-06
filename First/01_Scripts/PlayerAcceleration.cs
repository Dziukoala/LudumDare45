using UnityEngine;

public class PlayerAcceleration : MonoBehaviour
{
    public float ForwardSpeed;
    public FloatObject MaxForwardSpeed;
    public FloatObject BoostSpeed;
    public FloatObject MaxAspirationSpeed;
    public FloatObject ForwardAcceleration;
    public FloatObject ForwardDecceleration;
    public FloatObject ObstacleSpeedReduce;

    public ParticleSystem fxBoost;
    public ParticleSystem fxBoostAspi;
    public ParticleSystem impactBoost;
    public ParticleSystem impactDmg;

    private RaceController GlobRaceController;
    private int NextRacepoint;

    private Transform SelfTransform;
    public Transform SelfLookAt;
    public CameraController SelfCameraController;
    private PlayerObject SelfPlayerObject;
    private Transform SelfPlayerTransform;

    public FloatObject BaseRotationSpeed;
    private float RotationSpeed;
    public FloatObject MinDistanceFromPoint;

    public AudioSource playerSonorEffectsAS;
    public AudioClip lubAC;
    public AudioClip impactAC;

    public FloatObject TimeForEnableAspiration;
    public FloatObject TimeForDisableAspiration;
    public FloatObject AspirationIncresseValue;
    public FloatObject AspirationDistance;
    public LayerMask AspirationMask;
    public LayerMask ObstacleMask;
    private float AspirationEnableTime;
    private float AspirationDisableTime;
    private bool IsOnAspiration;

    public Animator playerAnimator;
    public Animator playerAnimator02;
    public AudioClip aspirationAC;

    private void Awake()
    {
        SelfTransform = GetComponent<Transform>();
        GlobRaceController = FindObjectOfType<RaceController>();
        RotationSpeed = BaseRotationSpeed.Value;
        SelfPlayerObject = GetComponentInChildren<PlayerObject>();
        SelfPlayerTransform = GetComponentInChildren<PlayerMove>().GetComponent<Transform>();
    }

    private void Update()
    {
        Debug.DrawRay(Vector3.zero, Vector3.one);
        playerAnimator.SetFloat("speed", ForwardSpeed);
        playerAnimator02.SetFloat("speed", ForwardSpeed);

        if (ForwardSpeed < MaxForwardSpeed.Value + 1)
        {
            fxBoost.Stop();
        }

        if (ForwardSpeed > MaxForwardSpeed.Value)
        {
            ForwardSpeed = Mathf.Lerp(ForwardSpeed, MaxForwardSpeed.Value, ForwardDecceleration.Value);
        }
        else
        {
            if (!IsOnAspiration)
            {
                ForwardSpeed = Mathf.Lerp(ForwardSpeed, MaxForwardSpeed.Value, ForwardAcceleration.Value);
            }
        }
        SelfTransform.Translate(Vector3.forward * ForwardSpeed * Time.deltaTime);

        SelfLookAt.LookAt(GlobRaceController.Racepoints[NextRacepoint], GlobRaceController.Racepoints[NextRacepoint].up);
        SelfTransform.rotation = Quaternion.Lerp(SelfTransform.rotation, SelfLookAt.rotation, RotationSpeed);
        SelfCameraController.NewRotation();

        if (Vector3.Distance(SelfTransform.position, GlobRaceController.Racepoints[NextRacepoint].position) < MinDistanceFromPoint.Value)
        {
            if (GlobRaceController.RacePointsInfo[NextRacepoint].PlayerRotationSpeed > 0)
            {
                RotationSpeed = GlobRaceController.RacePointsInfo[NextRacepoint].PlayerRotationSpeed;
            }
            else
            {
                RotationSpeed = BaseRotationSpeed.Value;
            }
            if (GlobRaceController.RacePointsInfo[NextRacepoint].CameraRotationSpeed > 0)
            {
                SelfCameraController.SmoothRotation = GlobRaceController.RacePointsInfo[NextRacepoint].CameraRotationSpeed;
            }
            else
            {
                SelfCameraController.SmoothRotation = SelfCameraController.SmoothRotationValue.Value;
            }
            if (GlobRaceController.RacePointsInfo[NextRacepoint].CameraPositionSpeed > 0)
            {
                SelfCameraController.SmoothPosition = GlobRaceController.RacePointsInfo[NextRacepoint].CameraPositionSpeed;
            }
            else
            {
                SelfCameraController.SmoothPosition = SelfCameraController.SmoothPositionValue.Value;
            }
            NextRacepoint++;
            SelfCameraController.NewRotation();
        }

        Aspiration();
    }

    public void Boost()
    {
        playerSonorEffectsAS.clip = lubAC;
        playerSonorEffectsAS.pitch = 1f;
        playerSonorEffectsAS.Play();

        ForwardSpeed = Mathf.Clamp(ForwardSpeed + BoostSpeed.Value * .1f, 0, BoostSpeed.Value);
        fxBoost.Play();
    }

    public void Obstacle()
    {
        playerSonorEffectsAS.clip = impactAC;
        playerSonorEffectsAS.pitch = Random.Range(0.9f, 1.1f);
        playerSonorEffectsAS.Play();
        impactDmg.Play();

        if (SelfPlayerObject.InstanciedShield == null)
        {
            ForwardSpeed -= ObstacleSpeedReduce.Value;
        }
        else
        {
            Destroy(SelfPlayerObject.InstanciedShield);
            SelfPlayerObject.SelfBonusObject = PlayerObject.BonusObjects.Nothing;
        }
    }

    private void Aspiration()
    {
        Debug.DrawRay(SelfPlayerTransform.position, SelfPlayerTransform.forward * AspirationDistance.Value, Color.blue);
        if (Physics.Raycast(SelfPlayerTransform.position, SelfPlayerTransform.forward, AspirationDistance.Value, AspirationMask))
        {
            if (!Physics.Raycast(SelfPlayerTransform.position, SelfPlayerTransform.forward, AspirationDistance.Value, ObstacleMask))
            {
                if (AspirationEnableTime < TimeForEnableAspiration.Value && !IsOnAspiration)
                {
                    AspirationEnableTime += Time.deltaTime;
                }
            }
            else
            {
                if (AspirationEnableTime >= 0)
                {
                    AspirationEnableTime -= Time.deltaTime;
                }
                if (IsOnAspiration)
                {
                    AspirationDisableTime += Time.deltaTime;
                }
            }
        }
        else
        {
            if (AspirationEnableTime >= 0)
            {
                AspirationEnableTime -= Time.deltaTime;
            }
            if (IsOnAspiration)
            {
                AspirationDisableTime += Time.deltaTime;
            }
        }

        if(AspirationEnableTime >= TimeForEnableAspiration.Value)
        {         
            IsOnAspiration = true;
            playerAnimator.SetBool("in_asp", IsOnAspiration);
            playerAnimator02.SetBool("in_asp", IsOnAspiration);
            AspirationEnableTime = 0;

            playerSonorEffectsAS.clip = aspirationAC;
            playerSonorEffectsAS.pitch = 1f;
            playerSonorEffectsAS.Play();

            fxBoostAspi.Play();
            impactBoost.Play();
        }
      

        if (AspirationDisableTime >= TimeForDisableAspiration.Value)
        {
            IsOnAspiration = false;
            playerAnimator.SetBool("in_asp", IsOnAspiration);
            playerAnimator02.SetBool("in_asp", IsOnAspiration);
            AspirationDisableTime = 0;

            playerSonorEffectsAS.Stop();
            fxBoostAspi.Stop();
           
        }

        if (IsOnAspiration)
        {
            ForwardSpeed = Mathf.Lerp(ForwardSpeed, MaxAspirationSpeed.Value, AspirationIncresseValue.Value);
           
        }
    }
}