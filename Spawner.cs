using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //配列の作成（生成するブロック全てを格納する）
    public Block[] Blocks;

    public Block[] NewBlocks;

    //関数の作成
    //ランダムなブロックを一つ選ぶ関数
    Block GetRandomBlock()
    {
        int i = Random.Range(0, NewBlocks.Length);

        if (NewBlocks[i])
        {
            return NewBlocks[i];
        }
        else
        { 
            return null;
        }
    }


    //選ばれたブロックを生成する関数
    public Block SpawnBlock()
    {
        Block block = Instantiate(GetRandomBlock(), transform.position, Quaternion.identity);

        if (block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }

 

}
