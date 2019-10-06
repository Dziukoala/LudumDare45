using UnityEngine;

public class PlayerCollisionTrigger : MonoBehaviour
{
    private PlayerAcceleration SelfPlayerAcceleration;
    private PlayerObject SelfPlayerObject;

    private void Awake()
    {
        SelfPlayerAcceleration = GetComponentInParent<PlayerAcceleration>();
        SelfPlayerObject = GetComponentInChildren<PlayerObject>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Boost"))
        {
            SelfPlayerAcceleration.Boost();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            SelfPlayerAcceleration.Obstacle();
        }
        if (other.CompareTag("Bonus"))
        {
            SelfPlayerObject.Bonus();
        }
    }
}