using UnityEngine;
using UnityEditor;

namespace Banana.UI.Popup
{
    [CustomEditor(typeof(PopupManager))]
    public class PopupManagerEditor : Editor
    {
        private string[] popupNames;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("popupRoot"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ignoreWhileTransition"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isOnlyOnePopup"));

            if (GUILayout.Button("Find Popups"))
            {
                var popupManager = (PopupManager)target;
                popupNames = popupManager.GetPopupNames();
            }

            if (popupNames != null)
            {
                foreach(var popupName in popupNames)
                {
                    EditorGUILayout.LabelField(popupName);
                }        
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}