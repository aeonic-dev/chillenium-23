using Core;
using UnityEngine;
using Util;

namespace UI {
    public class ExitButton : SpriteButton {
        public override void Click() {
            Application.Quit();
        }
    }
}