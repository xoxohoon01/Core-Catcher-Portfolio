using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulEventListener : MonoBehaviour
{

    public void Attack()
    {
        GhoulController controller = transform.parent.GetComponent<GhoulController>();
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"GhoulAttack{Random.Range(1, 4)}");
        
        controller.AttackInitialize();
    }

    public void EndAttack()
    {
        GhoulController controller = transform.root.GetComponent<GhoulController>();
        controller.isAttack = false;
    }
}
