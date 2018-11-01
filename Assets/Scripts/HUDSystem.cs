using Unity.Collections;
using Unity.Entities;

[AlwaysUpdateSystem]
public class HUDSystem : ComponentSystem
{
#pragma warning disable 649
    struct PlayerDataGroup
    {
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<Player> Player;
    }

    [Inject] private PlayerDataGroup _playerDataGroup;
#pragma warning restore 649

    protected override void OnUpdate()
    {
        var gameStarted = _playerDataGroup.Length > 0;
        GameManagaer.Instant.UpdateUI(gameStarted);
    }
}
