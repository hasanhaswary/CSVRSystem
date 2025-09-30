using Oculus.Interaction.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CasesManager : MonoBehaviour
{
    public void SelectCaseOne()
    {
        SceneManager.LoadScene("CrimeScene001");
    }

    public void ExitToLogin()
    {
        FindObjectOfType<AuthManager>().LogOut();
    }
}
