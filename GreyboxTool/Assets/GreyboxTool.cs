using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GreyboxTool : EditorWindow
{

    Vector3 halfScale = new Vector3(0.5f, 0.5f, 0.5f);

    public float posXVal;
    public float negXVal;
    public float posYVal;
    public float negYVal;
    public float posZVal;
    public float negZVal;

    SerializedObject serializedObject;
    
    [MenuItem("My Tools/Greybox Creation")]
    public static void ShowWindow() => GetWindow(typeof(GreyboxTool));

    private GameObject folder;


    private void OnEnable()
    {

        SceneView.duringSceneGui += DuringSceneGUI;

        folder = new GameObject();
        folder.name = "GreyBox Tool";

        GameObject greyboxBase = CreateGreyCube(Vector3.zero, Vector3.one);
        greyboxBase.transform.parent = folder.transform;

        serializedObject = new SerializedObject(this);

        posXVal = 0.0f;
        negXVal = 0.0f;
        posYVal = 0.0f;
        negYVal = 0.0f;
        posZVal = 0.0f;
        negZVal = 0.0f;

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
        serializedObject.ApplyModifiedProperties();

        if(GUILayout.Button("Finalize"))
        {
            FinalizeGreybox();
        }
    }

    void DuringSceneGUI(SceneView sceneView)
    {

        if (Selection.gameObjects.Length == 1 &&
            Selection.gameObjects[0].transform.parent != null &&
            Selection.gameObjects[0].transform.parent.gameObject == folder)
        {

            GameObject selectedObject = Selection.gameObjects[0];
            Transform parentTransform = selectedObject.transform;

            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                if (posXVal != 0.0f)
                {
                    Vector3 newCubeScale = new Vector3(posXVal, selectedObject.transform.localScale.y, selectedObject.transform.localScale.z);
                    GameObject obj = CreateGreyCube(selectedObject.transform.position + new Vector3((parentTransform.localScale.x / 2.0f + newCubeScale.x / 2.0f), 0.0f, 0.0f), newCubeScale);
                    Undo.RegisterCreatedObjectUndo(obj, "Expand Greybox");
                    posXVal = 0.0f;
                }

                if (negXVal != 0.0f)
                {
                    Vector3 newCubeScale = new Vector3(-negXVal, selectedObject.transform.localScale.y, selectedObject.transform.localScale.z); //Invert negYVal for Positive Scale
                    GameObject obj = CreateGreyCube(selectedObject.transform.position + new Vector3(-(parentTransform.localScale.x / 2.0f + newCubeScale.x / 2.0f), 0.0f, 0.0f), newCubeScale);
                    Undo.RegisterCreatedObjectUndo(obj, "Expand Greybox");
                    negXVal = 0.0f;
                }

                if (posYVal != 0.0f)
                {
                    Vector3 newCubeScale = new Vector3(selectedObject.transform.localScale.x, posYVal, selectedObject.transform.localScale.z);
                    GameObject obj = CreateGreyCube(selectedObject.transform.position + new Vector3(0.0f, (parentTransform.localScale.y / 2.0f + newCubeScale.y / 2.0f), 0.0f), newCubeScale);
                    Undo.RegisterCreatedObjectUndo(obj, "Expand Greybox");
                    posYVal = 0.0f;
                }

                if (negYVal != 0.0f)
                {
                    Vector3 newCubeScale = new Vector3(selectedObject.transform.localScale.x, -negYVal, selectedObject.transform.localScale.z); //Invert negYVal for Positive Scale
                    GameObject obj = CreateGreyCube(selectedObject.transform.position + new Vector3(0.0f, -(parentTransform.localScale.y / 2.0f + newCubeScale.y / 2.0f), 0.0f), newCubeScale);
                    Undo.RegisterCreatedObjectUndo(obj, "Expand Greybox");
                    negYVal = 0.0f;
                }

                if (posZVal != 0.0f)
                {
                    Vector3 newCubeScale = new Vector3(selectedObject.transform.localScale.x, selectedObject.transform.localScale.y, posZVal);
                    GameObject obj = CreateGreyCube(selectedObject.transform.position + new Vector3(0.0f, 0.0f, (parentTransform.localScale.z / 2.0f + newCubeScale.z / 2.0f)), newCubeScale);
                    Undo.RegisterCreatedObjectUndo(obj, "Expand Greybox");
                    posZVal = 0.0f;
                }

                if (negZVal != 0.0f)
                {
                    Vector3 newCubeScale = new Vector3(selectedObject.transform.localScale.x, selectedObject.transform.localScale.y, -negZVal); //Invert negZVal for Positive Scale
                    GameObject obj = CreateGreyCube(selectedObject.transform.position + new Vector3(0.0f, 0.0f, -(parentTransform.localScale.z / 2.0f + newCubeScale.z / 2.0f)), newCubeScale);
                    Undo.RegisterCreatedObjectUndo(obj, "Expand Greybox");
                    negZVal = 0.0f;
                }

                Selection.activeGameObject = null;
            }

            Handles.color = Color.yellow;

            Vector3 PosXSliderLoc = new Vector3(posXVal + (selectedObject.transform.localScale.x / 2.0f + selectedObject.transform.position.x), selectedObject.transform.position.y, selectedObject.transform.position.z);
            Vector3 NegXSliderLoc = new Vector3(negXVal - (selectedObject.transform.localScale.x / 2.0f - selectedObject.transform.position.x), selectedObject.transform.position.y, selectedObject.transform.position.z);
            Vector3 PosYSliderLoc = new Vector3(selectedObject.transform.position.x, posYVal + (selectedObject.transform.localScale.y / 2.0f + selectedObject.transform.position.y), selectedObject.transform.position.z);
            Vector3 NegYSliderLoc = new Vector3(selectedObject.transform.position.x, negYVal - (selectedObject.transform.localScale.y / 2.0f - selectedObject.transform.position.y), selectedObject.transform.position.z);
            Vector3 PosZSliderLoc = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y, posZVal + (selectedObject.transform.localScale.z/2.0f + selectedObject.transform.position.z)); 
            Vector3 NegZSliderLoc = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y, negZVal - (selectedObject.transform.localScale.z/2.0f - selectedObject.transform.position.z));

            posXVal = Handles.Slider(PosXSliderLoc, Vector3.right).x - (selectedObject.transform.localScale.x / 2.0f + selectedObject.transform.position.x);
            negXVal = Handles.Slider(NegXSliderLoc, Vector3.left).x + (selectedObject.transform.localScale.x / 2.0f - selectedObject.transform.position.x);
            posYVal = Handles.Slider(PosYSliderLoc, Vector3.up).y - (selectedObject.transform.localScale.y / 2.0f + selectedObject.transform.position.y);
            negYVal = Handles.Slider(NegYSliderLoc, Vector3.down).y + (selectedObject.transform.localScale.y / 2.0f - selectedObject.transform.position.y);
            posZVal = Handles.Slider(PosZSliderLoc, Vector3.forward).z - (selectedObject.transform.localScale.z/2.0f + selectedObject.transform.position.z); 
            negZVal = Handles.Slider(NegZSliderLoc, Vector3.back).z + (selectedObject.transform.localScale.z/2.0f - selectedObject.transform.position.z);

            if (posXVal < 0.0f)
                posXVal = 0.0f;

            if (negXVal > 0.0f)
                posXVal = 0.0f;

            if (posYVal < 0.0f)
                posYVal = 0.0f;

            if (negYVal > 0.0f)
                posYVal = 0.0f;

            if (posZVal < 0.0f)
                posZVal = 0.0f;

            if (negZVal > 0.0f)
                posZVal = 0.0f;


            if (posXVal != 0.0f)
            {
                Vector3 PosXCubeSize = new Vector3(posXVal, selectedObject.transform.localScale.y, selectedObject.transform.localScale.z);
                Handles.DrawWireCube(selectedObject.transform.position + new Vector3((selectedObject.transform.localScale.x / 2.0f + posXVal / 2.0f), 0.0f, 0.0f), PosXCubeSize);
            }
            if (negXVal != 0.0f)
            {
                Vector3 NegXCubeSize = new Vector3(-negXVal, selectedObject.transform.localScale.y, selectedObject.transform.localScale.z);
                Handles.DrawWireCube(selectedObject.transform.position + new Vector3(-(selectedObject.transform.localScale.x / 2.0f - negXVal / 2.0f), 0.0f, 0.0f), NegXCubeSize);
            }
            if (posYVal != 0.0f)
            {
                Vector3 PosYCubeSize = new Vector3(selectedObject.transform.localScale.x, posYVal, selectedObject.transform.localScale.z);
                Handles.DrawWireCube(selectedObject.transform.position + new Vector3(0.0f, (selectedObject.transform.localScale.y / 2.0f + posYVal / 2.0f), 0.0f), PosYCubeSize);
            }
            if (negYVal != 0.0f)
            {
                Vector3 NegYCubeSize = new Vector3(selectedObject.transform.localScale.x, -negYVal, selectedObject.transform.localScale.z);
                Handles.DrawWireCube(selectedObject.transform.position + new Vector3(0.0f, -(selectedObject.transform.localScale.y / 2.0f - negYVal / 2.0f), 0.0f), NegYCubeSize);
            }
            if (posZVal != 0.0f)
            {
                Vector3 PosZCubeSize = new Vector3(selectedObject.transform.localScale.x, selectedObject.transform.localScale.y, posZVal);
                Handles.DrawWireCube(selectedObject.transform.position + new Vector3(0.0f, 0.0f, (selectedObject.transform.localScale.z / 2.0f + posZVal / 2.0f)), PosZCubeSize);
            }
            if (negZVal != 0.0f)
            {
                Vector3 NegZCubeSize = new Vector3(selectedObject.transform.localScale.x, selectedObject.transform.localScale.y, -negZVal);
                Handles.DrawWireCube(selectedObject.transform.position + new Vector3(0.0f, 0.0f, -(selectedObject.transform.localScale.z / 2.0f - negZVal / 2.0f)), NegZCubeSize);
            }
        }

        

        if ((Event.current.modifiers & EventModifiers.Control) > 0)
        {
            return;
        }

        
    }

    GameObject CreateGreyCube(Vector3 loc, Vector3 scale)
    {
        GameObject folder = GameObject.Find("GreyBox Tool");
        if (folder == null)
        {
            folder = new GameObject();
            folder.name = "GreyBox Tool";
        }

        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.localScale = scale;
        obj.transform.position = loc;
        obj.transform.parent = folder.transform;

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
