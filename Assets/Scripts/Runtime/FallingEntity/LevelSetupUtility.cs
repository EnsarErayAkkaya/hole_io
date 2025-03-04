using NaughtyAttributes;
using UnityEngine;

namespace EEA.Game
{
    /// <summary>
    /// This City class can automaticly sets up level objects (FallingEntities) according to collider bounding box and mesh filter
    /// </summary>
    public class LevelSetupUtility : MonoBehaviour
    {
        #region FALLING ENTITY SETUP
#if UNITY_EDITOR

        [SerializeField, Layer] private int entityLayer;
        [SerializeField] private float minCanBeTransparentHeight;
        [SerializeField] private float bottomVerticesDetectThreshold = 0.01f;

        [Button]
        private void SetupLevel()
        {
            SetupParent(transform);
        }

        private void SetupParent(Transform target)
        {
            for (int i = 0; i < target.childCount; i++)
            {
                GameObject obj = target.GetChild(i).gameObject;

                if (obj.TryGetComponent<MeshFilter>(out var filter))
                    Setup(obj);
                else
                    SetupParent(obj.transform);
            }
        }

        private void Setup(GameObject obj)
        {
            // continue if object has mesh filter
            if (obj.TryGetComponent<MeshFilter>(out var filter))
            {
                if (!obj.TryGetComponent<FallingEntity>(out var entity))
                {
                    entity = obj.AddComponent<FallingEntity>();
                }

                obj.layer = entityLayer;

                if (!obj.TryGetComponent<MeshCollider>(out var meshCollider))
                {
                    meshCollider = obj.AddComponent<MeshCollider>();
                }

                meshCollider.convex = true;
                meshCollider.sharedMesh = filter.sharedMesh;

                if (entity.references == null)
                    entity.references = new();

                // CREATE TRIGGER OBJECT
                /*if (entity.references.fallingEntityTrigger == null)
                {
                    var triggerObject = new GameObject("FallingEntityTrigger");
                    triggerObject.transform.parent = obj.transform;
                    triggerObject.transform.localPosition = Vector3.zero;
                    var triggerMeshCollider = triggerObject.AddComponent<MeshCollider>();

                    triggerMeshCollider.convex = true;
                    triggerMeshCollider.sharedMesh = filter.sharedMesh;
                    triggerMeshCollider.isTrigger = true;
                    triggerObject.SetActive(false);

                    entity.references.fallingEntityTrigger = triggerObject;
                }
                if (entity.references.fallingEntityTrigger != null)
                {
                    DestroyImmediate(entity.references.fallingEntityTrigger);
                }*/

                // set editor references
                entity.references.holeBottomTag = "HoleBottom";
                entity.references.holeDestroyTag = "HoleDestroy";

                // calculate required size and point
                float minRadius = CalculateBottomMinRadius(meshCollider);

                int size = Mathf.CeilToInt(minRadius);
                // set required size
                entity.references.requiredSize = minRadius > 5 ? size + 1 : size;

                // calculate max height for transperency
                float maxHeight = CalculateMaxHeight(meshCollider);
                entity.references.canBeTransparent = minCanBeTransparentHeight < maxHeight;

                // update weight of object by size
                if (obj.TryGetComponent<Rigidbody>(out var rigid))
                {
                    rigid.mass = (minRadius > 3 ? size + 1 : size) * 2;
                }
            }
        }

        float CalculateBottomMinRadius(MeshCollider collider)
        {
            Mesh mesh = collider.sharedMesh;
            Vector3[] vertices = mesh.vertices;

            // Find the lowest Y value
            float minY = float.MaxValue;
            foreach (Vector3 vertex in vertices)
            {
                if (vertex.y < minY)
                {
                    minY = vertex.y;
                }
            }

            // Find max distance from center (X-Z plane) among bottom vertices
            float maxRadius = 0f;
            foreach (Vector3 vertex in vertices)
            {
                if (Mathf.Abs(vertex.y - minY) <= bottomVerticesDetectThreshold) // Consider only bottom vertices
                {
                    float distance = new Vector2(vertex.x, vertex.z).magnitude; // Distance in X-Z plane
                    if (distance > maxRadius)
                    {
                        maxRadius = distance;
                    }
                }
            }

            return maxRadius;
        }

        float CalculateMaxHeight(MeshCollider collider)
        {
            Mesh mesh = collider.sharedMesh;
            Vector3[] vertices = mesh.vertices;

            // Find the lowest Y value
            float maxY = float.MinValue;
            foreach (Vector3 vertex in vertices)
            {
                if (vertex.y > maxY)
                {
                    maxY = vertex.y;
                }
            }

            return maxY;
        }
#endif
        #endregion
    }
}