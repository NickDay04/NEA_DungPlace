using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLoaderManager : MonoBehaviour
{
    public GameObject loadingPanel;
    public GameObject hudPanel;

    // Start is called before the first frame update
    void Start()
    {
        BackendDungeonGen.Room[,] Dungeon = BackendDungeonGen.GenerateDungeon.Run();
        Debug.Log("Dungeon generated");
        loadingPanel = GameObject.Find("Loading");
        hudPanel = GameObject.Find("HUD");
        Debug.Log(loadingPanel.name);
        Debug.Log(hudPanel.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
