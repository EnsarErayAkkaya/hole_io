using UnityEngine;
using Eflatun.SceneReference;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

namespace EEA.BaseService
{
    [CreateAssetMenu(fileName = "SceneConfig", menuName = "BaseServices/Scene/Scene Config", order = 1)]
    public class SceneConfig : ScriptableObject
    {
        public SceneReference sceneReference;
        public LoadSceneMode loadMode = LoadSceneMode.Additive;
        public bool removeAllOtherScenes = false;
        public bool showSceneTransition;
    }
}
