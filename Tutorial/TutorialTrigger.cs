using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    // Use this for initialization
    public enum TutorialTriggerTypes
    {
        TutorialText,
        TutorialDamage,
        TutorialDestroy,
        TutorialRotate
    }

    public GameObject m_MovePosition;
    public TutorialTriggerTypes m_TutorialTrigger;
    public Tutorial.TutorialState m_TutorialState;
    Tutorial m_Tutorial;
    bool m_HasBeenEntered;
	void Start ()
    {
        m_Tutorial = GameObject.FindObjectOfType<Tutorial>();
        m_HasBeenEntered = false;
      
    }
	
	// Update is called once per frame
	void Update ()
    {
 
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (m_HasBeenEntered == false)
        {
            if (other.tag == "Player")
            {
                if (m_TutorialTrigger == TutorialTriggerTypes.TutorialText)
                {
                    m_Tutorial.SetTutorialState(m_TutorialState);
                    m_HasBeenEntered = true;
                }
                if (m_TutorialTrigger == TutorialTriggerTypes.TutorialDamage)
                {
                    m_Tutorial.m_PlayerScr.TakeDamage(260);
                    if (m_TutorialState != Tutorial.TutorialState.None)
                    {
                        m_Tutorial.SetTutorialState(m_TutorialState);
                    }
                    
                    m_HasBeenEntered = true;
                }
                
             }

            if (other.tag == "Projectile")
            {
                if (m_TutorialTrigger == TutorialTriggerTypes.TutorialDestroy)
                {
                    Destroy(gameObject);
                    m_HasBeenEntered = true;
                }
            }

        }
    }


}
