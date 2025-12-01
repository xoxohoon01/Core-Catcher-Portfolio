using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;

public class IMonsterAttackState : IMonsterState
{
    private MonsterController monster;

    public IMonsterAttackState(MonsterController monster)
    {
        this.monster = monster;
    }

    public void OnEnter()
    {
    }

    public void OnUpdate()
    {
    }

    public void OnExit() { }

}
