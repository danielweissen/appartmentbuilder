using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour  {
    [SerializeField] PositionSaver posSaver;
    GameObject selectedWall;
    GameObject selectedUI;
    bool dragEventRight = false;
    bool dragEventLeft = false;
    bool tab = false;
    bool rightmouse = false;

    void Update()   {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePosition = ray.origin;

        tab = Input.GetMouseButtonDown(0);
        rightmouse = Input.GetMouseButtonDown(1);

        if (dragEventRight) {
            GameObjectOperations.MoveRightEdge(selectedUI, new Vector3(mousePosition.x, 2f, mousePosition.z));
            if (tab) {
                dragEventRight = false;
            }
            return;
        }

        if (dragEventLeft) {
            GameObjectOperations.MoveLeftEdge(selectedUI, new Vector3(mousePosition.x, 2f, mousePosition.z));
            if (tab) {
                dragEventLeft = false;
            }
            return;
        }

        if (tab) {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                switch (hit.collider.tag) {
                    case "Wall":
                        HandleWallSelection(hit);
                        break;
                    case "UIRight":
                        if (!selectedWall) {
                            return;
                        }
                        selectedUI = hit.collider.gameObject;
                        dragEventRight = true;
                        break;
                    case "UILeft":
                        if (!selectedWall) {
                            return;
                        }
                        selectedUI = hit.collider.gameObject;
                        dragEventLeft = true;
                        break;
                    default:
                        Debug.LogError("nothing");
                        break;
                }
            } else {
                if (selectedWall) {
                    GameObjectOperations.SetUI(selectedWall, false);
                    selectedWall = null;
                } else {
                    posSaver.addToPositions(mousePosition);
                }
            }
        }
        
        if(rightmouse) {
            Debug.Log(mousePosition);
        }

    }

    private void HandleWallSelection(RaycastHit hit) {
        if (selectedWall) {
            GameObjectOperations.SetUI(selectedWall, false);
            selectedWall = null;
        }
        selectedWall = hit.collider.gameObject;
        GameObjectOperations.SetUI(selectedWall, true);
    }
}
