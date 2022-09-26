using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : Item
{
    public override void LootAction()
    {
        SceneManager.LoadScene("Test_Boss");
    }
}
