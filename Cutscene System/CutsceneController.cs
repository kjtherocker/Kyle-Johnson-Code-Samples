using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.Events;

public class CutsceneController : MonoBehaviour
{

    public enum Cutscenes
    {
        KnightGetsAttacked,
        WereCoon,
        MechaSeanIntro,
        MechaSeanExit,
        CastleExitGoats,


        NumberOfcutscenes
    }
   
    [SerializeField] VideoClip[] m_Videoclips;

    public List<PlayerController> m_PlayerControllers;
    public bool CanSkipCutscene;
    public bool SkipCutscene;

    public bool m_StartSkipTimer;
    public bool m_CutsceneHasFinished;
    public bool HaveSkipTextAppear;
    public bool m_SkipObjectsAreEnabled;

    public float SkipHeld;
    public float m_SkipObjectsFadeTimer;
    public float m_ColorTimer;
    bool CutsceneEnd;
    public bool m_CutsceneisDone;
    bool m_FadeToBlack;
    public bool m_CutsceneFadeIsFinished;

    Coroutine m_AButtonFade;
    Coroutine m_SkipTextFade;


    Color TransparentBlack;

    public RawImage m_rawImage;

    public GameObject m_SkipObjects;
    public RawImage m_AButton;
    public Text m_SkipText;

    public AudioSource audioSource;
    public AudioMixer audioMixer;

    public VideoPlayer m_CutsceneVideoPlayer;
	// Use this for initialization
	void Start ()
    {
        System.Array.Resize(ref m_Videoclips, (int)Cutscenes.NumberOfcutscenes);

        CanSkipCutscene = false;
        m_CutsceneHasFinished = false;
        m_CutsceneisDone = false;
        m_ColorTimer = 0;
        SkipHeld = 0;

        TransparentBlack = new Color(0, 0, 0, 0);

        m_CutsceneVideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        m_CutsceneVideoPlayer.EnableAudioTrack(0, true);
        m_CutsceneVideoPlayer.SetTargetAudioSource(0, audioSource);
        
        
        m_rawImage.texture = m_CutsceneVideoPlayer.texture;

        SetVideoClip(Cutscenes.KnightGetsAttacked);
    }


    public void SetVideoClip(Cutscenes cutscenes, string sourceName = "Global")
    {
        StopAllCoroutines();
       
        m_SkipObjectsFadeTimer = 0;
        if (Constants.Constants.TurnCutscenesOff == false)
        {
            m_CutsceneHasFinished = false;
            audioMixer.SetFloat("MusicVolume", -100);
            GameManager.Instance.m_CutsceneISRunning = true;
            m_rawImage.gameObject.SetActive(true);
            m_CutsceneVideoPlayer.clip = m_Videoclips[(int)cutscenes];
            m_rawImage.texture = m_CutsceneVideoPlayer.texture;
            StartCoroutine(PlayVideo());
        }
        else if (Constants.Constants.TurnCutscenesOff == true)
        {
            m_CutsceneHasFinished = true;
            m_CutsceneisDone = true;
            m_rawImage.gameObject.SetActive(false);
            m_CutsceneVideoPlayer.Stop();
        }
    }

    public IEnumerator PlayVideo()
    {

        m_CutsceneVideoPlayer.Prepare();
        m_PlayerControllers = GameManager.Instance.playerControllers;
        GameManager.Instance.m_CutsceneISRunning = true;
        
            foreach (PlayerController aPlayercontroller in m_PlayerControllers)
         {
             aPlayercontroller.GetComponent<PlayerController>().ClearAnimationTriggers();
             aPlayercontroller.GetComponent<PlayerController>().isMoving = false;
             aPlayercontroller.GetComponent<PlayerController>().isStunned = true;
         }
        
        StartCoroutine(FadeToCutscene(color =>
                   m_rawImage.color = color));

        while (!m_CutsceneVideoPlayer.isPrepared)
         {
            
            Debug.Log("Preparing Video");
             yield return null;
         }

        CanSkipCutscene = true;

        

        Debug.Log("Done Preparing Video");
         m_rawImage.gameObject.SetActive(true);
         m_rawImage.texture = m_CutsceneVideoPlayer.texture;
         m_CutsceneVideoPlayer.Play();
         audioSource.Play();
       
         m_StartSkipTimer = false;

        while (m_CutsceneVideoPlayer.isPlaying)
        {
           
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)m_CutsceneVideoPlayer.time));
            yield return null;
        }
        
        //StartCoroutine(FadeToCutscene(result => m_rawImage.color = result));
        m_CutsceneisDone = true;
        Debug.Log("Done Playing Video");
    }

    public IEnumerator FadeToCutscene(System.Action<Color> a_color)
    {
        
        float colortimer = 0;
        Color aTest;

        while (colortimer <= 1.0)
        {
            
            colortimer += Time.deltaTime / 2;
            aTest = new Color(colortimer, colortimer, colortimer, colortimer);
            yield return new WaitForSeconds(0.005F);
            a_color(aTest);

        }
        yield break;
    }

    public IEnumerator FadeToGame(System.Action<Color> a_color)
    {
        float colortimer = 1;
        Color aTest;

        while (colortimer > -0.05f)
        {

            colortimer -= Time.deltaTime / 2;
            aTest = new Color(1, 1, 1, colortimer);
            yield return new WaitForSeconds(0.005F);
            a_color(aTest);

        }
        yield break;
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_CutsceneisDone == true)
        {
            OnVideoEnd();
        }

      


        if (CanSkipCutscene == true)
        {
            if (Input.anyKeyDown)
            {
                if (m_SkipObjectsAreEnabled == false)
                {
                    m_SkipObjects.SetActive(true);
                    m_SkipObjectsAreEnabled = true;
                }
            }

            if (Input.GetButtonDown("A_2") || Input.GetButtonDown("A_1") || Input.GetButtonDown("A_3") || Input.GetButtonDown("A_4"))
                {
                if (m_AButtonFade != null)
                {
                    StopCoroutine(m_AButtonFade);
                }

                if (m_SkipTextFade != null)
                {
                    StopCoroutine(m_SkipTextFade);
                }

                m_SkipText.color = new Color(1, 1, 1, 1);
                m_AButton.color = new Color(1, 1, 1, 1);

                m_StartSkipTimer = true;
                m_SkipObjectsFadeTimer = 0;
            }
            else if (Input.GetButtonUp("A_2") || Input.GetButtonUp("A_1") || Input.GetButtonUp("A_3") || Input.GetButtonUp("A_4"))
            {
                m_StartSkipTimer = false;
            }

            if (m_StartSkipTimer == true)
            {

                if (m_AButtonFade != null)
                {
                    StopCoroutine(m_AButtonFade);
                }

                if (m_SkipTextFade != null)
                {
                    StopCoroutine(m_SkipTextFade);
                }

                m_SkipText.color = new Color(1, 1, 1, 1);
                m_AButton.color = new Color(1, 1, 1, 1);
                m_SkipObjectsFadeTimer = 0;
                SkipHeld += Time.deltaTime;
            }
            else if (m_StartSkipTimer == false)
            {

                SkipHeld = 0;
            }
            if (SkipHeld >= 3.0f)
            {
                m_CutsceneisDone = true;
            }

            if (m_SkipObjectsAreEnabled == true)
            {
                m_SkipObjectsFadeTimer += Time.deltaTime;
            }

            if (m_SkipObjectsFadeTimer >= 2)
            {
              
                m_AButtonFade = StartCoroutine(FadeToGame(color =>  m_AButton.color = color));

                m_SkipTextFade = StartCoroutine(FadeToGame(color => m_SkipText.color = color));
            }


            if (m_AButton.color.a <= -0.05)
            {
               
                m_SkipObjectsAreEnabled = false;
            }
        }

     

        if (m_FadeToBlack == true)
        {
            StartCoroutine(FadeToGame(color =>
                    m_rawImage.color = color));
        }

        if (m_rawImage.color.a >= 1)
        {
            m_CutsceneFadeIsFinished = true;
        }

        if (m_rawImage.color.a <= -0.05f)
        {
            m_rawImage.color = Color.black;
  
            m_ColorTimer = 0;
            m_CutsceneVideoPlayer.Stop();
            m_FadeToBlack = false;
            m_rawImage.gameObject.SetActive(false);
        }

    }

    public void OnVideoEnd()
    {
        SkipHeld = 0;
        m_ColorTimer = 1;
        m_FadeToBlack = true;

        m_SkipObjects.SetActive(false);
        m_CutsceneVideoPlayer.Pause();

        GameManager.Instance.m_CutsceneISRunning = false;
        foreach (PlayerController aPlayercontroller in m_PlayerControllers)
        {
            aPlayercontroller.GetComponent<PlayerController>().isStunned = false;
        }
        
        audioMixer.SetFloat("MusicVolume", 0.0f);
        CanSkipCutscene = false;
        m_CutsceneisDone = false;
        m_StartSkipTimer = false;
        m_SkipObjectsAreEnabled = false;
        m_CutsceneHasFinished = true;
    }
}
