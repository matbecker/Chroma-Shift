using UnityEngine;
using System.Collections;

public class QuitOnClick : MonoBehaviour {

    /*This method utilizes compilation specific code. Which obviously, will determine which one to use during compile
     time.*/
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
