using UnityEngine;

public interface IMonsterState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}
