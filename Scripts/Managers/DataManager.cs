using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting;


public class DataManager : MonoSingleton<DataManager>
{
    // 파일 경로 구성
    private static string GetPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName + ".json");
    }

    // 저장: 제네릭 T 타입을 Json으로 직렬화하여 파일에 쓴다.
    public static bool Save<T>(string fileName, T data, Formatting formatting = Formatting.Indented)
    {
        try
        {
            string path = GetPath(fileName);
            string json = JsonConvert.SerializeObject(data, formatting);
            File.WriteAllText(path, json);
            Debug.Log($"SaveSystemNewtonsoft: Saved \"{fileName}\" to {path}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"SaveSystemNewtonsoft Save Error: {ex}");
            return false;
        }
    }

    // 불러오기: 존재하면 역직렬화하여 반환, 없으면 default(T)
    public static T Load<T>(string fileName)
    {
        try
        {
            string path = GetPath(fileName);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"SaveSystemNewtonsoft: File not found: {path}");
                return default;
            }

            string json = File.ReadAllText(path);
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        catch (Exception ex)
        {
            Debug.LogError($"SaveSystemNewtonsoft Load Error: {ex}");
            return default;
        }
    }

    // 존재 확인
    public static bool Exists(string fileName)
    {
        return File.Exists(GetPath(fileName));
    }

    // 삭제
    public static bool Delete(string fileName)
    {
        try
        {
            string path = GetPath(fileName);
            if (File.Exists(path)) File.Delete(path);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"SaveSystemNewtonsoft Delete Error: {ex}");
            return false;
        }
    }
}
