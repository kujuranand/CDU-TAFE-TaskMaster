using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    [Header("References")]
    public GameObject mainMenu;

    private void Start() {
        UpdateCursorState();
    }

    private void Update() {
        UpdateCursorState();
    }

    private void UpdateCursorState() {
        if (mainMenu != null && mainMenu.activeSelf) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}