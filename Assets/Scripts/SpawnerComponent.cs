using System;
using Unity.Entities;

[Serializable]
public struct SpawnerData : IComponentData
{
    public float SpawnInterval;
    public float SpawnRadius;
    public float TimeSinceLastSpawn;
}

public class SpawnerComponent : ComponentDataWrapper<SpawnerData> { }