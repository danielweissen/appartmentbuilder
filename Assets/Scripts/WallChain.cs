using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChain : MonoBehaviour{

    int wallCount;
    [SerializeField] Material test;
    [SerializeField] Material test2;
    [SerializeField] GameObject UIPrefab;

    List<GameObject> wallChain = new List<GameObject>();


    public void AddWall(Vector3 startPoint, Vector3 endPoint) {
        if (wallCount == 0) {
            wallCount++;
            CreateFirstWallMesh(startPoint, endPoint);
        } else {
            Wall oldWall = wallChain[wallCount - 1].GetComponent<Wall>();
            wallCount++;
            CreateSubsequentWallMesh(oldWall, startPoint, endPoint);
        }

    }

    private void CreateFirstWallMesh(Vector3 startPoint, Vector3 endPoint) {
        wallChain.Add(GameObjectOperations.CreateWall(startPoint, endPoint, null, test, test2, test2, UIPrefab));
    }



    private void CreateSubsequentWallMesh(Wall oldWall, Vector3 startPoint, Vector3 endPoint) {
        wallChain.Add(GameObjectOperations.CreateWall(startPoint, endPoint, oldWall, test, test2, test2, UIPrefab));
    }
}
