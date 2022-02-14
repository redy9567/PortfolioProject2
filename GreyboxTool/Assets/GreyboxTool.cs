using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GreyboxTool : EditorWindow
{
    
    [MenuItem("My Tools/Greybox Creation")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GreyboxTool));
    }

    public void OnGUI()
    {
        if(GUILayout.Button("Generate"))
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = Vector3.zero;
        }
    }

}
