using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragGenerator : MonoBehaviour
{
    public GameObject Fragrance;

    //从左上角到右下角的区域
    public Vector2 UpperLeftCorner;
    public Vector2 DownwardRightCorner;

    public LayerMask LayerToAvoid;
    
    private int GeneratePositionsNum = 2;

    public float MaxGenerateInterval = 8.0f;
    public float MinGenerateInterval = 4.0f;
    private float _thisWaveInterval = 0.0f;
    private float _generateTimer = 0.0f;
    
    //用于生成场景的镜子碎片的位置
    
    //一次生成两个吧先
    
    // Start is called before the first frame update
    void Start()
    {
        GetThisWaveInterval();
        Debug.Log(GenerateRamdomPositions()[0]);
    }

    private void GetThisWaveInterval()
    {
        _thisWaveInterval = Random.Range(MinGenerateInterval, MaxGenerateInterval);
    }

    private Vector2[] GenerateRamdomPositions()
    {
        Vector2[] positions = new Vector2[GeneratePositionsNum];
        List<Collider2D> checkResults = new List<Collider2D>();
        for (int i = 0; i < GeneratePositionsNum;)
        {
            positions[i].x = Random.Range(UpperLeftCorner.x, DownwardRightCorner.x);
            positions[i].y = Random.Range(DownwardRightCorner.y, UpperLeftCorner.y);
            var collider2D = Physics2D.OverlapCircle(positions[i], 2.0f, LayerToAvoid);
            if (collider2D != null)
            {
                continue;
            }
            i++;
        }
        //最终输出没问题的俩点
        return positions;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
