using UnityEngine;
using Eflatun.SceneReference;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

namespace EEA.GameService
{
    [CreateAssetMenu(fileName = "SceneConfig", menuName = "GameServices/Scene/Scene Config", order = 1)]
    public class SceneConfig : ScriptableObject
    {
        public SceneReference sceneReference;
        public LoadSceneMode loadMode = LoadSceneMode.Additive;
        public bool removeAllOtherScenes = false;
        public bool showSceneTransition;
    }
}
