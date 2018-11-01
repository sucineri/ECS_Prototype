using Unity.Collections;
using Unity.Entities;

[UpdateAfter(typeof(SpawnerSystem))]
public class DestroySystem : ComponentSystem
{
#pragma warning disable 649
    struct ArrowDataGroup
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<Health> Health;
        [ReadOnly] public ComponentDataArray<Arrow> Arrow;
        public EntityArray Entities;
    }

    struct SpawnerDataGroup
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<SpawnerData> Spawner;
        public EntityArray Entities;
    }

    struct ShieldDataGroup
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<Shield> Shield;
        public EntityArray Entities;
    }

    struct PlayerDataGroup
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<Player> Player;
        public EntityArray Entities;
    }

    [Inject] private ArrowDataGroup _arrowDataGroup;
    [Inject] private SpawnerDataGroup _spawnerDataGroup;
    [Inject] private ShieldDataGroup _shieldDataGroup;
    [Inject] private PlayerDataGroup _playerDataGroup;
#pragma warning restore 649

    protected override void OnUpdate()
    {
        var playerDead = false;

        for (var i = 0; i < _playerDataGroup.Length; ++i)
        {
            if (_playerDataGroup.Player[i].Lives <= 0f)
            {
                PostUpdateCommands.DestroyEntity(_playerDataGroup.Entities[i]);
                playerDead = true;
            }
        }

        for (var i = 0; i < _arrowDataGroup.Length; ++i)
        {
            if (playerDead || _arrowDataGroup.Health[i].Value <= 0f)
            {
                PostUpdateCommands.DestroyEntity(_arrowDataGroup.Entities[i]);
            }
        }

        if (playerDead)
        {
            for (var i = 0; i < _spawnerDataGroup.Length; ++i)
            {
                PostUpdateCommands.DestroyEntity(_spawnerDataGroup.Entities[i]);
            }

            for (var i = 0; i < _shieldDataGroup.Length; ++i)
            {
                PostUpdateCommands.DestroyEntity(_shieldDataGroup.Entities[i]);
            }
        }
    }
}

