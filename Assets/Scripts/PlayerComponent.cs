using System;
using Unity.Entities;

[Serializable]
public struct Player : IComponentData
{
    public float Lives;
}

public class PlayerComponent : ComponentDataWrapper<Player> { }