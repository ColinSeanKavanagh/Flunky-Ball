using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ToggleImage : MonoBehaviour
{
    public GameObject Panel;

    public void ToggleImg()
    {
        if (Panel != null) {
        bool isActive = Panel.activeSelf;
        Panel.SetActive(!isActive);
        }
    }
}
