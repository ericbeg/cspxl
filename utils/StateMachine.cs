#pragma  warning disable 1591

using System;
using System.Collections.Generic;


namespace pxl
{
    /// <summary>
    /// A simple StateMachine in which the states and transitions are defined using lamda expression.
    /// </summary>
    public class StateMachine
    {
        public delegate bool Condition();
        public delegate void State();
        public delegate void TransitionFunction();

        /// <summary>
        /// The current state the machine is in.
        /// </summary>
        public State currentState
        {
            get
            {
                return m_currentState;
            }

            set
            {
                m_previousState = m_currentState;
                m_currentState = value;
            }
        }

        /// <summary>
        /// The previous state the machine were in.
        /// </summary>
        public State previousState
        {
            get
            {
                return m_previousState;
            }
        }

        /// <summary>
        /// Define a new transition to exit state <from> and entering state <to>.
        /// </summary>
        /// <param name="from">The starting state the machine</param>
        /// <param name="to">The state next state</param>
        /// <param name="condition">The condition to activate the transition.</param>
        /// <param name="transitionFunction">The transition function is called when the transition is activated.</param>
        public void AddTransition(State from, State to, Condition condition, TransitionFunction transitionFunction)
        {
            // The first encountered state is defined as the initial state.
            if (currentState == null)
                currentState = from;

            if (!m_fsm.ContainsKey(from))
            {
                m_fsm[from] = new List<StateTransition>();
            }

            StateTransition tran = new StateMachine.StateTransition();
            tran.condition = condition;
            tran.nextState = to;
            tran.transitionFunction = transitionFunction;

            m_fsm[from].Add(tran);
        }

        /// <summary>
        /// Define a new transition to exit state <from> and entering state <to>.
        /// </summary>
        /// <param name="from">The starting state the machine</param>
        /// <param name="to">The state next state</param>
        /// <param name="condition">The condition to activate the transition.</param>
        public void AddTransition(State from, State to, Condition condition)
        {
            AddTransition(from, to, condition, null);
        }

        /// <summary>
        /// Define an unconditionnal transition to exit state <from> and entering state <to>.
        /// </summary>
        /// <param name="from">The starting state the machine</param>
        /// <param name="to">The state next state</param>
        public void AddTransition(State from, State to)
        {
            AddTransition(from, to, null, null);
        }

        /// <summary>
        /// Step the State machine.
        /// Calling this method will invoke the current state and evaluate its transitions.
        /// </summary>
        public void Step()
        {
            if (currentState != null)
            {
                // Run the current state
                currentState();
                // Evaluate the transitions
                //if (!EvaluateTransitions(null))
                EvaluateTransitions(currentState);
            }
        }

        private bool EvaluateTransitions(State state)
        {
            bool hasActiveTransition = false;
            if (m_fsm.ContainsKey(state))
            {
                List<StateTransition> transitions = m_fsm[state];
                foreach (StateTransition t in transitions)
                {
                    if (t.condition == null || t.condition())
                    {
                        if (t.transitionFunction != null)
                            t.transitionFunction();

                        currentState = t.nextState;
                        hasActiveTransition = true;
                        break;
                    }
                }
            }
            return hasActiveTransition;
        }

        private State m_currentState;
        private State m_previousState;

        private Dictionary<State, List<StateTransition>> m_fsm = new Dictionary<State, List<StateTransition>>();

        private struct StateTransition
        {
            public Condition condition;
            public State nextState;
            public TransitionFunction transitionFunction;
        }
    }
}