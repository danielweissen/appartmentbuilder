using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallUI {

    private WallData wallData;
    private WallEdgeUIObject edgeEndSelectionUIObject;
    private WallEdgeUIObject edgeStartSelectionUIObject;
    private Vector3 edgeEndReferencePoint;
    private Vector3 edgeStartReferencePoint;
    private Vector3 edgeLeftReferencePoint;
    private Vector3 edgeRightReferencePoint;

    private float selectionYOffest = 0.001f;

    private Vector3[] selectionMesh;

    public WallUI(WallData wall) {
        this.wallData = wall;
        UpdateEdgeReferencePoints();
        CreateEdgeSelectionUIObject();
        UpdateSelectionOutline();
    }

    public void UpdateSelectionOutline() {
        Vector3[] selectionInner = new Vector3[4];
        Vector3[] selectionOuter;

        for(int i = 0; i < selectionInner.Length; i++) {
            selectionInner[i] = this.wallData.getConnectionMeshVectors()[i] + new Vector3(0, selectionYOffest, 0);
        }

        selectionOuter = WallOperations.CalculateWallSelectionMesh(selectionInner[0], selectionInner[1], selectionInner[2], selectionInner[3], 0.05f);

        this.selectionMesh = new Vector3[] { selectionInner[0] , selectionInner[1], selectionInner[2], selectionInner[3], selectionOuter[0], selectionOuter[1], selectionOuter[2], selectionOuter[3] };
    }


    public void UpdateEdgeReferencePoints() {
        Vector3[] wallMesh = this.wallData.getConnectionMeshVectors();
        this.edgeEndReferencePoint = wallMesh[2] + 0.5f * (wallMesh[3] - wallMesh[2]);
        this.edgeStartReferencePoint = wallMesh[0] + 0.5f * (wallMesh[1] - wallMesh[0]);
        this.edgeLeftReferencePoint = wallMesh[0] + 0.5f * (wallMesh[2] - wallMesh[0]);
        this.edgeRightReferencePoint = wallMesh[1] + 0.5f * (wallMesh[3] - wallMesh[1]);
    }

    private void CreateEdgeSelectionUIObject() {
        edgeEndSelectionUIObject = new WallEdgeUIObject(edgeEndReferencePoint, 0.1f);
        edgeStartSelectionUIObject = new WallEdgeUIObject(edgeStartReferencePoint, 0.1f);
    }

    public Vector3[] GetSelectionMesh() {
        return this.selectionMesh;
    }
    public Vector3 GetEdgeRightReferencePoint() {
        return this.edgeRightReferencePoint;
    }
    public Vector3 GetEdgeLeftReferencePoint() {
        return this.edgeLeftReferencePoint;
    }
    public Vector3 GetEdgeStartReferencePoint() {
        return this.edgeStartReferencePoint;
    }
    public Vector3 GetEdgeEndReferencePoint() {
        return this.edgeEndReferencePoint;
    }

}




//this.leftSelectionMesh = new Vector3[] { selectionOuter[0], selectionInner[0], selectionOuter[2], selectionInner[2]};
//this.topSelectionMesh = new Vector3[] { selectionInner[2], selectionInner[3], selectionOuter[2], selectionOuter[3] };
//this.rightSelectionMesh = new Vector3[] { selectionInner[1], selectionOuter[1], selectionInner[3], selectionOuter[3] };
//this.bottomSelectionMesh = new Vector3[] { selectionOuter[0], selectionOuter[1], selectionInner[0], selectionInner[1]};
