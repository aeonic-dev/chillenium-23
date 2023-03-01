using Util;

namespace UI {
    public class StartButton : SpriteButton {
        public SceneReference hubScene;

        public override void Click() {
            hubScene.LoadScene();
        }
    }
}