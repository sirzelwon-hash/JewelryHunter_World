using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public string sceneName;            // 読み込むシーン名


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // シーンを読み込む
    public void Load()
    {
        GameManager.totalScore = 0; //新しくゲームを始めるにあたってスコアをリセット
        SceneManager.LoadScene(sceneName);
    }
}