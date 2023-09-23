using System;

public class MathProblem
{
    private string question; // Math problem
    private string answer1; // Answer one
    private string answer2; // Answer two
    private string rightAnswer; // Right answer

    public string Answer1
    {
        get { return answer1; }
        //set { answer1 = value; }
    }

    public string Answer2
    {
        get { return answer2; }
        //set { answer2 = value; }
    }

    public string RightAnswer
    {
        get { return rightAnswer; }
    }

    public string Question
    {
        get { return question; }
    }

    public MathProblem (string question, string answer1, string answer2, string rightAnswer)
    {
        this.question = question;
        this.answer1 = answer1;
        this.answer2 = answer2;
        this.rightAnswer = rightAnswer;
    }


}