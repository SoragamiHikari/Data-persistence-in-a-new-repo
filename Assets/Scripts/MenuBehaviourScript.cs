using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehaviourScript : MonoBehaviour
{
    public string playerName;
    public GameObject inputField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectedName()
    {
        playerName= inputField.GetComponent<InputField>().text;
        GameManager.Instance.palyerName = playerName;
        Debug.Log(playerName);
    }

    public void startButton()
    {
        SceneManager.LoadScene("main");
    }
}
