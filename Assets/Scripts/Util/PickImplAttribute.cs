using System;
using UnityEngine;

namespace Util {
    public class PickImplAttribute : PropertyAttribute {
        public Type baseType;

        public PickImplAttribute(Type baseType) {
            this.baseType = baseType;
        }
    }
}