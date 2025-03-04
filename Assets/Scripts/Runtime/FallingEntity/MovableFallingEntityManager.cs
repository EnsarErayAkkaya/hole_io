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

                var startingWaypoint = BaseGameManager.WaypointManager.GetRandomWaypoint();

                instance.transform.parent = transform;
                instance.transform.position = startingWaypoint.transform.position;
                instance.WaypointFollower.FromNode = startingWaypoint;

                instance.Init();
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