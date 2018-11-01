using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

public class DamageSystem : JobComponentSystem
{
    [BurstCompile]
    struct CollisionJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction]
        public ComponentDataArray<Health> ArrowHealth;

        [NativeDisableParallelForRestriction]
        public ComponentDataArray<Player> Players;

        [ReadOnly] public float CollisionRadius;
        [ReadOnly] public float ShieldCollisionRadius;
        [ReadOnly] public ComponentDataArray<Position> PlayerPositions;
        [ReadOnly] public ComponentDataArray<Position> ShieldPositions;
        [ReadOnly] public ComponentDataArray<Position> ArrowPositions;

        public void Execute(int index)
        {
            var playerPosition = PlayerPositions[index].Value;
            var shieldPosition = ShieldPositions[index].Value;

            for (int i = 0; i < ArrowHealth.Length; ++i)
            {
                var arrowPosition = ArrowPositions[i].Value;

                var deltaShield = arrowPosition - shieldPosition;
                var distanceShield = math.dot(deltaShield, deltaShield);
                if (distanceShield <= ShieldCollisionRadius)
                {
                    var health = ArrowHealth[i];
                    health.Value -= 1f;
                    ArrowHealth[i] = health;
                    continue;
                }


                var delta = arrowPosition - playerPosition;
                var distance = math.dot(delta, delta);
                if (distance <= CollisionRadius)
                {
                    var health = ArrowHealth[i];
                    health.Value -= 1f;
                    ArrowHealth[i] = health;

                    var player = Players[index];
                    player.Lives -= 1f;
                    Players[index] = player;
                }
            }
        }
    }

#pragma warning disable 649
    struct PlayerDataGroup
    {
        public readonly int Length;
        public ComponentDataArray<Player> Player;
        [ReadOnly] public ComponentDataArray<Position> Position;
    }

    struct ArrowDataGroup
    {
        public readonly int Length;
        public ComponentDataArray<Health> Health;
        [ReadOnly] public ComponentDataArray<Arrow> Arrow;
        [ReadOnly] public ComponentDataArray<Position> Position;
    }

    struct ShieldDataGroup
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<Shield> Shield;
        [ReadOnly] public ComponentDataArray<Position> Position;
    }

    [Inject] PlayerDataGroup _players;
    [Inject] ArrowDataGroup _arrows;
    [Inject] ShieldDataGroup _shields;
#pragma warning restore 649

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var collisionJob = new CollisionJob
        {
            CollisionRadius = .5f,
            ShieldCollisionRadius = 1f,
            PlayerPositions = _players.Position,
            Players = _players.Player,
            ArrowHealth = _arrows.Health,
            ArrowPositions = _arrows.Position,
            ShieldPositions = _shields.Position,
        };

        return collisionJob.Schedule(_players.Length, 1, inputDeps);
    }
}
