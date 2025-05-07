using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ÐÂ½¨½Å±¾ SceneButton.cs
public class SceneButton : MonoBehaviour
{
    public string targetScene;

    public void OnClick()
    {
        FindObjectOfType<MapManager>().LoadScene(targetScene);
    }
}
