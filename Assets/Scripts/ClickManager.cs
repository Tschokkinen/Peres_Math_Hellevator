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
    [SerializeField] private TMP_Text mathQuestion;

    [SerializeField] private Timer timer;

    public static event EventHandler<bool> nextSoul;
    private bool toHeaven;

    public string rightAnswer;
    private int nextMathProblem;
    
    void Awake()
    {
        // Add listeners to answer buttons
        answer1Button.onClick.AddListener(delegate{AnswerClick(answer1Button.GetComponentInChildren<TMP_Text>());});
        answer2Button.onClick.AddListener(delegate{AnswerClick(answer2Button.GetComponentInChildren<TMP_Text>());});
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
    void AnswerClick(TMP_Text answer)
    {
        Debug.Log(answer.text);
        if (answer.text == rightAnswer) timer.GetMoreTime();
        if (answer.text != rightAnswer) timer.ReduceTime();

        SetMathProblem(); // Get next math problem
    }

    // Broadcast destination of Heaven/Hell button click (toHeaven or !toHeaven)
    private void DeliverNextSoul(bool destination)
    {
        nextSoul?.Invoke(this, destination); // Destination heaven (heaven) or hell (!heaven)
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                string targetName = hit.collider.transform.name;
                switch (targetName)
                {
                    case "GoesToHeaven":
                        toHeaven = true;
                        Debug.Log("Hit GoesToHeaven");
                        DeliverNextSoul(toHeaven);
                        break;
                    case "GoesToHell":
                        toHeaven = false;
                        Debug.Log("Hit GoesToHell");
                        DeliverNextSoul(toHeaven);
                        break;
                }
            }
        }
    }
}
