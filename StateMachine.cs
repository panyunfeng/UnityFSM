using System.Collections;
using System.Collections.Generic;

namespace FiniteStateMachine
{
    /// <summary>
    /// state machine
    /// </summary>
    public class FSMStateMachine
    {
        private FSMState CurState;

        private Dictionary<System.Enum, FSMState> stateDic = new Dictionary<System.Enum, FSMState>();

        public void AddState(FSMState state)
        {
            if (!stateDic.ContainsKey(state.name))
            {
                stateDic.Add(state.name, state);
            }
        }

        public void AddTranslation(FSMTranslation fsmTranslation)
        {
            if (stateDic.ContainsKey(fsmTranslation.fromState.name))
            {
                if (!stateDic[fsmTranslation.fromState.name].TranslationDict.ContainsKey(fsmTranslation.toState.name))
                {
                    stateDic[fsmTranslation.fromState.name].TranslationDict.Add(fsmTranslation.toState.name, fsmTranslation);
                }
            }
        }

        public void Start(FSMState state)
        {
            Start(state.name);
        }

        public void Start(System.Enum stateName)
        {
            if (stateDic.ContainsKey(stateName))
            {
                CurState = stateDic[stateName];
                CurState.EnterState();
            }
        }

        public void OnUpdate()
        {
            CurState.OnUpdate();
        }

        public void SwitchState(FSMState toState)
        {
            SwitchState(toState.name);
        }

        public void SwitchState(System.Enum stateName)
        {
            if (CurState.TranslationDict.ContainsKey(stateName) && stateDic.ContainsKey(stateName))
            {
                CurState.ExitState();
                System.Action callback = CurState.TranslationDict[stateName].translationCallBack;
                CurState = stateDic[stateName];
                CurState.EnterState();
                if (callback != null)
                {
                    callback();
                }
            }
        }

        public bool isState(System.Enum stateName)
        {
            if (CurState.name.Equals(stateName))
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// state
    /// </summary>
    public class FSMState
    {
        public System.Enum name;
        private System.Action onEnter;
        private System.Action onExit;
        private System.Action onUpdate;

        public FSMState(System.Enum name, System.Action onEnter, System.Action onExit, System.Action onUpdate)
        {
            this.name = name;
            this.onEnter = onEnter;
            this.onExit = onExit;
            this.onUpdate = onUpdate;
        }

        public Dictionary<System.Enum, FSMTranslation> TranslationDict = new Dictionary<System.Enum, FSMTranslation>();

        public void EnterState()
        {
            if (onEnter != null)
            {
                onEnter();
            }
        }

        public void ExitState()
        {
            if (onExit != null)
            {
                onExit();
            }
        }

        public void OnUpdate()
        {
            if (onUpdate != null)
            {
                onUpdate();
            }
        }
    }
    /// <summary>
    /// translation
    /// </summary>
    public class FSMTranslation
    {
        public FSMState fromState;
        public FSMState toState;
        public System.Action translationCallBack;

        public FSMTranslation(FSMState fromState, FSMState toState, System.Action translationCallBack)
        {
            this.fromState = fromState;
            this.toState = toState;
            this.translationCallBack = translationCallBack;
        }
    }
}
