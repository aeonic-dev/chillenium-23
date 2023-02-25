using UnityEngine.SceneManagement;

namespace Util {
    public static class Scenes {
        public static Scene GetScene(this SceneReference sceneReference) =>
            SceneManager.GetSceneByPath(sceneReference.ScenePath);

        public static void LoadScene(this SceneReference sceneReference, LoadSceneMode mode = LoadSceneMode.Single) =>
            SceneManager.LoadScene(sceneReference.ScenePath, mode);

        public static void SetActiveScene(this SceneReference sceneReference) =>
            SceneManager.SetActiveScene(sceneReference.GetScene());
    }
}