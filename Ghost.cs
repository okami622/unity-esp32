using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Ghost : MonoBehaviour
{

    List<string[]> csvDatas = new List<string[]>();
    public string fname = "YYYY_MM_DD_hh_mm_ss";
    Transform myTransform;
    Vector3 mypos;
    Quaternion myquart;

    private float time;
    int i = 1;
    float pbTime = 0.0f;
    // Use this for initialization
    void Awake()
    {
        myTransform = this.gameObject.transform;
        mypos = myTransform.position;
        myquart = myTransform.rotation;
        string path = "csvfile/" + fname;
        var csv = Resources.Load(path) as TextAsset;
        var reader = new StringReader(csv.text);
        while (reader.Peek() > -1)
        {
            // ','ごとに区切って配列へ格納
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
        }//ファイルのデータを配列に格納
        time = 0.0f - Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Transform t = this.gameObject.GetComponent<Transform>();
        time += Time.deltaTime;
        pbTime = float.Parse(csvDatas[i][0]);
        if (Mathf.Abs(pbTime - time) < 0.01f)
        {
            t.position = new Vector3(float.Parse(csvDatas[i][3]), float.Parse(csvDatas[i][4]), float.Parse(csvDatas[i][5]));
            t.rotation = new Quaternion(float.Parse(csvDatas[i][6]), float.Parse(csvDatas[i][7]), float.Parse(csvDatas[i][8]), float.Parse(csvDatas[i][9]));
            i += 5; //再生時の負荷のほうが大きいため、１ずつ増やすと遅くなるため、少し飛ばしながら再生
        }
        else if (pbTime > time)
        {
                        //早い場合何もしない
        }
        else
        {
            i += 25;  //再生が遅れている場合、飛ばす
        }
    }
}