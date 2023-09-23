using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Database : MonoBehaviour
{
    private static List<MathProblem> mathProblems = new List<MathProblem>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateDatabase();
    }

    // Generate database
    private static void GenerateDatabase()
    {
        // mathProblems = new List<MathProblem>()
        // {
        //     new MathProblem("Kuinka paljon on 3 + 5", "8", "12", "8"),
        //     new MathProblem("Kuinka paljon on 8 + 4", "5", "12", "12"),
        //     new MathProblem("Kuinka paljon on 2 - 2", "0", "4", "0"),
        //     new MathProblem("Kuinka paljon on 8 - 1", "10", "7", "7")
        // };

        var numbers = new[] {(3, 5), (8, 4), (2, 2), (8, 1)}; // Test values for math problems

        for (int i = 0; i < numbers.Length; i++)
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

    // private string MathGenerator(int val1, int val2, string op)
    // {
    //     int result;

    //     switch (op)
    //     {
    //         case "+":
    //             result = val1 + val2;
    //             break;
    //         case "-":
    //             result = val1 - val2;
    //             break;
    //     }
    //     return result.ToString();
    // }

    // Return database and if it hasn't been generated yet, generate before return
    public static List<MathProblem> GetDatabase()
    {
        if (mathProblems.Count > 0)
        {
            return mathProblems;
        }
        else
        {
            GenerateDatabase();
            return mathProblems;
        }
        
    }
}
