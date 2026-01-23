using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(DetailRandomizer))]
public class DetailRandomizerEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    DetailRandomizer script = (DetailRandomizer)target;

    //    // 기본 인스펙터 그리기
    //    DrawDefaultInspector();

    //    GUILayout.Space(15);
    //    if (GUILayout.Button("1. 모든 프리뷰 업데이트", GUILayout.Height(30)))
    //    {
    //        script.UpdatePreviews();
    //        // 에디터 윈도우 강제 리프레시
    //        EditorUtility.SetDirty(script);
    //        AssetDatabase.Refresh();
    //    }

    //    GUILayout.Space(10);

    //    for (int i = 0; i < script.biomeDetails.Count; i++)
    //    {
    //        var detail = script.biomeDetails[i];

    //        EditorGUILayout.BeginVertical("helpbox");
    //        EditorGUILayout.LabelField($"바이옴 {i}: {detail.name}", EditorStyles.boldLabel);

    //        if (detail.previewTexture != null)
    //        {
    //            // GUI 그리기 영역 확보 및 명확한 인스턴스 출력
    //            Rect rect = GUILayoutUtility.GetRect(128, 128);
    //            GUI.DrawTexture(rect, detail.previewTexture, ScaleMode.ScaleToFit);
    //        }
    //        else
    //        {
    //            EditorGUILayout.HelpBox("텍스처가 없습니다. 업데이트 버튼을 누르세요.", MessageType.None);
    //        }
    //        EditorGUILayout.EndVertical();
    //        GUILayout.Space(5);
    //    }

    //    GUILayout.Space(15);
    //    GUI.color = Color.cyan;
    //    if (GUILayout.Button("2. 터레인에 실제 생성 (Generate)", GUILayout.Height(40)))
    //    {
    //        script.Generate();
    //    }

    //    GUI.color = Color.white;
    //    if (GUILayout.Button("전체 삭제 (Clear)"))
    //    {
    //        script.ClearAll();
    //    }
    //}
}
#endif