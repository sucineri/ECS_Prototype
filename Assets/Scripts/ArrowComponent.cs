using System;
using Unity.Entities;

[Serializable]
public struct Arrow : IComponentData
{
}

public class ArrowComponent : ComponentDataWrapper<Arrow> { }