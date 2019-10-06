using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public enum BonusObjects
    {
        Nothing,
        Shield,
        Shoot
    }

    public BonusObjects SelfBonusObject;
    [HideInInspector] public GameObject InstanciedShield;
    private InputManager SelfInputManager;
    public GameObject ShieldPrefab;
    public GameObject ShootPrefab;
    private Transform SelfTransform;

    public AudioSource playerSonorEffectsAS;
    public AudioClip shieldAC;
    public AudioClip shootAC;
    public AudioClip powerUpAC;

    private void Awake()
    {
        SelfTransform = GetComponent<Transform>();
        SelfInputManager = GetComponentInParent<InputManager>();
    }
    public void Bonus()
    {
        playerSonorEffectsAS.clip = powerUpAC;
        playerSonorEffectsAS.pitch = 1f;
        playerSonorEffectsAS.Play();


        if (SelfBonusObject == BonusObjects.Nothing)
        {
            SelfBonusObject = (BonusObjects)Random.Range(1, 3);
        }
    }

    private void Update()
    {
        if (SelfInputManager.UseObject())
        {
            if(SelfBonusObject == BonusObjects.Shield)
            {
                Shield();
            }
            if(SelfBonusObject == BonusObjects.Shoot)
            {
                Shoot();
            }
        }
    }

    private void Shield()
    {
        playerSonorEffectsAS.clip = shieldAC;
        playerSonorEffectsAS.pitch = 1f;
        playerSonorEffectsAS.Play();

        InstanciedShield = Instantiate(ShieldPrefab, SelfTransform);
    }

    private void Shoot()
    {
        playerSonorEffectsAS.clip = shootAC;
        playerSonorEffectsAS.pitch = 1f;
        playerSonorEffectsAS.Play();

        Transform newShoot = Instantiate(ShootPrefab).GetComponent<Transform>();
        newShoot.position = SelfTransform.position + SelfTransform.up * 7;
        newShoot.rotation = SelfTransform.rotation;
        SelfBonusObject = BonusObjects.Nothing;
    }
}