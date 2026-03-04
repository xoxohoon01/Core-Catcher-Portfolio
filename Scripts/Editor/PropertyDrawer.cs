using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Attribute))]
public class AttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var typeProp = property.FindPropertyRelative("type");

        label.text = typeProp.enumDisplayNames[typeProp.enumValueIndex];

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}