using Ludred.States;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class StateTests
    {
        [Test]
        public void TestStateCreation()
        {
            var state = new State();
            
            Assert.NotNull(state);
            Assert.NotNull(state.Behaviours);
            Assert.True(state.Behaviours.Length == 0);
            Assert.True(state.Name == "New State");
        }

        [Test]
        public void TestStateChangeName()
        {
            var state = new State();
            state.Name = "TestName";

            Assert.True(state.Name == "TestName");
        }
        
        [Test]
        public void TestAddBehaviour()
        {
            var state = new State();

            var behaviourTestNumber = Random.Range(1, 10);
            var behaviours = new TestStateBehaviour[behaviourTestNumber];

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                behaviours[i] = new TestStateBehaviour();
            }

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                state.AddBehaviour(behaviours[i]);
                Assert.True(state.Behaviours.Length == i + 1);
                Assert.True(state.Behaviours[i] == behaviours[i]);
            }

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                Assert.True(behaviours[i] == state.Behaviours[i]);
            }

        }
        
        [Test]
        public void TestRemoveBehaviour()
        {
            var state = new State();

            var behaviourTestNumber = Random.Range(1, 10);
            var behaviours = new TestStateBehaviour[behaviourTestNumber];

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                behaviours[i] = new TestStateBehaviour();
            }

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                state.AddBehaviour(behaviours[i]);
            }

            var removeElement = Random.Range(0, behaviourTestNumber);
            
            state.RemoveBehaviour(behaviours[removeElement]);
            Assert.True(state.Behaviours.Length == behaviours.Length - 1);
            if(removeElement < state.Behaviours.Length)
                Assert.True(behaviours[removeElement] != state.Behaviours[removeElement]);
        }

        [Test]
        public void TestStateEnter()
        {
            var state = new State();

            var behaviourTestNumber = Random.Range(1, 10);

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                state.AddBehaviour(new TestStateBehaviour());
            }

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                Assert.True(!((TestStateBehaviour)state.Behaviours[i]).Entered);
            }
            
            state.EnterState(null);
            
            for (int i = 0; i < behaviourTestNumber; i++)
            {
                Assert.True(((TestStateBehaviour)state.Behaviours[i]).Entered);
            }
        }
        
        [Test]
        public void TestStateUpdate()
        {
            var state = new State();

            var behaviourTestNumber = Random.Range(1, 10);

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                state.AddBehaviour(new TestStateBehaviour());
            }

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                Assert.True(((TestStateBehaviour)state.Behaviours[i]).Updated == 0);
            }

            var cycles = Random.Range(0, 100);

            for (int i = 0; i < cycles; i++)
            {
                state.UpdateState(null);
            }

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                Assert.True(((TestStateBehaviour)state.Behaviours[i]).Updated == cycles);
            }
        }
        
        [Test]
        public void TestStateExit()
        {
            var state = new State();

            var behaviourTestNumber = Random.Range(1, 10);

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                state.AddBehaviour(new TestStateBehaviour());
            }

            for (int i = 0; i < behaviourTestNumber; i++)
            {
                Assert.True(!((TestStateBehaviour)state.Behaviours[i]).Exited);
            }
            
            state.ExitState(null);
            
            for (int i = 0; i < behaviourTestNumber; i++)
            {
                Assert.True(((TestStateBehaviour)state.Behaviours[i]).Exited);
            }
        }
    }
}
