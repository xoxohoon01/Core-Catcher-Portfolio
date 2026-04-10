using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<UnitController>() != null)
        {
            UnitController target = other.GetComponent<UnitController>();
            if (target.faction == Faction.Player)
            {
                ActivateMagnet();
                ObjectPoolManager.Instance.Despawn(gameObject);
            }
        }
    }

    private void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0);
    }

    private void ActivateMagnet()
    {
        ExpGem[] allGems = FindObjectsByType<ExpGem>(FindObjectsSortMode.None);

        foreach (ExpGem gem in allGems)
        {
            gem.BeginMagnet();
        }
    }
}
