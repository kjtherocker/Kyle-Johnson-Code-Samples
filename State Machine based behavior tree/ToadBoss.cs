using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToadBoss : Enemy
{
    float m_HealthSave;
    public bool m_HasBeenHit;
    string m_BossName;
    public Text m_BossText;
    public Image m_Image_Healthbar;

    public GameObject TargetForComboFront;
    public GameObject TargetForComboLeft;
    public GameObject TargetForComboRight;

    public GameObject m_CutsceneController;

    public GameObject m_PlayerCutsceneTeleportPoint;

    Timer m_OdellDeathCutscene;

    Animator m_Animator;

    public GameObject m_Canvas;

    public GameObject m_OdellExit;

    public GameObject m_ThroneRoomCollider;

    public GameObject m_BossCanvas;

    public override void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();

        m_HealthBarSize.Set(4.217635f, 1.128427f, 0.2941727f);
        //m_BossText = GameObject.Find("Text_BossName").GetComponent<Text>();
        m_HasBeenHit = false;
        m_MaxHealth = 500;
        m_CurrentHealth = 500;
        Immortality = false;
        m_BossName = "Odell The Abomination";

        m_OdellDeathCutscene = Services.TimerManager.CreateTimer("m_CutsceneTimer", 12.5f, false);

    }

    

    public override void Update()
    {
        base.Update();

        UpdateHealthBar();

        if (Immortality)
        {
            //Debug.Log("KILLME");
        }

        if (m_Marked == true && m_CurrentHealth !=0.0f)
        {

            if (!TargetForComboFront.GetComponent<ComboTarget>().m_comboCompleted)
            {
                TargetForComboFront.SetActive(true);

            }
            if (!TargetForComboLeft.GetComponent<ComboTarget>().m_comboCompleted)
            {
                TargetForComboLeft.SetActive(true);

            }
            if (!TargetForComboRight.GetComponent<ComboTarget>().m_comboCompleted)
            {
                TargetForComboRight.SetActive(true);

            }

            if (TargetForComboFront.GetComponent<ComboTarget>().NumOfStrikes > 0)
            {
                // TargetForComboFront.SetActive(true);
                TargetForComboLeft.SetActive(false);
                TargetForComboRight.SetActive(false);

            }
            if (TargetForComboLeft.GetComponent<ComboTarget>().NumOfStrikes > 0)
            {
                TargetForComboFront.SetActive(false);
                // TargetForComboLeft.SetActive(true);
                TargetForComboRight.SetActive(false);
            }
            if (TargetForComboRight.GetComponent<ComboTarget>().NumOfStrikes > 0)
            {
                TargetForComboFront.SetActive(false);
                TargetForComboLeft.SetActive(false);
                // TargetForComboRight.SetActive(true);
            }

        }

        if(m_CurrentHealth <= 0)
        {          
            m_Animator.SetBool("Dead", true);
        }


        if(m_OdellDeathCutscene.OnFinish())
        {
            gameObject.SetActive(false);
            m_Canvas.SetActive(true);
            m_CutsceneController.SetActive(false);
            Services.GameManager.Player.GetComponent<PlayerMovementController>().m_playerMovementDissabled = false;
        }

       
    }

    protected override void OnDeath()
    {
        m_Animator.SetBool("Dead", true);

        GetComponent<AIController>().enabled = false;

        Services.AudioManager.PlayMusic(Music.CourtyardTheme);

        m_BossCanvas.gameObject.SetActive(false);
        m_ThroneRoomCollider.gameObject.SetActive(false);
        m_OdellExit.gameObject.SetActive(false);
        m_Canvas.SetActive(false);
        Services.GameManager.Player.gameObject.transform.position = m_PlayerCutsceneTeleportPoint.transform.position;
        Services.GameManager.Player.GetComponent<PlayerMovementController>().m_playerMovementDissabled = true;
        //Services.GameManager.PlayerWon = true;

        m_OdellDeathCutscene.StartTimer();

        m_CutsceneController.transform.position = new Vector3(-8.72f, 53.35f, -20.59f);
        
        m_CutsceneController.SetActive(true);
        m_CutsceneController.GetComponent<Animator>().SetTrigger("t_OdellDeath");

        Services.GameManager.Odeldefeated = true;

        Alive = false;

       // Services.UIManager.PushScreen(UIManager.Screen.GameEnd);
    }

    protected override void OnHit()
    {

        if (m_CurrentHealth > 0.0f)
        {
            m_HasBeenHit = true;

            GameObject playerObject = Services.GameManager.Player.gameObject;

            Vector3 Direction = playerObject.transform.position - transform.position;
            Direction.Normalize();

            Vector3 Position = transform.position + Direction * 2f;

            Instantiate(m_HitEffectPrefab, Position,  Quaternion.identity);
        }

    }

    public void SetHit(bool set)
    {
        m_HasBeenHit = set;
    }

    void UpdateHealthBar()
    {
        //Checks to see if the current health is equal to or less then 0
        if (m_CurrentHealth <= 0)
        {
            //Sets health to 0
            m_CurrentHealth = 0;
        }

        //Checks to see if the Current health is not greater then the Maximum health
        if (m_CurrentHealth >= m_MaxHealth)
        {
            //Sets m_Currenthealth to Max health if it is going over
            m_CurrentHealth = m_MaxHealth;
        }
        //   m_BossText.text = m_BossName;
        //Gets the ratio of the healths maximum possible and current.
        float HealthRatio = (float)m_CurrentHealth / (float)m_MaxHealth;

        if (m_Image_Healthbar != null)
        {
            m_Image_Healthbar.fillAmount = HealthRatio;
            //Sets the Foreground health bar image to the scale of the health ratio
            // m_Image_Healthbar.rectTransform.localScale = new Vector3(HealthRatio * m_HealthBarSize.x, m_HealthBarSize.y, m_HealthBarSize.z);
        }
    }

    public float GetCurrentHealth()
    {
        return m_CurrentHealth;
    }

    public override void TakeDamage(float aValue)
    {
        base.TakeDamage(aValue);

        Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Hurt_New);
    }
}

//public void CollidedWithTongue()
//{
//    if (m_ExecuteAction == EnemyAction.TongueWhip)
//    {
//        Services.GameManager.Player.TakeDamage(m_TongueDamage);
//    }
//    if (m_ExecuteAction == EnemyAction.TongueGrab)
//    {
//        TongueGrabHit();
//    }
//}
//}


