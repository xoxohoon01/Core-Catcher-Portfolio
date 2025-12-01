using UnityEngine;

public class MonsterStateMachine
{
    private IMonsterState currentState;

    public void ChangeState(IMonsterState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter();
    }

    public void Update()
    {
        currentState?.OnUpdate();
    }
}
