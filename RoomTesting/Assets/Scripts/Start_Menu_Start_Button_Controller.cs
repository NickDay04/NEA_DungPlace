using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Menu_Start_Button_Controller : MonoBehaviour
{
    public void ChangeToGame()
    {
        SceneManager.LoadScene("Main_Game");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
