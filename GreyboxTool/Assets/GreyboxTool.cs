using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GreyboxTool : EditorWindow
{

    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Backwards
    }

    Vector3 halfScale = new Vector3(0.5f, 0.5f, 0.5f);
    
    [MenuItem("My Tools/Greybox Creation")]
    public static void ShowWindow() => GetWindow(typeof(GreyboxTool));

    private GameObject greyboxBase;
    private List<GameObject> extensionHandles;


    private void OnEnable()
    {
        extensionHandles = new List<GameObject>();

        greyboxBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        greyboxBase.transform.position = Vector3.zero;

        Material yellow = new Material(greyboxBase.GetComponent<MeshRenderer>().sharedMaterial);
        yellow.SetColor(Shader.PropertyToID("_Color"), new Color(0.75f, 0.75f, 0.0f));

        GameObject upHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        upHandle.transform.parent = greyboxBase.transform;
        upHandle.transform.localScale = halfScale;
        upHandle.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        upHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        upHandle.name = "Up";
        extensionHandles.Add(upHandle);

        GameObject downHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        downHandle.transform.parent = greyboxBase.transform;
        downHandle.transform.localScale = halfScale;
        downHandle.transform.localPosition = new Vector3(0.0f, -0.5f, 0.0f);
        downHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        downHandle.name = "Down";
        extensionHandles.Add(downHandle);

        GameObject leftHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leftHandle.transform.parent = greyboxBase.transform;
        leftHandle.transform.localScale = halfScale;
        leftHandle.transform.localPosition = new Vector3(-0.5f, 0.0f, 0.0f);
        leftHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        leftHandle.name = "Left";
        extensionHandles.Add(leftHandle);

        GameObject rightHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rightHandle.transform.parent = greyboxBase.transform;
        rightHandle.transform.localScale = halfScale;
        rightHandle.transform.localPosition = new Vector3(0.5f, 0.0f, 0.0f);
        rightHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        rightHandle.name = "Right";
        extensionHandles.Add(rightHandle);

        GameObject forwardHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        forwardHandle.transform.parent = greyboxBase.transform;
        forwardHandle.transform.localScale = halfScale;
        forwardHandle.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
        forwardHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        forwardHandle.name = "Forward";
        extensionHandles.Add(forwardHandle);

        GameObject backwardsHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        backwardsHandle.transform.parent = greyboxBase.transform;
        backwardsHandle.transform.localScale = halfScale;
        backwardsHandle.transform.localPosition = new Vector3(0.0f, 0.0f, -0.5f);
        backwardsHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        backwardsHandle.name = "Backwards";
        extensionHandles.Add(backwardsHandle);

    }


    private void OnDisable()
    {
        DestroyImmediate(greyboxBase);
    }

    public void OnGUI()
    {
        if(GUILayout.Button("Generate"))
        {
        }
    }

}
