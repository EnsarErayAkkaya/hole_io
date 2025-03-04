using System.Collections.Generic;
using UnityEngine;

namespace EEA.Game
{
    public class DynamicWaypointNode : MonoBehaviour
    {
        [SerializeField] private List<DynamicWaypointNode> waypointNeighbours; 

        public DynamicWaypointNode GetRandomNeighbour()
        {
            return waypointNeighbours[Random.Range(0, waypointNeighbours.Count)];
        }
    }
}