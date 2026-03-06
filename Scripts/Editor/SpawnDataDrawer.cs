using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpawnData))]
public class SpawnDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty spawnTime = property.FindPropertyRelative("spawnTime");

        if (spawnTime != null)
        {
            label.text = "Spawn (" + spawnTime.floatValue.ToString("0.##") + "s)";
        }

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}