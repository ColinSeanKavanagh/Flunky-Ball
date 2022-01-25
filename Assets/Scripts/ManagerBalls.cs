using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ManagerBalls : MonoBehaviour
{
    public GameObject[] balls;
    public int selectedBall;

    public void NextOption()
    {
        balls[selectedBall].SetActive(false);
        selectedBall = (selectedBall + 1) % balls.Length;
        balls[selectedBall].SetActive(true);
    }

    public void BackOption()
    {
        balls[selectedBall].SetActive(false);
        selectedBall --;
        if (selectedBall <0)
        {
            selectedBall += balls.Length;
        }
        balls[selectedBall].SetActive(true);
    }
}
