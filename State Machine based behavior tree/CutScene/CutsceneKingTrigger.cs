using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneKingTrigger : MonoBehaviour
{

    public Animator m_CameraAnimator;
    Timer m_CutsceneTime;
    Timer m_TurnFireOff;

    public GameObject m_King;
    public CuttSceneKing m_KingCutscene;
    public GameObject m_PlayerCamera;
    public GameObject m_CutsceneCamera;
    public GameObject m_Player;
    public GameObject m_Canvas;
    public GameObject m_BehindFire;

    public ParticleSystem m_KingFire;

    bool m_EnteredTrigger;
    // Use this for initialization
    void Start()
    {
        m_CutsceneTime = Services.TimerManager.CreateTimer("m_Cutscene2Timer", 5, false);
        m_TurnFireOff = Services.TimerManager.CreateTimer("m_Cutscene3Timer", 2, false);
        m_EnteredTrigger = false;
        m_CutsceneCamera.SetActive(false);
        m_Canvas.SetActive(true);
        m_KingCutscene.gameObject.SetActive(false);
        m_King.SetActive(false);
        Services.AudioManager.PlayMusic(Music.RoderickTheme);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CutsceneTime.OnFinish())
        {
            //Deactivating things for the cutscene
            m_KingCutscene.gameObject.SetActive(false);
            m_CutsceneCamera.SetActive(false);

            //Turning on things for after cutscene
            m_King.SetActive(true);
            
            m_Player.SetActive(true);
            m_Canvas.SetActive(true);
            m_BehindFire.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_CutsceneTime.IsRunning() == true)
            {
                m_Player.SetActive(true);
                m_KingCutscene.gameObject.SetActive(false);
                m_CutsceneCamera.SetActive(false);
                m_Canvas.SetActive(true);
                m_King.SetActive(true);
                m_BehindFire.SetActive(true);
            }
        }

        if(m_TurnFireOff.OnFinish())
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<ParticleSystem>() != null)
                {
                    child.GetComponent<ParticleSystem>().Stop();
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (m_EnteredTrigger == false)
            {
                Services.GameManager.Player.SetCurrentHealthAsMaxHealth();

                m_EnteredTrigger = true;
                m_CutsceneCamera.transform.position = new Vector3(151.8f, 97.58f, -172.99f);
                m_CutsceneCamera.SetActive(true);
                m_CutsceneCamera.GetComponent<Animator>().SetTrigger("t_KingOpening");
               
                m_Player.SetActive(false);
                m_Canvas.SetActive(false);
                m_KingCutscene.gameObject.SetActive(true);
                // m_CutsceneOdell.transform.rotation = Quaternion.Euler(m_CutsceneOdell.transform.rotation.x, 180, m_CutsceneOdell.transform.rotation.z);

                m_TurnFireOff.StartTimer();
                m_CutsceneTime.StartTimer();
            }
        }
    }
}
