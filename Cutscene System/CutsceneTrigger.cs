using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour {

    GameManager m_GameManager;
    bool m_HasEnteredTrigger;

    public CutsceneController.Cutscenes m_Cutscenetoplay;

	// Use this for initialization
	void Start ()
    {
        m_GameManager = GameObject.FindObjectOfType<GameManager>();

        m_HasEnteredTrigger = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (m_HasEnteredTrigger == false)
        {
            if (other.tag == "Player")
            {
                m_HasEnteredTrigger = true;
                m_GameManager.cutsceneController.SetVideoClip(m_Cutscenetoplay);
            }
        }
    }
}
