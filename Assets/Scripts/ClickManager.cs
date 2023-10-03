using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ClickManager : MonoBehaviour
{


    [SerializeField] private List<MathProblem> mathProblems = new List<MathProblem>();
    [SerializeField] private Button answer1Button = null;
    [SerializeField] private Button answer2Button = null;
    [SerializeField] private Button hellButton = null;
    [SerializeField] private Button heavenButton = null;
    [SerializeField] private TMP_Text mathQuestion;
    [SerializeField] private Button restart = null;

    [SerializeField] private Timer timer;

    public static event EventHandler<bool> nextSoul;
    private bool toHeaven;

    public string rightAnswer;
    private int nextMathProblem;
    
    void Awake()
    {
        // Add listeners to answer buttons
        // answer1Button.onClick.AddListener(delegate{AnswerClick(answer1Button.GetComponentInChildren<TMP_Text>());});
        // answer2Button.onClick.AddListener(delegate{AnswerClick(answer2Button.GetComponentInChildren<TMP_Text>());});
        // hellButton.onClick.AddListener(delegate{GoesToHell();});
        // heavenButton.onClick.AddListener(delegate{GoesToHeaven();});
        restart.onClick.AddListener(delegate{Restart();});

        ButtonEvent.onClick += OnClickManager;
    }

    void Start()
    {
        nextMathProblem = 0; // Default to first math problem
        timer = GameObject.Find("Timer").GetComponent<Timer>(); // Get timer
        mathProblems = Database.GetDatabase(); // Get math problems from database
        SetMathProblem();
    }

    void SetMathProblem()
    {
        Debug.Log($"currentMathProblem: {nextMathProblem}");
        answer1Button.GetComponentInChildren<TMP_Text>().text = mathProblems[nextMathProblem].Answer1;
        answer2Button.GetComponentInChildren<TMP_Text>().text = mathProblems[nextMathProblem].Answer2;
        rightAnswer = mathProblems[nextMathProblem].RightAnswer;
        mathQuestion.text = mathProblems[nextMathProblem].Question;

        nextMathProblem++;
        if (nextMathProblem == Database.GetNumberOfProblems()) nextMathProblem = 0;
    }

    // Detect answer click
    private void AnswerClick(TMP_Text answer)
    {
        Debug.Log(answer.text);
        if (answer.text == rightAnswer) timer.GetMoreTime();
        if (answer.text != rightAnswer) timer.ReduceTime();

        SetMathProblem(); // Get next math problem
    }

    // Restart game
    private void Restart()
    {
        GameManager.RestartGame();
    }

    // Broadcast destination of Heaven/Hell button click (toHeaven or !toHeaven)
    private void DeliverNextSoul(bool destination)
    {
        nextSoul?.Invoke(this, destination); // Destination heaven (heaven) or hell (!heaven)
    }

    // Update is called once per frame
    // void Update()
    // {
    //     // Mouse controls (not in use currently)
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Vector3 mousePosition = Input.mousePosition;
    //         Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    //         if (Physics.Raycast(ray, out RaycastHit hit))
    //         {
    //             // string targetName = hit.collider.transform.name;
    //             // switch (targetName)
    //             // {
    //             //     case "GoesToHeaven":
    //             //         GoesToHeaven();
    //             //         break;
    //             //     case "GoesToHell":
    //             //         GoesToHell();
    //             //         break;
    //             // }
    //         }
    //     }

    //     // Touch controls
    //     foreach (Touch touch in Input.touches)
    //     {
    //         if (touch.phase == TouchPhase.Began)
    //         {
    //             Ray ray = Camera.main.ScreenPointToRay(touch.position);
    //             RaycastHit hit;

    //             if (Physics.Raycast(ray, out hit))
    //             {
    //                 if (hit.collider.transform != null)
    //                 {
    //                     if (hit.collider.transform.name == "GoesToHeaven")
    //                     {
    //                         GoesToHeaven();
    //                     }
    //                     else if (hit.collider.transform.name == "GoesToHell")
    //                     {
    //                         GoesToHell();
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }

    private void OnClickManager(object sender, GameObject buttonName)
    {
        Debug.Log("Sender name: " + buttonName);
        // string buttonName = sender.ToString();
        if (buttonName.name == "HellButton")
        {
            GoesToHell();
        }
        else if (buttonName.name == "HeavenButton")
        {
            GoesToHeaven();
        }
        else if (buttonName.name == "Answer1" || buttonName.name == "Answer2")
        {
            AnswerClick(buttonName.GetComponentInChildren<TMP_Text>());
        }
        else if (buttonName.name == "Restart")
        {
            Restart();
        }
    }

    // Send soul to heaven (callable from TouchControls script)
    private void GoesToHeaven()
    {
        toHeaven = true;
        Debug.Log("Hit GoesToHeaven");
        DeliverNextSoul(toHeaven);
    }

    // Send soul to hell (callable from TouchControls script)
    private void GoesToHell()
    {
        toHeaven = false;
        Debug.Log("Hit GoesToHell");
        DeliverNextSoul(toHeaven);
    }
}
