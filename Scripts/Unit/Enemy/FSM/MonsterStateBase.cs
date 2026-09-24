using UnityEngine;

// 1. 모든 상태의 기반이 되는 제네릭 클래스
public abstract class MonsterStateBase<T> : IMonsterState where T : MonsterController
{
    protected T monster; // 자식들이 사용할 수 있게 protected로 선언

    public MonsterStateBase(T monster)
    {
        this.monster = monster;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }

    // OnUpdate를 virtual로 만들어 자식이 override 할 수 있게 함
    public virtual void OnUpdate() { }
}