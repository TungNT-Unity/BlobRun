using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;

public class AttractSystem : ComponentSystem
{
    public float3 center;
    public float maxDistanceSqrd;
    public float3 velocity;
    public float3 rotation;

    protected override unsafe void OnUpdate()
    {
        Entities.ForEach(
            ( ref PhysicsVelocity velocity,
                ref Translation position,
                ref Rotation rotation) =>
            {
                
                float3 diff = center - position.Value;
                rotation.Value = quaternion.LookRotation(diff,new float3(0,1,0));
                float distSqrd = math.lengthsq(diff);
                if (distSqrd > maxDistanceSqrd)
                {
                    // Alter linear velocity
                    velocity.Linear = this.velocity;
                }
            });
    }
};