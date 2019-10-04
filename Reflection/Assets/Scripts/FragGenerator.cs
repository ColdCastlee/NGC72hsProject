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

    private int GeneratedFragNum = 0;
    private int CollectedFragNum = 0;
    
    //用于生成场景的镜子碎片的位置
    
    //一次生成两个吧先
    
    // Start is called before the first frame update
    void Start()
    {
        GetThisWaveInterval();
    }

    private void GetThisWaveInterval()
    {
        _thisWaveInterval = Random.Range(MinGenerateInterval, MaxGenerateInterval);
    }

    private Vector2[] GenerateRamdomPositions()
    {
        Vector2[] positions = new Vector2[GeneratePositionsNum];
        List<Collider2D> checkResults = new List<Collider2D>();
        int times = 0;
        for (int i = 0; i < GeneratePositionsNum;)
        {
            times++;
            if (times > 100)
            {
                break;
            }
            positions[i].x = Random.Range(UpperLeftCorner.x, DownwardRightCorner.x);
            positions[i].y = Random.Range(DownwardRightCorner.y, UpperLeftCorner.y);
            var collider2D = Physics2D.OverlapCircle(positions[i], 0.2f, LayerToAvoid);
            //Debug.Log(positions[i]);
            if (collider2D != null)
            {
                //Debug.Log(collider2D.name);
                continue;
            }
            i++;
        }
        //最终输出没问题的俩点
        return positions;
    }

    //Debug
    private void OnDrawGizmos()
    {
        Vector3 upperLeft = new Vector3(UpperLeftCorner.x, UpperLeftCorner.y, 0);
        Vector3 downwardRight = new Vector3(DownwardRightCorner.x, DownwardRightCorner.y, 0);
        Debug.DrawLine(upperLeft + Vector3.down, upperLeft + Vector3.up, Color.red);
        Debug.DrawLine(upperLeft + Vector3.left, upperLeft + Vector3.right, Color.red);
        Debug.DrawLine(downwardRight + Vector3.down, downwardRight + Vector3.up, Color.green);
        Debug.DrawLine(downwardRight + Vector3.left, downwardRight + Vector3.right, Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        CheckGenerationTimer();
        //CheckIf We Should Generate Now
    }

    private void CheckGenerationTimer()
    {
        if (_generateTimer < _thisWaveInterval)
        {
            this._generateTimer += Time.deltaTime;
        }
        else
        {
            _generateTimer = 0.0f;
            GetThisWaveInterval();
            GenerateFrags(GenerateRamdomPositions());
        }
    }

    private void GenerateFrags(Vector2[] positions)
    {
        foreach (var pos in positions)
        {
            //TODO::PARENTS
            var frag = Instantiate(Fragrance, pos, Quaternion.identity);
            var fragScript = frag.GetComponent<BasicFrag>();
            fragScript.Init();
        }        
    }
}
