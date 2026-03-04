using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NapalmDrone : IArtifactCardEffect
{
    public void Update()
    {

    }
    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        PlayerManager.Instance.GetPlayer().GetComponent<RavenController>().DroneObject.SetActive(true);
    }
}
