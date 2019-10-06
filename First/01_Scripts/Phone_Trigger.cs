using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone_Trigger : MonoBehaviour
{
    [Header("Texture Settings")]
    public Texture m_MainTexture;
    public Texture m_MainTextureDad;
    private Renderer m_Renderer;

    [Space(10)]
    [Header("Sound Settings")]
    public AudioClip Vibrate;
    public AudioClip GirlCallVoice;
    public AudioClip DadCallVoice;
    private AudioSource m_audioSource;

    private Animation m_animation;
    private bool islunch = true;


    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_audioSource = GetComponent<AudioSource>();
        m_animation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Evenement());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(islunch == true)
        {
            StartCoroutine(Evenement());
        }

    }

    void DadCall()
    {
        m_Renderer.material.SetTexture("_MainTex", m_MainTextureDad);
    }

    void CallClose()
    {
        m_Renderer.material.SetTexture("_MainTex", m_MainTexture);
    }

    IEnumerator Evenement()
    {
        Debug.Log("Animation");
        islunch = false;

        for (int i = 0; i < 4; i++)
        {
            m_audioSource.clip = Vibrate;
            m_audioSource.Play();
            m_animation.Play();
            DadCall();
            yield return new WaitForSeconds(1f);
            CallClose();

            yield return new WaitForSeconds(1.5f);
        }

        DadCall();
        m_audioSource.clip = GirlCallVoice;
        m_audioSource.Play();

        yield return new WaitForSeconds(GirlCallVoice.length);

        yield return new WaitForSeconds(2f);
        m_audioSource.clip = DadCallVoice;
        m_audioSource.Play();

    }
}
