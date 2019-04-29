using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class DebugMenu : MonoBehaviour
{


    //List of MenuScreens Can be popped to go back to the last screen
    public List<GameObject> m_MenuScreens;

    Player m_Player;
    PlayerCombatManager m_PlayerCombatManager;
    //Prefab of the button we want to spawn
    public DebugButton m_Prefab_DebugButton;
    //DebugMenu Canvas to be spawned 
    public GameObject m_PrefabCanvasDebugMenu;

    public GameManager m_GameManager;

    public void Start()
    {
        // m_ToadReference 
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        //Instantiates the inital menu
        // Checking To see if the i key is down
        if (Input.GetKeyDown("i"))
        {
            //if the menuscreen is 0 then it is empty if its empty then
            if (m_MenuScreens.Count == 0)
            {
                m_GameManager.CurrentGameState = GameState.Menu;
                Services.GameManager.SetCursorVisibility(true);
                //Add the menu screen to the list 
                m_MenuScreens.Add(Instantiate<GameObject>(m_PrefabCanvasDebugMenu));

                
                m_Player = Services.GameManager.Player;

                m_PlayerCombatManager = GameObject.Find("Player").GetComponent<PlayerCombatManager>();

                if (m_Player != null)
                {
                    //Instantiate the button ontop of the last instantiated canvas
                    DebugButton Debugbutton1 = Instantiate<DebugButton>(m_Prefab_DebugButton, m_MenuScreens[0].gameObject.transform);
                    //Setting the position of the instantiated button
                    Debugbutton1.gameObject.transform.localPosition = new Vector3(170, 100, 0);
                    //Setting the initalization method
                    Debugbutton1.Init("Player Ui", this, DebugButton.DebugButtonType.DebugMenutype, SpawnPlayerCanvas);
                }
            }
        }

        //TODO: Switch button To something else
        // Destroys the whole Debugmenu setup
        if (Input.GetKeyDown("o"))
        {
            Services.GameManager.SetCursorVisibility(false);
            m_GameManager.CurrentGameState = GameState.Playing;
            for (int i = m_MenuScreens.Count; i > 0; i--)
            {

                Destroy(m_MenuScreens[0]);
                m_MenuScreens.RemoveAt(0);

            }
        }

        //TODO: Switch button to something else
        //Pops the top most part of the debugmenu setup
        if (Input.GetKeyDown("p"))
        {

            Destroy(m_MenuScreens[m_MenuScreens.Count - 1]);
            m_MenuScreens.RemoveAt(m_MenuScreens.Count - 1);
            if (m_MenuScreens.Count == 0)
            {
                m_GameManager.CurrentGameState = GameState.Playing;
            }
        }

    }

    public void SpawnPlayerCanvas()
    {
        m_MenuScreens.Add(Instantiate<GameObject>(m_PrefabCanvasToadBoss));
    
        DebugButton DebugButton = Instantiate<DebugButton>(m_Prefab_DebugButton, m_MenuScreens[m_MenuScreens.Count - 1].gameObject.transform);
        DebugButton.gameObject.transform.localPosition = new Vector3(280, 200 , 0);
        DebugButton.Init("Immortality On", this, DebugButton.DebugButtonType.DebugMenutype, m_Player.SetImmortalityToTrue);

        DebugButton DebugButton2 = Instantiate<DebugButton>(m_Prefab_DebugButton, m_MenuScreens[m_MenuScreens.Count - 1].gameObject.transform);
        DebugButton2.gameObject.transform.localPosition = new Vector3(280, 175, 0);
        DebugButton2.Init("Immortality Off", this, DebugButton.DebugButtonType.DebugMenutype, m_Player.SetImmortalityToFalse);

        DebugButton DebugButton3 = Instantiate<DebugButton>(m_Prefab_DebugButton, m_MenuScreens[m_MenuScreens.Count - 1].gameObject.transform);
        DebugButton3.gameObject.transform.localPosition = new Vector3(280, 150, 0);
        DebugButton3.Init("Teleport to Position 1", this, DebugButton.DebugButtonType.DebugMenutype, m_PlayerCombatManager.TeleportToPosition1);

        DebugButton DebugButton4 = Instantiate<DebugButton>(m_Prefab_DebugButton, m_MenuScreens[m_MenuScreens.Count - 1].gameObject.transform);
        DebugButton4.gameObject.transform.localPosition = new Vector3(280, 125, 0);
        DebugButton4.Init("Teleport to Position 2", this, DebugButton.DebugButtonType.DebugMenutype, m_PlayerCombatManager.TeleportToPosition2);

        DebugButton DebugButton5 = Instantiate<DebugButton>(m_Prefab_DebugButton, m_MenuScreens[m_MenuScreens.Count - 1].gameObject.transform);
        DebugButton5.gameObject.transform.localPosition = new Vector3(280, 100, 0);
        DebugButton5.Init("Teleport to Position 3", this, DebugButton.DebugButtonType.DebugMenutype, m_PlayerCombatManager.TeleportToPosition3);

        DebugButton DebugButton6 = Instantiate<DebugButton>(m_Prefab_DebugButton, m_MenuScreens[m_MenuScreens.Count - 1].gameObject.transform);
        DebugButton5.gameObject.transform.localPosition = new Vector3(280, 75, 0);
        DebugButton5.Init("Teleport to Position 4", this, DebugButton.DebugButtonType.DebugMenutype, m_PlayerCombatManager.TeleportToPosition4);


    }
}

