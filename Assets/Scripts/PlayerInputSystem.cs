using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

public class PlayerInputSystem : ComponentSystem
{
    struct ShieldDataGroup
    {
#pragma warning disable 649
        public readonly int Length;
        [ReadOnly] public ComponentDataArray<Shield> Shield;
        public ComponentDataArray<Position> Position;
        public ComponentDataArray<Rotation> Rotation;
    }

    [Inject] private ShieldDataGroup _shieldDataGrouop;
#pragma warning restore 649

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var screenWidth = Screen.width;
        var screenHeight = Screen.height;

        var center = new Vector3(screenWidth / 2f, screenHeight / 2f, 0f);

        if (Input.GetMouseButton(0))
        {
            var inputPosition = Input.mousePosition - center;
            UpdateShield(inputPosition.normalized);
        }
    }

    private void UpdateShield(Vector3 touchPosition)
    {
        for (int i = 0; i < _shieldDataGrouop.Length; ++i)
        {
            var distance = _shieldDataGrouop.Shield[i].OrbitRadius;

            var x = touchPosition.x;
            var y = touchPosition.y;
            var rotation = quaternion.LookRotation(new float3(x, y, 0f), new float3(0, 0, -1));
            var position = new float3(x * distance, y * distance, 0f);

            _shieldDataGrouop.Position[i] = new Position { Value = position };
            _shieldDataGrouop.Rotation[i] = new Rotation { Value = rotation };
        }
    }
}