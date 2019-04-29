using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    // Use this for initialization

    public enum TutorialState
    {
        None,
        SliceHorizontal,
        SliceLeftDiagonally,
        SliceRightDiagonally,
        SliceVertically,
        BasicEnd,
        Mark,
      
        Jump,
        Focus,
        FocusProjectile,
        FocusGain,
        HitWall,



        End
    }

    public enum InputType
    {
        Keyboard,
        Controller
    }


    public InputType m_InputType;
    public TutorialState m_TutorialState;


    public Text m_TutorialTextDescription;
    public Text m_TutorialTextControls;

    public TriggerBoxObject m_FallDamageTrigger;

    public GameObject m_TutorialCanvas;
    public GameObject m_TestCubes;
    public GameObject m_TestWall;
    public GameObject m_Player;
    public GameObject m_ScafoldingBlocker;

    public Player m_PlayerScr;
    public PlayerMovementController m_PlayerController;
    public FPSCameraController m_FpsCameraController;
    public PlayerCombatManager m_PlayerCombatManager;
    void Start()
    {
        m_TutorialState = TutorialState.SliceHorizontal;
       // m_Player = GameObject.Find("Player");
        m_PlayerController = m_Player.GetComponent<PlayerMovementController>();
        //m_PlayerController.m_playerMovementDissabled = true;
          m_InputType = InputType.Keyboard;

        Services.AudioManager.PlayMusic(Music.CourtyardTheme);
        Services.AudioManager.SetMusicLooping(true,"Global");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
           
            m_InputType = InputType.Controller;

            //////Debug.Log(Input.GetJoystickNames().Length);
        }
        else
        {
            m_InputType = InputType.Keyboard;
        }
       

        if (m_TutorialState == TutorialState.SliceHorizontal)
        {

            if (m_InputType == InputType.Keyboard)
            {
                m_TutorialTextControls.text = "Hold down the <b>left mouse button</b> to get into Aiming Mode. \n In Aiming Mode, drag the mouse in the direction you want to slash and let go.";
            }
            if (m_InputType == InputType.Controller)
            {
                m_TutorialTextControls.text = "Hold down the <b>right trigger button</b> to get into Aiming Mode. \n In Aiming Mode, drag the joystick in the direction you want to slash and let go.";
            }

            //m_TutorialTextDescription.text = "Slice The Vertical Target";

        }

        if (m_TutorialState == TutorialState.SliceLeftDiagonally)
        {
            if (m_InputType == InputType.Keyboard)
            {
                m_TutorialTextControls.text = "Hold down the <b>left mouse button</b> to get into Aiming Mode. \n In Aiming Mode, drag the mouse in the direction you want to slash and let go.";
            }
            if (m_InputType == InputType.Controller)
            {
                m_TutorialTextControls.text = "Hold down the <b>right trigger button</b> to get into Aiming Mode. \n In Aiming Mode, drag the joystick in the direction you want to slash and let go.";
            }

            //m_TutorialTextDescription.text = "Slice The Diagonal Target";
        }

        if (m_TutorialState == TutorialState.SliceRightDiagonally)
        {
            if (m_InputType == InputType.Keyboard)
            {
                m_TutorialTextControls.text = "Hold down the <b>left mouse button</b> to get into Aiming Mode. \n In Aiming Mode, drag the mouse in the direction you want to slash and let go.";
            }
            if (m_InputType == InputType.Controller)
            {
                m_TutorialTextControls.text = "Hold down the <b>right trigger button</b> to get into Aiming Mode. \n In Aiming Mode, drag the joystick in the direction you want to slash and let go.";
            }

            //m_TutorialTextDescription.text = "Slice The Horizontal Target";
        }

        if (m_TutorialState == TutorialState.SliceVertically)
        {
            if (m_InputType == InputType.Keyboard)
            {
                m_TutorialTextControls.text = "Hold down the <b>left mouse button</b> to get into Aiming Mode. \n In Aiming Mode, drag the mouse in the direction you want to slash and let go.";
            }
            if (m_InputType == InputType.Controller)
            {
                m_TutorialTextControls.text = "Hold down the <b>right trigger button</b> to get into Aiming Mode. \n In Aiming Mode, drag the joystick in the direction you want to slash and let go.";
            }

            //m_TutorialTextDescription.text = "Slice The Diagonal Target";
        }

        if (m_TutorialState == TutorialState.BasicEnd)
        {
            m_PlayerController.m_playerMovementDissabled = false;
            if (m_InputType == InputType.Keyboard)
            {
                m_TutorialTextControls.text = "";
            }
            if (m_InputType == InputType.Controller)
            {
                m_TutorialTextControls.text = "";
            }

            m_TutorialTextDescription.text = "";
        }

        if (m_TutorialState == TutorialState.FocusGain)
        {
            if (m_InputType == InputType.Keyboard)
            {
                m_TutorialTextControls.text = "Press <b>Tab</b> to lock on/unlock to an enemy. \n Attacking an enemy builds up your <color=#DA70D6>Focus</color>.";
            }
            if (m_InputType == InputType.Controller)
            {
                m_TutorialTextControls.text = "Press down on the <b>right joystick</b> to lock on/unlock to an enemy. \n Attacking an enemy builds up your <color=#DA70D6>Focus</color>.";
            }

           // m_TutorialTextDescription.text = "Attack this Dummy to gain Focus."; // Down on Right Joystick
        }


        if (m_TutorialState == TutorialState.Jump)
        {
            if (m_InputType == InputType.Keyboard)
            {
                m_TutorialTextControls.text = "Press <b>Spacebar</b> to jump and <b>Shift</b> to dash. \n You can dash midjump to perform a jump dash. \n Dashing uses up your <color=#7CCD7C>Stamina</color>.";
            }
            if (m_InputType == InputType.Controller)
            {
                m_TutorialTextControls.text = "Press <b>A</b> to jump and <b>Left bumper</b> to dash. \n You can dash midjump to perform a jump dash. \n Dashing uses up your <color=#7CCD7C>Stamina</color>.";
            }

           // m_TutorialTextDescription.text = "Jump Over the broken bridge"; // A & RB
        }

        if (m_TutorialState == TutorialState.Mark)
        {
            if (m_InputType == InputType.Keyboard)
            {
                m_TutorialTextControls.text = "You can use <color=#DA70D6>Focus</color> to mark an enemy. \n Press <b>'E'</b> to place a mark. \n Slash all targets on a marked enemy to do critical damage.";
            }
            if (m_InputType == InputType.Controller)
            {
                m_TutorialTextControls.text = "You can use <color=#DA70D6>Focus</color> to mark an enemy. \n Press <b>Y</b> to place a mark. \n Slash all targets on a marked enemy to do critical damage.";
            }

            //m_TutorialTextDescription.text = "Follow the directions shown to destroy the Mark"; // Y
        }


        if (m_TutorialState == TutorialState.Focus)
        {
            if (m_InputType == InputType.Keyboard)
            {
                m_TutorialTextControls.text = " Attacking an enemy builds up your <color=#DA70D6>Focus</color>. \n <color=#DA70D6>Focus</color> can be used to heal you. Press <b>'T'</b> to heal.";
            }
            if (m_InputType == InputType.Controller)
            {
                m_TutorialTextControls.text = " Attacking an enemy builds up your <color=#DA70D6>Focus</color>. \n <color=#DA70D6>Focus</color> can be used to heal you. Press the <b>left trigger</b> to heal.";
            }

            //m_TutorialTextDescription.text = "You took damage, \n Use your purple Focus to heal you"; // left trigger
        }

        if (m_TutorialState == TutorialState.FocusProjectile)
        {
            if (m_InputType == InputType.Keyboard)
            {
                m_TutorialTextControls.text = "Press E to use a projectile";
            }
            if (m_InputType == InputType.Controller)
            {
                m_TutorialTextControls.text = "Press Y to use a projectile made of focus";
            }

            //m_TutorialTextDescription.text = "Shoot the chains holding the drawbridge";
        }

        // If the player falls into the damage trigger
        //if(m_FallDamageTrigger.m_IsPlayerinside)
        //{
            // Deal damage to the player and disable the trigger
            //m_PlayerCombatManager.dealdamage(20);
          //  m_FallDamageTrigger.enabled = false;
        //}



        if (m_TutorialState == TutorialState.End)
        {

            m_TutorialCanvas.SetActive(false);
            m_TutorialTextControls.text = "";
            m_TutorialTextDescription.text = "";
        }

    }


    public void SetTutorialState(TutorialState aTutorialState)
    {
        m_TutorialState = aTutorialState;
    }


    public TutorialState GetTutorialState()
    {
        return m_TutorialState;
    }
}
