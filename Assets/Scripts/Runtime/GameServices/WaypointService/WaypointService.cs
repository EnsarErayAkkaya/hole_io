using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EEA.Game
{
    public class WaypointService : IWaypointService
    {
        private List<DynamicWaypointNode> _waypointNodes;

        public List<DynamicWaypointNode> WaypointNodes => _waypointNodes;

        public WaypointService()
        {
            _waypointNodes = GameObject.FindObjectsOfType<DynamicWaypointNode>().ToList();
        }

        public DynamicWaypointNode GetRandomWaypoint()
        {
            return WaypointNodes[Random.Range(0, WaypointNodes.Count)];
        }

        public float GetPointProjection(DynamicWaypointNode from, DynamicWaypointNode to, Vector3 point)
        {
            Vector3 mainVector = to.transform.position - from.transform.position;
            Vector3 pointVector = point - from.transform.position;

            // dot product of a vector with itself is sqrMagnitude of that vector
            // this code project a point into the main vector
            float progress = Vector3.Dot(pointVector, mainVector) / Vector3.Dot(mainVector, mainVector);
            return progress;
        }

        public Vector3 GetProjectedPosition(DynamicWaypointNode from, DynamicWaypointNode to, Vector3 point)
        {
            Vector3 mainVector = to.transform.position - from.transform.position;
            Vector3 pointVector = point - from.transform.position;

            // dot product of a vector with itself is sqrMagnitude of that vector
            // this code project a point into the main vector
            float progress = Vector3.Dot(pointVector, mainVector) / Vector3.Dot(mainVector, mainVector);

            return from.transform.position + (mainVector * progress);
        }

        public float GetDistanceToProjection(DynamicWaypointNode from, DynamicWaypointNode to, Vector3 point)
        {
            Vector3 projection = GetProjectedPosition(from, to, point);

            return Vector3.Distance(projection, point);
        }
    }
}