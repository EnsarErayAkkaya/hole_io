using UnityEngine;

namespace EEA.Game
{
    public interface IWaypointService
    {
        public DynamicWaypointNode GetRandomWaypoint();

        /// <summary>
        /// project point to from - to segment
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetPointProjection(DynamicWaypointNode from, DynamicWaypointNode to, Vector3 point);
        /// <summary>
        /// calculate projected position of point on from - to segment
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3 GetProjectedPosition(DynamicWaypointNode from, DynamicWaypointNode to, Vector3 point);
        /// <summary>
        /// calcualte closest distance of point on segment
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetDistanceToProjection(DynamicWaypointNode from, DynamicWaypointNode to, Vector3 point);

    }
}