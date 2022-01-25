using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ManagerBeers : MonoBehaviour
{
    public GameObject[] beers;
    public int selectedBeer;

    public void NextOption()
    {
        beers[selectedBeer].SetActive(false);
        selectedBeer = (selectedBeer + 1) % beers.Length;
        beers[selectedBeer].SetActive(true);
    }

    public void BackOption()
    {
        beers[selectedBeer].SetActive(false);
        selectedBeer --;
        if (selectedBeer <0)
        {
            selectedBeer += beers.Length;
        }
        beers[selectedBeer].SetActive(true);
    }
}
