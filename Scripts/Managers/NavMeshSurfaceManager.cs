using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshSurfaceManager : MonoBehaviour
{
    public NavMeshSurface surface;
    private void Start()
    {
        surface.BuildNavMesh();
    }
}
