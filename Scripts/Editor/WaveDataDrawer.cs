using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WaveData))]
public class WaveDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty duringTime = property.FindPropertyRelative("duringTime");

        if (duringTime != null)
        {
            label.text = "Wave (" + duringTime.floatValue.ToString("0.##") + "s)";
        }

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}