using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorOperations   {

    //line a consisting of point a (aP) and direction (aD)
    //2D intersection with z up and x right
    public static Vector3 Intersection(Vector3 aP, Vector3 aD, Vector3 bP, Vector3 bD) {
        Vector3 inter1 = bP + ((aD.x * (aP.z - bP.z) + aD.z * (bP.x - aP.x)) / (bD.z * aD.x - aD.z * bD.x)) * bD;
        inter1.y = aP.y;
        return inter1;
    }

    public static Vector3 GetCentreOfQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d) {
        return new Vector3((1f / 4f) * (a.x + b.x + c.x + d.x), a.y, (1f / 4f) * (a.z + b.z + c.z + d.z));
    }

}
