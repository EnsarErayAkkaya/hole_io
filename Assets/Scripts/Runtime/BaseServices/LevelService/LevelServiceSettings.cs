using UnityEngine;
using Eflatun.SceneReference;

namespace EEA.BaseService
{
    [CreateAssetMenu(fileName = "LevelServiceSettings", menuName = "BaseServices/Level Service Settings", order = 0)]
    public class LevelServiceSettings : ScriptableObject
    {
        public LevelConfig[] levelConfigs;

        public LevelConfig GetLevelConfig(int level)
        {
            return levelConfigs[level % levelConfigs.Length];
        }

        [System.Serializable]
        public class LevelConfig
        {
            public SceneConfig sceneConfig;

            public Vector3 levelSize;
        }
    }
}
