using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=System.Random;

public class Database : MonoBehaviour
{
    private static List<MathProblem> mathProblems = new List<MathProblem>();
    private static List<(int, int)> numbers = new List<(int, int)>();

    [SerializeField] private int numberOfProblems;
    private static int numberOfProblemsStatic;

    // Start is called before the first frame update
    void Awake()
    {
        //numberOfProblems = 30;
        numberOfProblemsStatic = numberOfProblems != 0 ? numberOfProblems : 30; // If no value is given in Unity editor
        Debug.Log("numberOfProblemStatic: " + numberOfProblemsStatic);
        GenerateDatabase();
    }

    public static int GetNumberOfProblems()
    {
        return numberOfProblemsStatic;
    }

    // Generate database
    private static void GenerateDatabase()
    {
        Random rnd = new Random();
        for (int i = 0; i < numberOfProblemsStatic; i++)
        {
            int val1 = rnd.Next(0, 20);
            int val2 = rnd.Next(0, 20);
            numbers.Add((val1, val2));
            Debug.Log($"Next val1 {val1} and val2 {val2}");
        }

        for (int i = 0; i < numbers.Count; i++)
        {
            string op = null;
            float randomVal = UnityEngine.Random.Range(0.0f, 20.0f);
            if (randomVal < 10.0f)
            {
                op = "+";
            }
            else
            {
                op = "-";
            }
            var getStuff = MathProblemGenerator(numbers[i].Item1, numbers[i].Item2, op);

            mathProblems.Add(new MathProblem(getStuff.Item1, getStuff.Item2, getStuff.Item3, getStuff.Item4));
        }
    }

    private static (string, string, string, string) MathProblemGenerator(int val1, int val2, string op)
    {
        string question = "Kuinka paljon on " + val1.ToString() + ' ' + op + ' ' + val2.ToString() + "?";
        int result = 0;
        int randomWrongAnswer;

        string returnVal1;
        string returnVal2;

        switch (op)
        {
            case "+":
                result = val1 + val2;
                break;
            case "-":
                result = val1 - val2;
                break;
        }

        // Generate random wrong answer
        do
        {
            float generatedWrongAnswer = UnityEngine.Random.Range(-10.0f, 20.0f);
            randomWrongAnswer = (int) generatedWrongAnswer;
        } 
        while (randomWrongAnswer == result);

        Debug.Log(randomWrongAnswer);
        float randomizeRightWrongOrder = UnityEngine.Random.Range(1.0f, 50.0f);

        if (randomizeRightWrongOrder > 25.0f)
        {
            returnVal1 = result.ToString();
            returnVal2 = randomWrongAnswer.ToString();
        }
        else
        {
            returnVal1 = randomWrongAnswer.ToString();
            returnVal2 = result.ToString();
        }

        return (question, returnVal1, returnVal2, result.ToString()); 
    }

    // Return database and if it hasn't been generated yet, generate before return
    public static List<MathProblem> GetDatabase()
    {
        if (mathProblems.Count != 0)
        {
            Debug.Log("More than 0 items on mathProblems");
            return mathProblems;
        }
        else
        {
            GenerateDatabase();
            return mathProblems;
        }
    }
}
