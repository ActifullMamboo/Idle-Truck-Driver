using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts.Load;
using UnityEngine;

public class restart : MonoBehaviour
{
    public void Restart()
    {
        LoadingManager.LoadScene(LoadingType.Game);
    }

    public void Mainmenu()
    {
        LoadingManager.LoadScene(LoadingType.Garage);

    }
}
