using System.Collections.Generic;
using System.Text.RegularExpressions;
using Triggers;
using UnityEngine;
using UnityEngine.Events;

public class ConditionTrigger : AbstractTrigger
{
    [SerializeField] private UnityEvent<string> onResult;
    [SerializeField] private UnityEvent onTrue;
    [SerializeField] private string condition;

    private readonly Dictionary<string, string> variables = new();
    
    public void SetVariable(string value)
    {
        var split = value.Split(" = ");
        if (split.Length != 2) return;
        variables[split[0]] = split[1];
        var result = TextProcessor.Execute(condition, variables);
        onResult.Invoke(result);
        if (bool.TryParse(result, out var resultBool) && resultBool) 
            onTrue.Invoke();
    }

    public override void ResetState(Dictionary<string, object> states) { }
    public override void ApplyState(Dictionary<string, object> states) { }
}