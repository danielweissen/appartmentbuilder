using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this is the unity perspective of a wall
public class GameObjectOperations {


    public static void MoveRightEdge(GameObject UIRight, Vector3 newPos) {
        Wall wallScript = UIRight.transform.parent.parent.gameObject.GetComponent<Wall>();
        wallScript.MoveRightEdge(newPos);
        RebuildWall(wallScript.gameObject);
        if (wallScript.GetPrev()) {
            RebuildWall(wallScript.GetPrev().gameObject);
        }
        if(wallScript.GetSucc()) {
            RebuildWall(wallScript.GetSucc().gameObject);
        }
    }

    public static void MoveLeftEdge(GameObject UILeft, Vector3 newPos) {
        Wall wallScript = UILeft.transform.parent.parent.gameObject.GetComponent<Wall>();
        wallScript.MoveLeftEdge(newPos);
        RebuildWall(UILeft.transform.parent.parent.gameObject);
        if (wallScript.GetPrev()) {
            RebuildWall(wallScript.GetPrev().gameObject);
        }
        if (wallScript.GetSucc()) {
            RebuildWall(wallScript.GetSucc().gameObject);
        }
    }
    public static GameObject CreateWall(Vector3 startPoint, Vector3 endPoint, Wall prev, Material wallMaterial, Material selectionMaterial, Material uiMaterial, GameObject UIPrefab) {
        GameObject wall = new GameObject();
        Wall wallScript = wall.AddComponent<Wall>();

        if (prev == null) {
            wallScript.SetWall(startPoint, endPoint, prev);
        } else {
            wallScript.SetWall(prev.GetConnectionPoint(), endPoint, prev);
            prev.SetSuccessorWall(wallScript);
            wallScript.ConnectWalls();
            RebuildWall(prev.gameObject);
        }

        Vector3[] wallMeshVertecies = wallScript.GetWallMesh();
        Vector3 transformPosition = VectorOperations.GetCentreOfQuad(wallMeshVertecies[0], wallMeshVertecies[1], wallMeshVertecies[2], wallMeshVertecies[3]);

        wall.transform.position = transformPosition;

        GameObject wallMesh = CreateQuad(wallMeshVertecies, wallMaterial, transformPosition);
        GameObject selectionMesh = CreateSelectionOutline(wallScript.GetSelectionMesh(), selectionMaterial, transformPosition);
        GameObject collider = CreateCollider(wallMeshVertecies, wallScript.GetConnectionPoint(), wallScript.GetWallLength(), wallScript.GetWallWidth(), transformPosition);
        GameObject wallUI = CreateWallUI(wallScript, uiMaterial, UIPrefab, transformPosition);

        wall.name = "Wall";
        selectionMesh.name = "SelectionMesh";
        wallMesh.name = "WallMesh";
        collider.tag = "Wall";
        collider.name = "WallCollider";
        wallUI.name = "WallUI";

        wallMesh.transform.parent = wall.transform;
        selectionMesh.transform.parent = wall.transform;
        collider.transform.parent = wall.transform;
        wallUI.transform.parent = wall.transform;

        return wall;
    }

    public static void SetUI(GameObject wall, bool val) {
        wall.transform.parent.GetChild(1).gameObject.SetActive(val);
        wall.transform.parent.GetChild(3).gameObject.SetActive(val);
    }

    //TODO: inefficient / dependent on CreateWall()
    private static GameObject RebuildWall(GameObject wall) {
        GameObject wallMesh = wall.transform.GetChild(0).gameObject;
        GameObject selectionMesh = wall.transform.GetChild(1).gameObject;
        GameObject collider = wall.transform.GetChild(2).gameObject;
        GameObject ui = wall.transform.GetChild(3).gameObject;

        Wall wallScript = wall.GetComponent<Wall>();
        Vector3[] wallMeshVertecies = wallScript.GetWallMesh();
        Vector3[] selectionMeshVertecies = wallScript.GetSelectionMesh();

        Vector3 transformPosition = VectorOperations.GetCentreOfQuad(wallMeshVertecies[0], wallMeshVertecies[1], wallMeshVertecies[2], wallMeshVertecies[3]);

        UpdateTransformPosition(wall, transformPosition);
        UpdateTransformPosition(wallMesh, transformPosition);
        UpdateTransformPosition(selectionMesh, transformPosition);
        UpdateTransformPosition(collider, transformPosition);
        UpdateTransformPosition(ui, transformPosition);

        RebuildMesh(wallMesh, wallMeshVertecies);
        RebuildMesh(selectionMesh, selectionMeshVertecies);
        ResetUI(ui, wallScript);


        return wall;
    }

    private static GameObject CreateQuad(Vector3[] vertexArray, Material material, Vector3 transormPosition) {
        GameObject quad = new GameObject();
        UpdateTransformPosition(quad, transormPosition);
        SetUpMesh(vertexArray,FaceData.topFacingQuadFace,material,quad);
        return quad;
    }

    private static GameObject CreateSelectionOutline(Vector3[] vertexArray, Material material, Vector3 transormPosition) {
        GameObject newObject = new GameObject();
        newObject.SetActive(false);
        UpdateTransformPosition(newObject, transormPosition);
        SetUpMesh(vertexArray, FaceData.topFacingSelectionFace, material, newObject);
        return newObject;
    }

    private static Vector3[] TransformWorldToLocal(Vector3[] vertexArray, GameObject quad) {
        Vector3[] local = new Vector3[vertexArray.Length];
        for(int i = 0; i < vertexArray.Length; i++) {
            local[i] = quad.transform.InverseTransformPoint(vertexArray[i]);
        }
        return local;
    }

    private static Vector3 TransformWorldToLocal(Vector3 vertex, GameObject quad) {
        return quad.transform.InverseTransformPoint(vertex);
    }

    private static void SetUpMesh(Vector3[] vertexArray, int[] faceData, Material material, GameObject newObject) {
        MeshFilter meshFilter = newObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.MarkDynamic();
        Vector3[] localMeshVertecies = TransformWorldToLocal(vertexArray, newObject);
        meshFilter.mesh.vertices = localMeshVertecies;
        meshFilter.mesh.triangles = faceData;
        meshFilter.mesh.RecalculateNormals();
    }

    private static void RebuildMesh(GameObject gameObject, Vector3[] vertexArray) {
        Vector3[] localMeshVertecies = TransformWorldToLocal(vertexArray, gameObject);
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.vertices = localMeshVertecies;
        mesh.RecalculateNormals();
    }

    private static GameObject CreateCollider(Vector3[] vertexArray, Vector3 endPoint, float wallLength, float wallWidth, Vector3 transormPosition) {
        GameObject colliderObject = new GameObject();
        UpdateTransformPosition(colliderObject, transormPosition);
        colliderObject.AddComponent<BoxCollider>();
        colliderObject.AddComponent <Collision>();
        colliderObject.transform.localScale = new Vector3(wallWidth,0.1f,wallLength);
        colliderObject.transform.LookAt(endPoint);
        return colliderObject;
    }

    private static GameObject CreateWallUI(Wall wallScript, Material uiMaterial, GameObject UIPrefab, Vector3 transormPosition) {
        GameObject uiParent = new GameObject();
        GameObject uiObjectLeft = new GameObject();
        GameObject uiObjectRight = new GameObject();
        GameObject uiObjectStart = new GameObject();
        GameObject uiObjectEnd = new GameObject();
        UpdateTransformPosition(uiParent, transormPosition);
        uiObjectLeft.transform.parent = uiParent.transform;
        uiObjectRight.transform.parent = uiParent.transform;
        uiObjectStart.transform.parent = uiParent.transform;
        uiObjectEnd.transform.parent = uiParent.transform;
        uiParent.SetActive(false);
        uiObjectLeft.transform.position = wallScript.GetLeftSelectionPoint();
        uiObjectRight.transform.position = wallScript.GetRightSelectionPoint();
        uiObjectStart.transform.position = wallScript.GetStartSelectionPoint();
        uiObjectEnd.transform.position = wallScript.GetEndSelectionPoint();
        SetUpMesh(uiObjectLeft, uiMaterial, UIPrefab);
        SetUpMesh(uiObjectRight, uiMaterial, UIPrefab);
        SetUpMesh(uiObjectStart, uiMaterial, UIPrefab);
        SetUpMesh(uiObjectEnd, uiMaterial, UIPrefab);
        uiObjectLeft.AddComponent<BoxCollider>();
        uiObjectRight.AddComponent<BoxCollider>();
        uiObjectStart.AddComponent<BoxCollider>();
        uiObjectEnd.AddComponent<BoxCollider>();
        uiObjectLeft.tag = "UILeft";
        uiObjectRight.tag = "UIRight";
        uiObjectStart.tag = "UIStart";
        uiObjectEnd.tag = "UIEnd";
        uiObjectLeft.name = "UILeft";
        uiObjectRight.name = "UIRight";
        uiObjectStart.name = "UIStart";
        uiObjectEnd.name = "UIEnd";
        return uiParent;
    }


    private static void UpdateTransformPosition(GameObject gameObject, Vector3 position) {
        gameObject.transform.position = position;
    }

    private static void SetUpMesh(GameObject newObject, Material material, GameObject UIPrefab) {
        MeshFilter meshFilter = newObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        meshFilter.mesh = UIPrefab.GetComponent<MeshFilter>().sharedMesh;
        newObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }


    private static void ResetUI(GameObject ui, Wall wallScript) {
        GameObject uileft = ui.transform.GetChild(0).gameObject;
        GameObject uiright = ui.transform.GetChild(1).gameObject;
        GameObject uistart = ui.transform.GetChild(2).gameObject;
        GameObject uiend = ui.transform.GetChild(3).gameObject;

        uileft.transform.position = wallScript.GetLeftSelectionPoint();
        uiright.transform.position = wallScript.GetRightSelectionPoint();
        uistart.transform.position = wallScript.GetStartSelectionPoint();
        uiend.transform.position = wallScript.GetEndSelectionPoint();
    }

}
