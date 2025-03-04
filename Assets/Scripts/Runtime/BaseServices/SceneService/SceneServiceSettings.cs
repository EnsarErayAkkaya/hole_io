using Eflatun.SceneReference;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

namespace EEA.BaseService
{
    [CreateAssetMenu(fileName = "SceneServiceSettings", menuName = "BaseServices/Scene/Scene Service Settings", order = 0)]
    public sealed class SceneServiceSettings : ScriptableObject
    {
        [BoxGroup("Test")]
        public bool loadTestScene = false;
        [BoxGroup("Test")]
        public SceneConfig testSceneConfig;

        [BoxGroup("Template Scenes")]
        public SceneConfig resolveBaseSceneConfig;
        [BoxGroup("Template Scenes")]
        public SceneConfig splashSceneConfig;
        [BoxGroup("Template Scenes")]
        public SceneConfig menuSceneConfig;
        [BoxGroup("Template Scenes")]
        public SceneConfig gameSceneConfig;

        [BoxGroup("All Scenes")]
        public List<SceneConfig> sceneConfigs;

        [Header("Utility Settings")]
        public float delayBeforeFirstSceneLoad;

        public SceneReference GetSceneReferenceByBuildIndex(int buildIndex)
        {
            SceneConfig sceneConfig = sceneConfigs.FirstOrDefault(s => s.sceneReference.BuildIndex == buildIndex);

            if (sceneConfig != null)
            {
                return sceneConfig.sceneReference;
            }

            return null;
        }
    }
}
