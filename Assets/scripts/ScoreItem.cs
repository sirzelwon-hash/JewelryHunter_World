using UnityEngine;

public class ScoreItem : MonoBehaviour
{

    public ItemData itemdata;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>
            ().sprite =
            itemdata.itemSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
