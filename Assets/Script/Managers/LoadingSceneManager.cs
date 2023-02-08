using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static int nextScene;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
    }

    void Start()
    {
        StartCoroutine(LoadScene());
    }

    /// <summary>
    /// 다음 씬으로 넘어가기 전 로딩씬 불러오기
    /// </summary>
    /// <param name="nextSceneIndex">로딩씬 다음으로 로드할 씬</param>
    public static void LoadScene(int nextSceneIndex)
    {
        //MonsterManager.ReturnAllMonsters();
        //GameManager.Inst.MainPlayer.gameObject.SetActive(false);

        nextScene = nextSceneIndex;
        SceneManager.LoadScene("LoadingScreen");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation AsyncOp = SceneManager.LoadSceneAsync(nextScene);
        AsyncOp.allowSceneActivation = false;

        yield return new WaitForSeconds(4.0f);

        AsyncOp.allowSceneActivation = true;
    }
}
