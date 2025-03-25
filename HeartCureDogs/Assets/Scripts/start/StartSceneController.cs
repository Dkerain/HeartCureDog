using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    public string petShopSceneName = "PetShopScene";

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(petShopSceneName);
    }
}
