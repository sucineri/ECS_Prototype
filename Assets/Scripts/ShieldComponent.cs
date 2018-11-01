using System;
using Unity.Entities;

[Serializable]
public struct Shield : IComponentData
{
    public float OrbitRadius;
}

public class ShieldComponent : ComponentDataWrapper<Shield> { }