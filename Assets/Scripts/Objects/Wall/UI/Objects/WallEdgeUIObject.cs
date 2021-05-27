using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEdgeUIObject {

    private Vector3 referencePoint;
    [SerializeField] private float uiSize = 0.1f;
    [SerializeField] private float uiHeightOffset = 0.001f;

    private Vector3 topLeftVertexPos;
    private Vector3 topRightVertexPos;
    private Vector3 bottomLeftVertexPos;
    private Vector3 bottomRightVertexPos;
    private Vector3[] meshVectors;

    public WallEdgeUIObject(Vector3 referencePoint, float uiSize) {
        this.referencePoint = referencePoint;
        this.uiSize = uiSize;
        SetVertexPos();
    }

    private void SetVertexPos() {
        this.topLeftVertexPos = new Vector3(this.referencePoint.x - uiSize, this.referencePoint.y + uiHeightOffset, this.referencePoint.z + uiSize);
        this.topRightVertexPos = new Vector3(this.referencePoint.x + uiSize, this.referencePoint.y + uiHeightOffset, this.referencePoint.z + uiSize);
        this.bottomLeftVertexPos = new Vector3(this.referencePoint.x - uiSize, this.referencePoint.y + uiHeightOffset, this.referencePoint.z - uiSize);
        this.bottomRightVertexPos = new Vector3(this.referencePoint.x + uiSize, this.referencePoint.y + uiHeightOffset, this.referencePoint.z - uiSize);

        meshVectors = new Vector3[] { bottomLeftVertexPos, bottomRightVertexPos, topLeftVertexPos, topRightVertexPos };
    }

    public Vector3[] GetMeshVectors() {
        return this.meshVectors;
    }
}
