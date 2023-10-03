using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Soul : MonoBehaviour
{
    [SerializeField]public int currentPos;
    public static event EventHandler? soulDelivered;
    [SerializeField] private bool goodSoul;
    [SerializeField] private bool alreadyDelivered;

    // Set up soul on enable
    private void OnEnable()
    {
        GameManager.moveSoulsForward += GetNewPos; // Subscribe to moveSoulsForward brodcast
        ClickManager.nextSoul += AmINext; // Subscribe to nextSoul broadcast
        SetReady();
    }

    // Set soul ready
    private void SetReady()
    {
        currentPos = GameManager.AssignPos() - 1; 
        transform.position = GameManager.SetPos(currentPos);
        alreadyDelivered = false;
    }

    // Receive broadcast for next position if a soul has been delivered
    public void GetNewPos(object sender, EventArgs e)
    {
        currentPos--;
        if (currentPos < 0) // Prevent currentPos from going out of range
        {
            currentPos = 0;
        }
        else
        {
            transform.position = GameManager.SetPos(currentPos);
        }
    }

    // Broadcast that a soul has been delivered
    private void SoulDeliveredBroadcast(EventArgs e)
    {
        soulDelivered?.Invoke(this, e);
    }

    // Receive broadcast for next soul in line to be delivered
    private void AmINext(object sender, bool toHeaven)
    {
        if (currentPos == 0 && !alreadyDelivered) 
        {
            transform.position = GameManager.SendToHeavenOrHell(toHeaven);
            alreadyDelivered = true;
        }
    }

    // De-subscribe to broadcasts on disable
    private void OnDisable()
    {
        ClickManager.nextSoul -= AmINext;
        GameManager.moveSoulsForward -= GetNewPos;
        GameManager.DecrementPos(); // Decrement posInt
        SoulDeliveredBroadcast(EventArgs.Empty);
    }

    // Detect collision with hell or heaven trigger when a soul has been delivered
    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Hell" && !goodSoul) 
        {
            GameManager.PointCounter(1);
        }
        else if (collider.name == "Hell" && goodSoul)
        {
            GameManager.PointCounter(-1);
        }
        else if (collider.name == "Heaven" && !goodSoul)
        {
            GameManager.PointCounter(-1);
        }
        else if (collider.name == "Heaven" && goodSoul)
        {
            GameManager.PointCounter(1);
        }

        if (this.gameObject != null) Destroy(this.gameObject, 0.5f);
        Debug.Log("Soul delivered");
    }
}
