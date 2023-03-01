using UnityEngine;

namespace UI {
    public class ExitButton : SpriteButton {
        public override void Click() {
            Application.Quit();
        }
    }
}