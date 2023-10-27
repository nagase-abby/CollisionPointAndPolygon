// リファレンス
// 数式の公式 http://poltergeist.web.fc2.com/hit_test.html
// 数式の読解 https://oshiete.goo.ne.jp/qa/482563.html

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 点と多角形の当たり判定
/// </summary>
public class CollisionPointAndPolygon : MonoBehaviour
{
    /// <summary>
    /// 対象の多角形
    /// </summary>
    [SerializeField]
    private LineRenderer poligon = null;

    [SerializeField]
    private LineRenderer target = null;

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0;
        // 終点
        Vector3 mouseOrigin = Camera.main.ScreenToWorldPoint(mousePosition);

        int vertexCount = poligon.positionCount - 1;

        // poligonの各辺のベクトルAを取得
        var poligonVectorList = new List<Vector3>();
        for (int i = 0; i < vertexCount; i++)
        {
            var vector = poligon.GetPosition(i + 1) - poligon.GetPosition(i);
            poligonVectorList.Add(vector);
        }

        // poligonの各点と対象の点のベクトルBを取得
        var pointVectorList = new List<Vector2>();
        for (int i = 0; i < vertexCount; i++)
        {
            var vector = mouseOrigin - poligon.GetPosition(i);
            pointVectorList.Add(vector);
        }

        // ベクトルAとベクトルBから外積を取得してz軸(垂線)の向きがプラスかマイナスかを判定する
        // 全部同じ向きなら多角形の内部に点がある
        for (int i = 0; i < vertexCount; i++)
        {
            var z = Vector3.Cross(poligonVectorList[i], pointVectorList[i]);
            Debug.Log($"cross{i} : {z}");
        }
    }
}
