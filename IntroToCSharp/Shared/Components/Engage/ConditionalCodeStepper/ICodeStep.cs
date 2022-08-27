using IntroToCSharp.Shared.Components.Engage.ConditionalCodeStepper;
using Microsoft.AspNetCore.Components;

public interface ICodeStep {
    public bool ReadLine {get;}
    public string Value {get;}
    public void UpdateValue(ChangeEventArgs e);
    public Action<string[]>? UpdateCode {get;}
    public int Line {get;}
    public string Output {get;}
    public RenderFragment Explanation {get;}
    public bool HasNext {get;}
    public string? NextId {get; set;}
    // public void SetOnNext(Action<ConditionalCodeStepper> stepper);
    // public Action<ConditionalCodeStepper>? OnNext {get;}
    public RenderFragment Fragment { get; set; }
    public string ID {get;}
    public void DoUpdateCode(string[] code);
    public void OnNext();
}