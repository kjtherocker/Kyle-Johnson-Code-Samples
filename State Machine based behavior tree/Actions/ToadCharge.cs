using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadCharge : AIAction
{

    Vector3 m_TargetPosition;
    Vector3 m_PreJumpPosition;

    Timer m_ToadChargeTimer;
    Timer m_ToadChargeDelayTimer;
    Timer m_ToadHitTimer;
    Timer m_ToadLookTimer2;

    private bool m_Hit;
    private float m_ChargeDistanceBehindPlayer = 5.0f;

    bool m_Charging;

    public ToadCharge(AIController aAIController) : base(aAIController)
    {
        m_ToadChargeTimer = Services.TimerManager.CreateTimer("m_ToadChargeTimer", Constants.ChargeDuration, false);
        m_ToadChargeDelayTimer = Services.TimerManager.CreateTimer("m_ToadChargeDelayTimer", Constants.ChargeDelay, false);
        m_ToadHitTimer = Services.TimerManager.CreateTimer("m_ToadHitTimer", Constants.PushTimer, false);
        m_ToadLookTimer2 = Services.TimerManager.CreateTimer("m_ToadLookTimer2", 0.5f, false);
    }

    // Use this for initialization
    public override void Start()
    {
        // Set the toads PreCharge position
        m_PreJumpPosition = ((AIToadController)m_AIController).transform.position;

        // Set hit and charging to false
        m_Hit = false;
        m_Charging = false;

        ((AIToadController)m_AIController).EnableColliders(true);

        // Setup the timers
        m_ToadChargeDelayTimer.Restart();

        ((AIToadController)m_AIController).m_Animator.SetTrigger("Toad_Charge");      
    }

    // Update is called once per frame
    public override void Update()
    {
        // If the delay is done and the toad isin't charging
        if (m_ToadChargeDelayTimer.IsFinished() && m_Charging == false)
        {
            // Get the player watcher to look at the Player
            ((AIToadController)m_AIController).m_PlayerWatcher.transform.LookAt(Services.GameManager.Player.gameObject.transform);

            // Set charging to true and start the charge timer
            m_Charging = true;
            m_ToadChargeTimer.Restart();
            m_ToadLookTimer2.Restart();

            // Sets the target's position to the Player's position

            // For charging in a straight line
            //m_TargetPosition = ((AIToadController)m_AIController).transform.position + (((AIToadController)m_AIController).transform.forward * (((AIToadController)m_AIController).GetDistanceToPlayer() + m_ChargeDistanceBehindPlayer));

            // For Chargeing at the player
            m_TargetPosition = Services.GameManager.Player.transform.position;

            // Set the target's y to be water level
            m_TargetPosition.y = ((AIToadController)m_AIController).m_JumpPosition[3].transform.position.y;
        }

        // If the toad is charging
        if (m_ToadChargeTimer.IsRunning() && m_Charging == true)
        {
            // CHARGE!!!!!!
            ((AIToadController)m_AIController).transform.position = Vector3.Lerp(m_PreJumpPosition, m_TargetPosition, m_ToadChargeTimer.GetPercentage());        
        }

        // If the look timer is running
        if(m_ToadLookTimer2.IsRunning() == true)
        {
            // Turn towards the player so the charge looks better

            // Calculate the angle between the toad and the player watcher
            var angle = Quaternion.Angle(((AIToadController)m_AIController).transform.rotation, ((AIToadController)m_AIController).m_PlayerWatcher.transform.rotation);

            // If the angle is greater than 0
            if (angle > 0)
            {
                // Get the Quaternion and exclude the y axis
                Quaternion XYRotation = Quaternion.Euler(new Vector3(0f, ((AIToadController)m_AIController).transform.localEulerAngles.y, 0f));
                ((AIToadController)m_AIController).transform.rotation = Quaternion.Lerp(XYRotation, ((AIToadController)m_AIController).m_PlayerWatcher.transform.rotation, Time.deltaTime * Constants.TurnSpeed);
            }
        }

        // If the toad is done charging and he didn't hit the player
        if (m_ToadChargeTimer.IsFinished() && m_Charging == true && m_Hit == false)
        {
            // Attack is finished
            ((AIToadController)m_AIController).CurrentActionFinished();
        }

        // If the toad hit the player
        //if (((AIToadController)m_AIController).m_HitPlayer == true && m_Hit == false)
        if(((AIToadController)m_AIController).GetDistanceToPlayer() <= 4.5f)
        {
            // Start the push Timer
            m_ToadHitTimer.StartTimer();

            // Set hit to true
            m_Hit = true;

            // Deal Damage to player
            Services.GameManager.Player.TakeDamage(Constants.ChargeDamage);

            // Set the force and radius
            float Force = 150;

            // Push the player away
            Services.GameManager.Player.MovementController.PushPlayer(((AIToadController)m_AIController).transform.position, Force);
        }

        // If the toad hit the player and the hit timer is finished
        if (m_Hit == true && m_ToadHitTimer.IsFinished())
        {
            // Enable player movement
            Services.GameManager.Player.MovementController.m_playerMovementDissabled = false;

            // Attack is finished
            ((AIToadController)m_AIController).CurrentActionFinished();
        }

    }
}
