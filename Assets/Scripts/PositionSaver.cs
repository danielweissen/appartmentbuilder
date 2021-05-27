using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSaver : MonoBehaviour {
    ArrayList positions = new ArrayList();
    int posCount = 0;
    [SerializeField] WallChain wallChain;

    public void addToPositions(Vector3 newPos) {
        positions.Add(newPos);
        posCount++;
        if (posCount > 1) {
            Vector3 pos1 = (Vector3)positions[posCount-2];
            Vector3 pos2 = (Vector3)positions[posCount-1];
            pos1.y = 2f;
            pos2.y = 2f;
            wallChain.AddWall(pos1, pos2);
        }
    }
}
