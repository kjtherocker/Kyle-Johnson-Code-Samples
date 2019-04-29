using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour
{
    public enum DebugButtonType
    {
        DebugMenutype
    }

    DebugButtonType m_DebugbuttonType;

    public Button m_ReferenceToButton;

    int m_Attacknumber;
    string m_Name;
    public Text m_ButtonText;
    DebugMenu m_DebugMenu;

    bool buttonwasClicked = false;

    ToadBoss m_ToadBossReference;

    public delegate void Method();
    public Method m_Method;

    public delegate void Methodint(int a_int);
    public Methodint m_Methodint;
    // Use this for initialization
    public void Init(string a_Name, DebugMenu a_DebugMenu, DebugButtonType a_DebugButtonType, Method a_Method)
    {

        m_Name = a_Name;
        m_ButtonText.text = a_Name;
        m_DebugMenu = a_DebugMenu;
        m_DebugbuttonType = a_DebugButtonType;
        m_Method = a_Method;
        m_ReferenceToButton.onClick.AddListener(() => m_Method());
    } 
}
