using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PrimitiveMessenger : MonoBehaviour
{
    private Dictionary<string, dynamic> dictionary;

    private static PrimitiveMessenger instance;
    public static PrimitiveMessenger Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(PrimitiveMessenger)) as PrimitiveMessenger;

                if (!instance)
                {
                    Debug.LogError("There needs to be one active PrimitiveMessenger script on a GameObject in your scene.");
                }
                else
                {
                    instance.Init();
                }
            }

            return instance;
        }
    }
    void Init()
    {
        if (dictionary == null)
        {
            dictionary = new Dictionary<string, dynamic>();
        }
    }
    public static void AddObject<T>(string name, T obj)
    {
        if (Instance.dictionary.TryGetValue(name, out dynamic temp))
        {
            return;
        }
        Instance.dictionary.Add(name, obj);
    }
    public static void EditObject<T>(string name, T obj)
    {
        Instance.dictionary[name] = obj;
    }
    public static dynamic GetObject(string name)
    {
        return Instance.dictionary[name];
    }
    public static void RemoveObject(string name)
    {
        if (instance == null || !Instance.dictionary.TryGetValue(name, out dynamic temp)) { return; }
        Instance.dictionary.Remove(name);
    }
}
