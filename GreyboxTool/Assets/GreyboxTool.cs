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

    public Vector3 cubeScale;
    public Vector3 handleOutput;

    SerializedObject serializedObject;
    SerializedProperty cubeScaleProperty;
    SerializedProperty handleOutputProperty;
    
    [MenuItem("My Tools/Greybox Creation")]
    public static void ShowWindow() => GetWindow(typeof(GreyboxTool));

    private GameObject folder;
    private List<GameObject> extensionHandles;


    private void OnEnable()
    {
        extensionHandles = new List<GameObject>();

        SceneView.duringSceneGui += DuringSceneGUI;

        folder = new GameObject();
        folder.name = "GreyBox Tool";

        cubeScale = Vector3.one;

        GameObject greyboxBase = CreateGreyCube(Vector3.zero);
        greyboxBase.transform.parent = folder.transform;

        serializedObject = new SerializedObject(this);
        cubeScaleProperty = serializedObject.FindProperty("cubeScale");
        handleOutputProperty = serializedObject.FindProperty("handleOutput");

    }


    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;

        if(folder)
            DestroyImmediate(folder);
    }

    public void OnGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(cubeScaleProperty);
        EditorGUILayout.PropertyField(handleOutputProperty);
        serializedObject.ApplyModifiedProperties();

        if(GUILayout.Button("Finalize"))
        {
            FinalizeGreybox();
        }
    }

    void DuringSceneGUI(SceneView sceneView)
    {
        //UpdateHandles(sceneView);

        if ((Event.current.modifiers & EventModifiers.Control) > 0)
        {
            return;
        }

        if (Selection.gameObjects.Length == 1 &&
            Selection.gameObjects[0].transform.parent != null &&
            Selection.gameObjects[0].transform.parent.parent != null &&
            Selection.gameObjects[0].transform.parent.parent.gameObject == folder)
        {
            string handleName = Selection.gameObjects[0].name;
            Transform parentTransform = Selection.gameObjects[0].transform.parent;
            GameObject obj = null;
            switch (handleName)
            {
                case "Up":
                    obj = CreateGreyCube(parentTransform.position + new Vector3(0.0f, (parentTransform.localScale.y / 2.0f + cubeScale.y / 2.0f), 0.0f));
                    break;
                case "Down":
                    obj = CreateGreyCube(Selection.gameObjects[0].transform.parent.position + new Vector3(0.0f, -(parentTransform.localScale.y / 2.0f + cubeScale.y / 2.0f), 0.0f));
                    break;
                case "Left":
                    obj = CreateGreyCube(Selection.gameObjects[0].transform.parent.position + new Vector3(-(parentTransform.localScale.x / 2.0f + cubeScale.x / 2.0f), 0.0f, 0.0f));
                    break;
                case "Right":
                    obj = CreateGreyCube(Selection.gameObjects[0].transform.parent.position + new Vector3((parentTransform.localScale.x / 2.0f + cubeScale.x / 2.0f), 0.0f, 0.0f));
                    break;
                case "Forward":
                    obj = CreateGreyCube(Selection.gameObjects[0].transform.parent.position + new Vector3(0.0f, 0.0f, (parentTransform.localScale.z / 2.0f + cubeScale.z / 2.0f)));
                    break;
                case "Backwards":
                    obj = CreateGreyCube(Selection.gameObjects[0].transform.parent.position + new Vector3(0.0f, 0.0f, -(parentTransform.localScale.z / 2.0f + cubeScale.z / 2.0f)));
                    break;

            }

            if(obj)
                Undo.RegisterCreatedObjectUndo(obj, "Expand Greybox");

            Selection.activeGameObject = null;
        }

        Vector3 output = Handles.Slider(folder.transform.position, Vector3.forward);

        if (output != Vector3.zero)
            handleOutput = output;

        if(handleOutput != Vector3.zero)
        {
            Handles.DrawWireCube(folder.transform.position + (folder.transform.localScale / 2.0f + handleOutput / 2.0f), handleOutput);
        }

    }

    GameObject CreateGreyCube(Vector3 loc)
    {
        GameObject folder = GameObject.Find("GreyBox Tool");
        if(folder == null)
        {
            folder = new GameObject();
            folder.name = "GreyBox Tool";
        }

        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.localScale = cubeScale;
        obj.transform.position = loc;
        obj.transform.parent = folder.transform;

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

    void UpdateHandles(SceneView sceneView)
    {
        Transform[] objs = folder.GetComponentsInChildren<Transform>();

        List<Transform> handles = new List<Transform>();
        for(int i = 0; i < objs.Length; i++)
        {
            Transform[] children = objs[i].GetComponentsInChildren<Transform>();

            handles.AddRange(children);
        }

        for(int i = 0; i < handles.Count; i++)
        {
            handles[i].gameObject.SetActive(true);
            Ray ray = new Ray(sceneView.camera.transform.position, handles[i].position - sceneView.camera.transform.position);
            Physics.Raycast(ray, out RaycastHit hit, Vector3.Distance(sceneView.camera.transform.position, handles[i].position));

            if(hit.collider != null)
            {
                if(hit.collider.name == handles[i].name)
                {
                    handles[i].gameObject.SetActive(true);
                }
                else
                {
                    handles[i].gameObject.SetActive(false);
                }
            }
            else
            {
                handles[i].gameObject.SetActive(false);
            }
        }
    }

    void FinalizeGreybox()
    {
        Transform[] list = folder.GetComponentsInChildren<Transform>();

        List<Transform> toBeDestroyed = new List<Transform>();

        foreach(Transform obj in list)
        {
            if (obj == folder.transform)
                continue;

            Transform[] handles = obj.GetComponentsInChildren<Transform>();
            foreach(Transform handle in handles)
            {
                if (handle == obj)
                    continue;

                toBeDestroyed.Add(handle);
            }
        }

        foreach (Transform t in toBeDestroyed)
            DestroyImmediate(t.gameObject);

        folder.name = "Final Greybox";
        folder = null;

        this.Close();
    }


}
