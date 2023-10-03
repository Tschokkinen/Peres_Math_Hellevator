using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] public static GameObject[] movePoints; // Move points for souls
    [SerializeField] private GameObject soulToHell; // Send soul to hell button
    [SerializeField] private GameObject soulToHeaven; // Send soul to heaven button
    [SerializeField] private static int posInt; // Position for next soul
    [SerializeField] private static TMP_Text pointsText; // Current points text
    [SerializeField] private static int points; // Current points
    [SerializeField] private GameObject gameOver = null;

    private static GameObject hellTarget; // Hell transform
    private static GameObject heavenTarget; // Heaven transform

    public static event EventHandler? moveSoulsForward;

    // Instantiates souls
    IEnumerator SoulInstantiater()
    {
        while (true)
        {
            Debug.Log("Curent posInt: " + posInt);
            double rnd = UnityEngine.Random.Range(0.0f, 20.0f);
            if (posInt < 3) Spawn(rnd);
            yield return new WaitForSeconds(2.5f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        posInt = 0;
        // Get move points for souls
        movePoints = new GameObject[3];
        movePoints[0] = GameObject.Find("/MovePoints/Point0");
        movePoints[1] = GameObject.Find("/MovePoints/Point1");
        movePoints[2] = GameObject.Find("/MovePoints/Point2");

        // Get hell and heaven game object
        hellTarget = GameObject.Find("Hell");
        heavenTarget = GameObject.Find("Heaven");

        pointsText = GameObject.Find("/Canvas/Points").GetComponent<TMP_Text>();

        Soul.soulDelivered += SoulDelivered; // Subscribe to soul delivered
        Timer.timerStatus += TimerStatus; // Subscribe to timer status
    }

    public static void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    // Receive timer status broadcast
    private void TimerStatus(object sender, bool timerReachedZero)
    {
        Debug.Log("HOI");
        if (!timerReachedZero)
        {
            Time.timeScale = 1;
            StartCoroutine(SoulInstantiater());
        }
        else if (timerReachedZero)
        {
            Time.timeScale = 0;
            StopCoroutine(SoulInstantiater());
            Soul.soulDelivered -= SoulDelivered;
            Timer.timerStatus -= TimerStatus;
            gameOver.SetActive(true);
        }
    }

    // Soul spawner
    void Spawn(in double rnd)
    {
        GameObject nextSoul = null;
        if (rnd < 10.0f) 
        {
            nextSoul = soulToHeaven;
        }
        else 
        {
            nextSoul = soulToHell;
        }
        Instantiate(nextSoul);
    }

    // Return move point transform.position requested by instantiated soul
    public static Vector3 SetPos(in int assignedPos)
    {
        return movePoints[assignedPos].transform.position;
    }

    // Assign next free position to a new soul
    public static int AssignPos()
    {
        posInt++;
        return posInt;
    }

    // Return soul amount to timer. Used to control timer speed.
    public static int GetPosInt()
    {
        return posInt;
    }

    // Decrement pos when a soul has been delivered
    public static void DecrementPos()
    {
        posInt--;
    }

    // Return heaven or hell vector3
    public static Vector3 SendToHeavenOrHell(in bool toHeaven)
    {
        if (toHeaven)
        {
            return heavenTarget.transform.position;
        }
        else
        {
            return hellTarget.transform.position;
        }
    }

    // Receive soul delivered broadcast from soul
    private void SoulDelivered(object sender, EventArgs e)
    {
        MoveSoulsForwardBroadcast(EventArgs.Empty); // Call without arguments
    }

    // When a soul has been delivered, send a broadcast to souls
    // that they can move forward one position 
    private void MoveSoulsForwardBroadcast(EventArgs e)
    {
        moveSoulsForward?.Invoke(this, e);
    }

    // Point counter
    public static void PointCounter(int point)
    {
        points += point;
        UpdatePointsText();
        Debug.Log($"Current points {points}.");
    }

    // Update points text after point increment / decrement
    private static void UpdatePointsText()
    {
        pointsText.text = points.ToString();
    }   
}
