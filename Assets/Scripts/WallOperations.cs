using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOperations {

    public static void ConnectWalls(WallData oldWall, WallData newWall) {

        //this vector is used to see if the new wall endpoit is to the left or right of the old wall direction
        Vector3 vecToNewWall = (newWall.getEndPoint() - oldWall.getStartPoint());
        Vector3 connectionToStableVertexVertex;
        Vector3 otherVertex;
        Vector3 other;

        Vector3 startLeft = oldWall.getConnectionMeshVectors()[0];
        Vector3 startRight = oldWall.getConnectionMeshVectors()[1];
        Vector3 endLeft = oldWall.getConnectionMeshVectors()[2];
        Vector3 endRight = oldWall.getConnectionMeshVectors()[3];
        //to the left
        if (Vector3.Cross(oldWall.getDirection(), vecToNewWall).y < 0) {
            connectionToStableVertexVertex = VectorOperations.Intersection(endRight, newWall.getDirection(), newWall.getEndPoint(), newWall.getNormal());
            otherVertex = newWall.getEndPoint() - (connectionToStableVertexVertex - newWall.getEndPoint());
            other = VectorOperations.Intersection(startLeft, oldWall.getDirection(), otherVertex, newWall.getDirection());
            newWall.SetConnectionMeshVectors(other, endRight, otherVertex, connectionToStableVertexVertex);
            oldWall.SetConnectionMeshVectors(startLeft, startRight, other, endRight);
        //to the right
        } else {
            connectionToStableVertexVertex = VectorOperations.Intersection(endLeft, newWall.getDirection(), newWall.getEndPoint(), newWall.getNormal());
            otherVertex = newWall.getEndPoint() - (connectionToStableVertexVertex - newWall.getEndPoint());
            other = VectorOperations.Intersection(startRight, oldWall.getDirection(), otherVertex, newWall.getDirection());
            newWall.SetConnectionMeshVectors(endLeft, other, connectionToStableVertexVertex, otherVertex);
            oldWall.SetConnectionMeshVectors(startLeft, startRight, endLeft, other);
        }


    }
    //unstableConnectionVertex is moved according to the new wall
    //stableConnectionVertex remains as is
    //these two vertecies are used by both walls



    public static Vector3[] CalculateWallMesh(Vector3 startPoint, Vector3 endPoint, Vector3 direction, float upperLowerOffset, Vector3 normal, float leftWidth, float rightWidth, float leftRightOffset, float wallHeight) {
        Vector3 startLeftVertexPos = new Vector3(startPoint.x - direction.x * upperLowerOffset + (normal.x * leftWidth + (normal.x * leftRightOffset)), wallHeight, startPoint.z - direction.z * upperLowerOffset + (normal.z * leftWidth + (normal.z * leftRightOffset)));
        Vector3 startRightVertexPos = new Vector3(startPoint.x - direction.x * upperLowerOffset - (normal.x * rightWidth + (normal.x * leftRightOffset)), wallHeight, startPoint.z - direction.z * upperLowerOffset - (normal.z * rightWidth + (normal.z * leftRightOffset)));
        Vector3 endLeftVertexPos = new Vector3(endPoint.x + direction.x * upperLowerOffset + (normal.x * leftWidth + (normal.x * leftRightOffset)), wallHeight, endPoint.z + direction.z * upperLowerOffset + (normal.z * leftWidth + (normal.z * leftRightOffset)));
        Vector3 endRightVertexPos = new Vector3(endPoint.x + direction.x * upperLowerOffset - (normal.x * rightWidth + (normal.x * leftRightOffset)), wallHeight, endPoint.z + direction.z * upperLowerOffset - (normal.z * rightWidth + (normal.z * leftRightOffset)));

        return new Vector3[] { startLeftVertexPos, startRightVertexPos, endLeftVertexPos, endRightVertexPos };
    }


    //TODO looks fucking expensive!!
    public static Vector3[] CalculateWallSelectionMesh(Vector3 startleft, Vector3 startright, Vector3 endleft, Vector3 endright, float offset) {
        Vector3 a = Vector3.Normalize(startright - startleft); // ->
        Vector3 b = Vector3.Normalize(endright - startright); // ^
        Vector3 c = Vector3.Normalize(endleft - endright); // <-
        Vector3 d = Vector3.Normalize(startleft - endleft); // v

        Vector3 an = new Vector3(-a.z, a.y, a.x); // ->
        Vector3 bn = new Vector3(-b.z, b.y, b.x); // ^
        Vector3 cn = new Vector3(-c.z, c.y, c.x); // <-
        Vector3 dn = new Vector3(-d.z, d.y, d.x); // v

        Vector3 aof = startleft - an * offset; // ->
        Vector3 bof = startright - bn * offset; // ^
        Vector3 cof = endright - cn * offset; // <-
        Vector3 dof = endleft - dn * offset; // v

        aof.y = startleft.y;
        bof.y = startleft.y;
        cof.y = startleft.y;
        dof.y = startleft.y;

        Vector3 w = VectorOperations.Intersection(aof, a, bof, b);
        Vector3 x = VectorOperations.Intersection(bof, b, cof, c);
        Vector3 y = VectorOperations.Intersection(cof, c, dof, d);
        Vector3 z = VectorOperations.Intersection(dof, d, aof, a);

        return new Vector3[] {z,w,y,x };
    }
}
