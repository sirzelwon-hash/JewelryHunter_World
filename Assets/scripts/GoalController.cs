using UnityEngine;

public class GoalController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //BossEntranceが管理しているクリア帳簿をtrueに更新
            BossEntrance.stagesClear[GameManager.currentDoorNumber] = true;
        }
    }
}
