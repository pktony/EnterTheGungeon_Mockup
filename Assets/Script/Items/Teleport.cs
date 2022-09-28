using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour, ILootable
{
    public void LootAction()
    {
        SceneManager.LoadScene("Test_Boss");
    }
}
