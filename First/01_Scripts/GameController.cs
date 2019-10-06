using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Image Foreground;
    public GameObject TutoUp;
    public GameObject TutoDown;
    public float TutoSpeed;
    private float TutoTimeline;
    public float TimeBeforeStart;
    public float TimeKillTuto;
    private float RStickStrength;
    private bool IsStarted;
    private float StartTimeline;
    public PlayerAcceleration[] Players;
    public AudioClip startAC;
    public AudioSource playerSonorEffectsAS;
    public AudioSource[] StartingAudioSources;

    private void Awake()
    {
        Time.timeScale = 0;
        StartTimeline = TimeBeforeStart;
    }

    private void Update()
    {
        TutoTimeline += Time.unscaledDeltaTime;
        if(TutoTimeline >= TutoSpeed && TutoUp != null)
        {
            TutoTimeline = 0;
            TutoDown.SetActive(!TutoDown.activeSelf);
        }

        RStickStrength += Mathf.Abs(Input.GetAxis("Joy1 RightYAxis") + Input.GetAxis("Joy2 RightYAxis") + Input.GetAxis("Joy3 RightYAxis") + Input.GetAxis("Joy4 RightYAxis"));

        if (RStickStrength > 500 && !IsStarted)
        {
            IsStarted = true;

            playerSonorEffectsAS.clip = startAC;
            playerSonorEffectsAS.pitch = 1f;
            playerSonorEffectsAS.Play();
        }

        if (IsStarted)
        {
            StartTimeline -= Time.unscaledDeltaTime;
        }

        if (StartTimeline <= TimeBeforeStart - TimeKillTuto && TutoUp != null)
        {
            Destroy(TutoUp);
            Destroy(TutoDown);
        }

        if(StartTimeline <= 1f && Foreground != null)
        {
            Color foregroundColor = Foreground.color;
            foregroundColor.a -= Time.unscaledDeltaTime;
            Foreground.color = foregroundColor;

            if(Foreground.color.a <= .05f)
            {
                Destroy(Foreground.gameObject);
            }
        }

        if(StartTimeline <= 1)
        {
            foreach(PlayerAcceleration PA in Players)
            {
                PA.ForwardSpeed = Random.Range(150, 160);
            }
            Time.timeScale = 1;
            foreach(AudioSource AS in StartingAudioSources)
            {
                AS.Play();
            }
        }

        if(StartTimeline <= 0)
        {
            Destroy(this);
        }
    }
}