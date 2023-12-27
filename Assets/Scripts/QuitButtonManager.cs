using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButtonManager : MonoBehaviour
{
    private Button quitButton;
    void Start()
    {
        quitButton = GetComponent<Button>();
        quitButton.onClick.AddListener(Click);
    }

    private void Click()
    {
            Application.Quit();
    }
}
