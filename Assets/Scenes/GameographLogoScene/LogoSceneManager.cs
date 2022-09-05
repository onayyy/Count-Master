using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LogoSceneManager : MonoBehaviour
{
    public float Time;
    IEnumerator Start()
    {
       
        yield return new WaitForSeconds(Time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

}


