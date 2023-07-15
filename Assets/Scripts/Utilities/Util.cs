using System;
using System.Collections.Generic;
using UnityEngine;

public static class Util 
{
    public static void SetPosition(this Transform transform, Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
    public static void SetX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    public static void SetY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
    public static Vector3Int ToVector3Int(Vector3 vector3)
    {
        return new Vector3Int((int)vector3.x, (int)vector3.y, (int)vector3.z);
    }
    public static Vector3 ToVector3(this Vector2 vector2, float z)
    {
        return new Vector3(vector2.x, vector2.y, z);
    }
    public static Vector2 ToVector2(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }
    public static string RemoveSpaces(this string text)
    {
        return text.Replace(" ", "");
    }
    public static string AddSpaces(this string text)
    {
        string finalText = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && i != 0)
            {
                finalText += " ";
            }
            finalText += text[i];
        }
        return finalText;
    }
}
