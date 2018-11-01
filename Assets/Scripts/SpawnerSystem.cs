using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class SpawnerSystem : ComponentSystem
{
    struct SpawnerDataGroup
    {
#pragma warning disable 649
        public readonly int Length;
#pragma warning restore 649
        public ComponentDataArray<SpawnerData> spawnerDataArray;
    }

    [Inject]
    SpawnerDataGroup _spawnerDataGroup;

    protected override void OnUpdate()
    {
        for (int i = 0; i < _spawnerDataGroup.Length; ++i)
        {
            var data = _spawnerDataGroup.spawnerDataArray[i];
            data.TimeSinceLastSpawn += Time.deltaTime;

            var spawnCount = 0;

            while (data.TimeSinceLastSpawn >= data.SpawnInterval)
            {
                data.TimeSinceLastSpawn -= data.SpawnInterval;
                spawnCount++;
            }

            _spawnerDataGroup.spawnerDataArray[i] = data;

            while (spawnCount-- > 0)
            {
                SpawnArrow(data.SpawnRadius);
            }
        }
    }

    private void SpawnArrow(float radius)
    {
        var twoPi = (float)math.PI * 2;
        var radian = UnityEngine.Random.Range(0f, twoPi);
        //var distance = UnityEngine.Random.Range(0f, radius);

        float y, x;
        math.sincos(radian, out y, out x);

        var rotation = quaternion.LookRotation(new float3(-x, -y, 0f), math.up());
        var position = new float3(x * radius, y * radius, 0f);

        var entity = EntityManager.Instantiate(GameManagaer.Instant.ArrowPrefab);
        EntityManager.SetComponentData(entity, new Position { Value = position });
        EntityManager.SetComponentData(entity, new Rotation { Value = rotation });
    }
}
