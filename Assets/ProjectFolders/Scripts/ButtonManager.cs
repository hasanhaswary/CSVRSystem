using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject menu;

    public void Menu()
    {
        if (menu.activeInHierarchy == true)
        {
            menu.SetActive(false);
        }
        else if (menu.activeInHierarchy == false)
        {
            menu.SetActive(true);
        }
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("CasesView");
    }

    public void LogOut()
    {
        FindObjectOfType<AuthManager>().LogOut();
    }

    public void Restart()
    {
        SceneManager.LoadScene("CrimeScene001");
    }
}
