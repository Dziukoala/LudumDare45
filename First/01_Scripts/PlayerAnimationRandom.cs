using UnityEngine;

public class PlayerAnimationRandom : MonoBehaviour
{
    private Animator SelfAnimator;
    public float TimeBeforeChangeAnim;
    private float Timeline;

    private void Awake()
    {
        SelfAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Timeline -= Time.deltaTime;
        if(Timeline <= 0)
        {
            Timeline = TimeBeforeChangeAnim;
            SelfAnimator.SetInteger("which_race", Random.Range(0, 4));
        }
    }
}