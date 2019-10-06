using UnityEngine;

public class ShootController : MonoBehaviour
{
    public FloatObject ShootSpeed;
    public FloatObject ShootImpactSpeedReduce;
    private Rigidbody SelfRigidbody;
    private Transform SelfTransform;

    private void Awake()
    {
        SelfRigidbody = GetComponent<Rigidbody>();
        SelfTransform = GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        SelfRigidbody.velocity = SelfTransform.up * ShootSpeed.Value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.GetComponent<PlayerObject>().InstanciedShield == null)
            {
                other.GetComponentInParent<PlayerAcceleration>().ForwardSpeed -= ShootImpactSpeedReduce.Value;
                Destroy(other.GetComponent<PlayerObject>().InstanciedShield);
                other.GetComponent<PlayerObject>().SelfBonusObject = PlayerObject.BonusObjects.Nothing;
                Destroy(gameObject);
            }
        }
        Destroy(gameObject);
    }
}