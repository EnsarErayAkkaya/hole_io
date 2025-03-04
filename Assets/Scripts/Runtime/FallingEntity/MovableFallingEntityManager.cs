using EEA.BaseService;
using UnityEngine;

namespace EEA.Game
{
    public class MovableFallingEntityManager : MonoBehaviour
    {
        public EditorReferences references;

        private void Start()
        {
            for (int i = 0; i < references.spawnCount; i++)
            {
                int selectedPrefabIndex = Random.Range(0, references.movablePrefabs.Length);

                var instance = BaseServices.PoolService.Spawn(references.movablePrefabs[selectedPrefabIndex]);

                var fromNode = BaseGameManager.WaypointManager.GetRandomWaypoint();

                instance.transform.parent = transform;

                var toNode = fromNode.GetRandomNeighbour();

                instance.WaypointFollower.FromNode = fromNode;
                instance.WaypointFollower.ToNode = toNode;

                // start from a random point between from and to nodes
                instance.transform.position = fromNode.transform.position +
                    ((toNode.transform.position - fromNode.transform.position) * Random.value);

                instance.WaypointFollower.StartFollowing();
            }
        }

        [System.Serializable]
        public class EditorReferences
        {
            public MovableFallingEntity[] movablePrefabs;
            public int spawnCount;
        }
    }
}