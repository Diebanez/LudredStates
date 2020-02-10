using Ludred.States;
using NUnit.Framework;
using UnityEngine;

public class StateTransitionTests
{
    [Test]
    public void TestTransitionConstructor()
    {
        var firstState = new State();
        var secondState = new State();
        var testTransition = new StateTransition(firstState, secondState, "TransitionTrigger");
        
        Assert.NotNull(testTransition);
        Assert.True(testTransition.SourceState == firstState);
        Assert.True(testTransition.TargetState == secondState);
        Assert.True(testTransition.TransitionTrigger == "TransitionTrigger");
    }

    [Test]
    public void TestTransitionEquals()
    {
        var firstState = new State();
        var secondState = new State();
        
        var testTransition1 = new StateTransition(firstState, secondState, "TransitionTrigger");
        var testTransition2 = new StateTransition(firstState, secondState, "TransitionTrigger");
        var testTransition3 = new StateTransition(secondState, firstState, "TransitionTrigger");
        
        Assert.True(testTransition1.Equals(testTransition2));
        Assert.True(!testTransition1.Equals(testTransition3));
    }
}
