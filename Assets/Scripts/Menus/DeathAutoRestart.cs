using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAutoRestart : MonoBehaviour
{
    public float restartTimer = 1;
    private bool hasPlayerDied = false;

    private void OnEnable()
    {
        hasPlayerDied = true;
    }

    void Update()
    {
        if(restartTimer > 0 && hasPlayerDied)
        {
            restartTimer -= 1 * Time.deltaTime;
        }

        if (GameController.instance != null && restartTimer <= 0)
        {
            GameController.instance.RestartScene();
        }
    }
}
