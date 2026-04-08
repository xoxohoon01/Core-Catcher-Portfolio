using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medikit : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<UnitController>() != null)
        {
            UnitController target = other.GetComponent<UnitController>();
            if (target.faction == Faction.Player)
            {
                PlayerManager.Instance.GetPlayer().GetHeal(PlayerManager.Instance.GetPlayer().status.maxHP * 0.2f);
                ObjectPoolManager.Instance.Despawn(gameObject);
            }
        }
    }
}
