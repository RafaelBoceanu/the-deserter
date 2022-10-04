using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public SceneFader sceneFader;

   public void Menu()
    {
        sceneFader.FadeTo(0);
    }
}
