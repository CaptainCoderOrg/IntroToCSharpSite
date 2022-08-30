namespace CaptainCoder;

public class MultipleChoiceQuestionData 
{
    public string[] Answers { get; }
    public bool IsSubmitted { get; }
    public int Attempts { get; }
    public MultipleChoiceQuestionData(string[] answers, bool isSubmitted, int attempts)
    {
        this.IsSubmitted = isSubmitted;
        this.Attempts = attempts;
        this.Answers = answers;
    }
}