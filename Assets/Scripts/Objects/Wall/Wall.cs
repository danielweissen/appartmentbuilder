using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this and everythign included are just numbers
public class Wall : MonoBehaviour {

    private WallData wallData;
    private WallUI wallUI;

    private Wall prev;
    private Wall succ;



    public void SetWall(Vector3 startPoint, Vector3 endPoint, Wall prev) {
        this.wallData = new WallData(startPoint, endPoint);
        this.wallUI = new WallUI(this.wallData);
        this.prev = prev;
    }

    public void SetSuccessorWall(Wall succ) {
        this.succ = succ;
    }

    public Vector3[] GetWallMesh() {
        return this.wallData.getConnectionMeshVectors();
    }

    public Vector3[] GetSelectionMesh() {
        return this.wallUI.GetSelectionMesh();
    }

    public Vector3 GetConnectionPoint() {
        return this.wallData.getEndMiddlePoint();
    }

    public void ConnectWalls() {
        WallOperations.ConnectWalls(prev.wallData, this.wallData);
        this.wallUI.UpdateSelectionOutline();
        prev.wallUI.UpdateSelectionOutline();
        this.wallUI.UpdateEdgeReferencePoints();
        prev.wallUI.UpdateEdgeReferencePoints();
    }

    public void MoveRightEdge(Vector3 position) {
        Vector3 startRight = this.wallData.getConnectionMeshVectors()[1];
        Vector3 endRight = this.wallData.getConnectionMeshVectors()[3];
        Vector3 newStartRight = VectorOperations.Intersection(position, this.wallData.getDirection(), startRight, this.wallData.getNormal());
        Vector3 newEndRight = VectorOperations.Intersection(position, this.wallData.getDirection(), endRight, this.wallData.getNormal());
        if (!prev && !succ) {

        } else if(!prev && succ){
            newEndRight = VectorOperations.Intersection(newEndRight, this.wallData.getDirection(), endRight, succ.wallData.getDirection());
            succ.wallData.SetStartRight(newEndRight);
            succ.UpdateWall();
        } else if(prev && !succ) {
            newStartRight = VectorOperations.Intersection(newStartRight, this.wallData.getDirection(), startRight, prev.wallData.getDirection());
            prev.wallData.SetEndRight(newStartRight);
            prev.UpdateWall();
        } else if(prev && succ) {
            newStartRight = VectorOperations.Intersection(newStartRight, this.wallData.getDirection(), startRight, prev.wallData.getDirection());
            newEndRight = VectorOperations.Intersection(newEndRight, this.wallData.getDirection(), endRight, succ.wallData.getDirection());
            succ.wallData.SetStartRight(newEndRight);
            prev.wallData.SetEndRight(newStartRight);
            succ.UpdateWall();
            prev.UpdateWall();
        }
        this.wallData.SetRightEdge(newStartRight, newEndRight);
        this.UpdateWall();
    }

    public void MoveLeftEdge(Vector3 position) {
        Vector3 startRight = this.wallData.getConnectionMeshVectors()[0];
        Vector3 endRight = this.wallData.getConnectionMeshVectors()[2];
        Vector3 newStartRight = VectorOperations.Intersection(position, this.wallData.getDirection(), startRight, this.wallData.getNormal());
        Vector3 newEndRight = VectorOperations.Intersection(position, this.wallData.getDirection(), endRight, this.wallData.getNormal());
        if (!prev && !succ) {

        } else if (!prev && succ) {
            newEndRight = VectorOperations.Intersection(newEndRight, this.wallData.getDirection(), endRight, succ.wallData.getDirection());
            succ.wallData.SetStartLeft(newEndRight);
            succ.UpdateWall();
        } else if (prev && !succ) {
            newStartRight = VectorOperations.Intersection(newStartRight, this.wallData.getDirection(), startRight, prev.wallData.getDirection());
            prev.wallData.SetEndLeft(newStartRight);
            prev.UpdateWall();
        } else if (prev && succ) {
            newStartRight = VectorOperations.Intersection(newStartRight, this.wallData.getDirection(), startRight, prev.wallData.getDirection());
            newEndRight = VectorOperations.Intersection(newEndRight, this.wallData.getDirection(), endRight, succ.wallData.getDirection());
            succ.wallData.SetStartLeft(newEndRight);
            prev.wallData.SetEndLeft(newStartRight);
            succ.UpdateWall();
            prev.UpdateWall();
        }
        this.wallData.SetLeftEdge(newStartRight, newEndRight);
        this.UpdateWall();
    }

    public void UpdateWall() {
        this.wallData.UpdateData();
        this.wallUI.UpdateSelectionOutline();
        this.wallUI.UpdateEdgeReferencePoints();
    }

    public float GetWallLength() {
        return this.wallData.getWallVector().magnitude;
    }

    public float GetWallWidth() {
        return this.wallData.GetWallWidth();
    }

    public Vector3 GetRightSelectionPoint() {
        return this.wallUI.GetEdgeRightReferencePoint();
    }
    public Vector3 GetLeftSelectionPoint() {
        return this.wallUI.GetEdgeLeftReferencePoint();
    }
    public Vector3 GetStartSelectionPoint() {
        return this.wallUI.GetEdgeStartReferencePoint();
    }
    public Vector3 GetEndSelectionPoint() {
        return this.wallUI.GetEdgeEndReferencePoint();
    }

    public Wall GetPrev() {
        return this.prev;
    }

    public Wall GetSucc() {
        return this.succ;
    }

}
