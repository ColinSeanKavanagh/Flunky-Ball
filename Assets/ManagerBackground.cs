using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ManagerBackground : MonoBehaviour
{
    public GameObject[] backgrounds;
    public GameObject[] pictures;
    public int selectedBackground;
    public int selectedPicture;

    public void NextOption()
    {
        backgrounds[selectedBackground].SetActive(false);
        selectedBackground = (selectedBackground + 1) % backgrounds.Length;
        backgrounds[selectedBackground].SetActive(true);

        pictures[selectedPicture].SetActive(false);
        selectedPicture = (selectedPicture + 1) % pictures.Length;
        pictures[selectedPicture].SetActive(true);

    }

    public void BackOption()
    {
        backgrounds[selectedBackground].SetActive(false);
        selectedBackground --;
        if (selectedBackground <0)
        {
            selectedBackground += backgrounds.Length;
        }
        backgrounds[selectedBackground].SetActive(true);

        pictures[selectedPicture].SetActive(false);
        selectedPicture --;
        if (selectedPicture <0)
        {
            selectedPicture += pictures.Length;
        }
        pictures[selectedPicture].SetActive(true);
    }
}
