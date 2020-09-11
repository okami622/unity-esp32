using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class csvOutput : MonoBehaviour
{
    public SerialHandler serialHandler;
    //float analogdata = 0;

    private float time,beforeTime;
    string filename = DateTime.Now.ToLongTimeString();
    StreamWriter sw;
    Transform myTransform;
    Vector3 mypos;
    Quaternion myquart;
    
    //double recentTime = 0;
    //出力する場所のパスの指定と、ファイル名の指定
    string nowTime = "./Assets/Resources/csvfile/" + System.DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".csv";

    void Start()
    {
        time = 0.0f;
        beforeTime = -0.1f;
        // ファイル書き出し
        // 現在のフォルダにsaveData.csvを出力する(決まった場所に出力したい場合は絶対パスを指定してください)
        // 引数説明：第1引数→ファイル出力先, 第2引数→ファイルに追記(true)or上書き(false), 第3引数→エンコード
        sw = new StreamWriter(@nowTime, false, Encoding.GetEncoding("Shift_JIS"));
        // ヘッダー出力
       
        string[] s1 = {"i" , "Time", "Value1","Value2", "x_pos", "y_pos", "z_pos", "x_rot", "y_rot", "z_rot", "w_rot" };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);

        /*
         *         // ファイル読み込み
        // 引数説明：第1引数→ファイル読込先, 第2引数→エンコード
        StreamReader sr = new StreamReader(@"saveData.csv", Encoding.GetEncoding("Shift_JIS"));
        string line;
        // 行がnullじゃない間(つまり次の行がある場合は)、処理をする
        while ((line = sr.ReadLine()) != null)
        {
            // コンソールに出力
            Debug.Log(line);
        }
        // StreamReaderを閉じる
        sr.Close();*/

        // コルーチンを設定
        StartCoroutine(loop());
    }
    void Update() {
        time += Time.deltaTime;
    }

    private IEnumerator loop()
    {
        // ループ
        while (time != beforeTime)
        {
            // 0.01秒毎にループします
            yield return new WaitForSeconds(0.01f);
            serialHandler.OnDataReceived += OnDataReceived;
        }
    }

    void OnDataReceived(string message)
    {
        beforeTime = time;
        myTransform = this.gameObject.transform;
        mypos = myTransform.position;
        myquart = myTransform.rotation;

        var data = message.Split(new string[] { "\n" }, System.StringSplitOptions.None);
        if (data.Length != 1) return;
        
        Debug.Log("time:" + time);
        myTransform = this.gameObject.transform;
        mypos = myTransform.position;
        myquart = myTransform.rotation;

        float lastTime = 0.0f;
        string[] str = { "" + time + "," + data[0] + "," + mypos.x + "," + mypos.y + "," + mypos.z + "," + myquart.x + "," + myquart.y + "," + myquart.z + "," + myquart.w };
        
        string str2 = string.Join(",", str);
        if(lastTime != time)
        sw.WriteLine(str2);
        lastTime = time;
    }

    private void OnApplicationQuit()
    {
        sw.Close();
    }
}
