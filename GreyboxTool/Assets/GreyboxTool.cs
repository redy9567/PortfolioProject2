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
        Backwards,
        DIRECTION_MAX
    }

    Vector3 halfScale = new Vector3(0.5f, 0.5f, 0.5f);
    
    [MenuItem("My Tools/Greybox Creation")]
    public static void ShowWindow() => GetWindow(typeof(GreyboxTool));

    private GameObject greyboxBase;
    private List<GameObject> extensionHandles;


    private void OnEnable()
    {
        extensionHandles = new List<GameObject>();

        SceneView.duringSceneGui += DuringSceneGUI;

        greyboxBase = CreateGreyCube(Vector3.zero);

    }


    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
        DestroyImmediate(greyboxBase);
    }

    public void OnGUI()
    {
        if(GUILayout.Button("Generate"))
        {
        }
    }

    void DuringSceneGUI(SceneView sceneView)
    {

        if (Selection.gameObjects.Length == 1 && Selection.gameObjects[0].transform.parent != null && Selection.gameObjects[0].transform.parent.gameObject == greyboxBase)
        {
            Direction dir = 0;
            for (int i = 0; i < (int)Direction.DIRECTION_MAX; i++)
            {
                if(extensionHandles[i] == Selection.gameObjects[0])
                {
                    dir = (Direction)i;
                    break;
                }    
            }

            switch(dir)
            {
                case Direction.Up:
                    CreateGreyCube(greyboxBase.transform.position + new Vector3(0.0f, 1.0f, 0.0f));
                    break;
                case Direction.Down:
                    CreateGreyCube(greyboxBase.transform.position + new Vector3(0.0f, -1.0f, 0.0f));
                    break;
                case Direction.Left:
                    CreateGreyCube(greyboxBase.transform.position + new Vector3(-1.0f, 0.0f, 0.0f));
                    break;
                case Direction.Right:
                    CreateGreyCube(greyboxBase.transform.position + new Vector3(1.0f, 0.0f, 0.0f));
                    break;
                case Direction.Forward:
                    CreateGreyCube(greyboxBase.transform.position + new Vector3(0.0f, 0.0f, 1.0f));
                    break;
                case Direction.Backwards:
                    CreateGreyCube(greyboxBase.transform.position + new Vector3(0.0f, 0.0f, -1.0f));
                    break;

            }

            Selection.activeGameObject = null;
        }

    }

    GameObject CreateGreyCube(Vector3 loc)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.position = loc;

        Material yellow = new Material(obj.GetComponent<MeshRenderer>().sharedMaterial);
        yellow.SetColor(Shader.PropertyToID("_Color"), new Color(0.75f, 0.75f, 0.0f));

        GameObject upHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        upHandle.transform.parent = obj.transform;
        upHandle.transform.localScale = halfScale;
        upHandle.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        upHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        upHandle.name = "Up";
        extensionHandles.Add(upHandle);

        GameObject downHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        downHandle.transform.parent = obj.transform;
        downHandle.transform.localScale = halfScale;
        downHandle.transform.localPosition = new Vector3(0.0f, -0.5f, 0.0f);
        downHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        downHandle.name = "Down";
        extensionHandles.Add(downHandle);

        GameObject leftHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leftHandle.transform.parent = obj.transform;
        leftHandle.transform.localScale = halfScale;
        leftHandle.transform.localPosition = new Vector3(-0.5f, 0.0f, 0.0f);
        leftHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        leftHandle.name = "Left";
        extensionHandles.Add(leftHandle);

        GameObject rightHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rightHandle.transform.parent = obj.transform;
        rightHandle.transform.localScale = halfScale;
        rightHandle.transform.localPosition = new Vector3(0.5f, 0.0f, 0.0f);
        rightHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        rightHandle.name = "Right";
        extensionHandles.Add(rightHandle);

        GameObject forwardHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        forwardHandle.transform.parent = obj.transform;
        forwardHandle.transform.localScale = halfScale;
        forwardHandle.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
        forwardHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        forwardHandle.name = "Forward";
        extensionHandles.Add(forwardHandle);

        GameObject backwardsHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        backwardsHandle.transform.parent = obj.transform;
        backwardsHandle.transform.localScale = halfScale;
        backwardsHandle.transform.localPosition = new Vector3(0.0f, 0.0f, -0.5f);
        backwardsHandle.GetComponent<MeshRenderer>().sharedMaterial = yellow;
        backwardsHandle.name = "Backwards";
        extensionHandles.Add(backwardsHandle);

        return obj;
    }

}
