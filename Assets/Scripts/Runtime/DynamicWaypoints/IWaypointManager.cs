using UnityEngine;

namespace EEA.Game
{
    public interface IWaypointManager
    {
        public DynamicWaypointNode GetRandomWaypoint();
        public float GetPointProjection(DynamicWaypointNode from, DynamicWaypointNode to, Vector3 point);
        public Vector3 GetProjectedPosition(DynamicWaypointNode from, DynamicWaypointNode to, Vector3 point);
        public float GetDistanceToProjection(DynamicWaypointNode from, DynamicWaypointNode to, Vector3 point);

    }
}