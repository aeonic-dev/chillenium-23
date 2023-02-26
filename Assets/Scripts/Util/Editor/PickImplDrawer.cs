using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Util;

namespace Editor {
    [CustomPropertyDrawer(typeof(PickImplAttribute))]
    public class PickImplDrawer : PropertyDrawer {
        private List<Type> subTypes;
        private string[] subTypeNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Rect popupRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect propertyRect;

            PickImplAttribute attr = attribute as PickImplAttribute;

            if (subTypes == null) {
                // Find subtypes and set names list here to avoid doing it every frame in the property drawer
                subTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => attr.baseType.IsAssignableFrom(p) && !p.IsAbstract).ToList();
                subTypeNames = subTypes.Select(t => t.Name).ToArray();
            }

            int typeIndex = property.managedReferenceValue == null
                ? 0
                : subTypes.IndexOf(property.managedReferenceValue.GetType());
            typeIndex = EditorGUI.Popup(popupRect, "Type", typeIndex, subTypeNames);

            if (property.managedReferenceValue == null ||
                typeIndex != subTypes.IndexOf(property.managedReferenceValue.GetType())) {
                property.managedReferenceValue = Activator.CreateInstance(subTypes[typeIndex]);
                property.serializedObject.ApplyModifiedProperties();
            }

            // Update property rect
            propertyRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 1.2f, position.width,
                EditorGUI.GetPropertyHeight(property));
            EditorGUI.PropertyField(propertyRect, property,
                new GUIContent("Definition: " + subTypeNames[typeIndex].Replace(" Action", "").Prettify()), true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.singleLineHeight * 1.2f;
        }
    }
}