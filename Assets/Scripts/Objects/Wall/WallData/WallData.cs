using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallData    {

    // point clicked on by user, edge of wall
    private Vector3 startPoint;
    // point clicked on by user, edge of wall
    private Vector3 endPoint;
    // point calculated, used for wall connection
    private Vector3 startMiddlePoint;
    // point calculated, used for wall connection
    private Vector3 endMiddlePoint;
    // scaled direction vector of wall
    private Vector3 direction;
    // unscaled direction vector of wall
    private Vector3 wallVector;
    // scaled normal of direction
    private Vector3 normal;

    //0=sf 1=sr 2=el 3=er
    //represents original / unconnected state of wall
    private Vector3[] meshVectors;
    //represents connceted state of wall
    private Vector3[] meshConnectionVectors;

    [SerializeField] private float rightWidth = 0.3f;
    [SerializeField] private float leftWidth = 0.3f;
    [SerializeField] private float middlePointOffset = 0.2f;
    [SerializeField] private float wallHeight = 2f;
    private float wallWidth;

    private float realWorldLength;
    private float realWorldWidth;

    public WallData(Vector3 startPoint, Vector3 endPoint) {

        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.wallVector = endPoint - startPoint;
        this.direction = Vector3.Normalize(this.wallVector);
        this.normal = new Vector3(-direction.z, direction.y, direction.x);
        this.startMiddlePoint = this.startPoint + (this.direction * middlePointOffset);
        this.endMiddlePoint = this.endPoint - (this.direction * middlePointOffset);

        wallWidth = this.rightWidth + this.leftWidth;

        setUpMeshVectors();
        setUpConnectionMeshVectors();
    }

    private void setUpMeshVectors() {
        this.meshVectors = WallOperations.CalculateWallMesh(startPoint, endPoint, direction, 0f, normal, leftWidth, rightWidth, 0f, wallHeight);
    }

    private void setUpConnectionMeshVectors() {
        this.meshConnectionVectors = new Vector3[] { meshVectors[0], meshVectors[1], meshVectors[2], meshVectors[3] };
    }

    public Vector3[] getMeshVectors() {
        return this.meshVectors;
    }

    public Vector3[] getConnectionMeshVectors() {
        return this.meshConnectionVectors;
    }

    public Vector3 getStartPoint() {
        return this.startPoint;
    }

    public Vector3 getEndPoint() {
        return this.endPoint;
    }

    public Vector3 getNormal() {
        return this.normal;
    }

    public Vector3 getDirection() {
        return this.direction;
    }
    public Vector3 getStartMiddlePoint() {
        return this.startMiddlePoint;
    }
    public Vector3 getEndMiddlePoint() {
        return this.endMiddlePoint;
    }

    public float getRightWidth() {
        return this.rightWidth;
    }

    public float GetWallWidth() {
        return this.wallWidth;
    }

    public float getWallHeight() {
        return this.wallHeight;
    }

    public Vector3 getWallVector() {
        return this.wallVector;
    }

    public void UpdateData() {
        this.startPoint = meshConnectionVectors[0] + 0.5f*(meshConnectionVectors[1]- meshConnectionVectors[0]);
        this.endPoint = meshConnectionVectors[2] + 0.5f * (meshConnectionVectors[3] - meshConnectionVectors[2]);
        this.startMiddlePoint = this.startPoint + (this.direction * middlePointOffset);
        this.endMiddlePoint = this.endPoint - (this.direction * middlePointOffset);
    }

    public void SetConnectionMeshVectors(Vector3 startl, Vector3 startr, Vector3 endl, Vector3 endr) {
        this.meshConnectionVectors[0] = startl;
        this.meshConnectionVectors[1] = startr;
        this.meshConnectionVectors[2] = endl;
        this.meshConnectionVectors[3] = endr;
    }

    public void SetEndRight(Vector3 endr) {
        this.meshConnectionVectors[3] = endr;
    }

    public void SetStartRight(Vector3 startr) {
        this.meshConnectionVectors[1] = startr;
    }

    public void SetEndLeft(Vector3 endl) {
        this.meshConnectionVectors[2] = endl;
    }

    public void SetStartLeft(Vector3 startl) {
        this.meshConnectionVectors[0] = startl;
    }

    public void SetRightEdge(Vector3 startr, Vector3 endr) {
        this.meshConnectionVectors[1] = startr;
        this.meshConnectionVectors[3] = endr;
    }

    public void SetLeftEdge(Vector3 startl, Vector3 endl) {
        this.meshConnectionVectors[0] = startl;
        this.meshConnectionVectors[2] = endl;
    }

    public override string ToString() { 
        return this.startPoint.ToString() + "\n" +
            this.endPoint.ToString() + "\n" +
            this.direction.ToString() + "\n" +
            this.normal.ToString() + "\n";
    }


}
