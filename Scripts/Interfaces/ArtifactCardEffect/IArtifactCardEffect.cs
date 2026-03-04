using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArtifactCardEffect
{
    void Update();
    void ApplyEffect(ArtifactCardScriptableObject card);
}
