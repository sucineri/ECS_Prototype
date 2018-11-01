using Unity.Burst;
using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ArrowMoveSystem : JobComponentSystem
{
#pragma warning disable 649
    struct ArrowDataGroup
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<Arrow> Arrow;
        [ReadOnly] public ComponentDataArray<MoveSpeed> MoveSpeed;
        [ReadOnly] public ComponentDataArray<Rotation> Rotation;
        public ComponentDataArray<Position> Position;
    }
#pragma warning restore 649

    [BurstCompile]
    struct MovementJob : IJobParallelFor
    {
        public ComponentDataArray<Position> Positions;
        [ReadOnly] public ComponentDataArray<MoveSpeed> MoveSpeed;
        [ReadOnly] public ComponentDataArray<Rotation> Rotation;
        public float DeltaTime;

        public void Execute(int index)
        {
            var newPosition = Positions[index].Value;
            newPosition += DeltaTime * MoveSpeed[index].Value * math.forward(Rotation[index].Value);
            Positions[index] = new Position { Value = newPosition };
        }
    }

    [Inject] private ArrowDataGroup _arrowDataGroup;

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var moveForwardJob = new MovementJob
        {
            Positions = _arrowDataGroup.Position,
            Rotation = _arrowDataGroup.Rotation,
            MoveSpeed = _arrowDataGroup.MoveSpeed,
            DeltaTime = Time.deltaTime,
        };
        var moveForwardJobHandle = moveForwardJob.Schedule(_arrowDataGroup.Position.Length, 64, inputDeps);
        return moveForwardJobHandle;
    }
}
