// リファレンス
// 数式の公式 http://poltergeist.web.fc2.com/hit_test.html / https://blog.logicky.com/2011/09/06/blog-post_06/
// 数式の読解 https://oshiete.goo.ne.jp/qa/482563.html

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

    private Vector3[] PoligonPoints;

    private Vector3[] TargetPoints;

    private int MAX_VERTEX_COUNT = 5;

    private void Start()
    {
        PoligonPoints = new Vector3[poligon.positionCount];
        TargetPoints = new Vector3[target.positionCount];
    }

    private void Update()
    {
        MoveTarget();
        for (int i = 0; i < MAX_VERTEX_COUNT; i++)
        {
            if (i < poligon.positionCount)
            {
                PoligonPoints[i] = poligon.GetPosition(i);
            }

            if (i < target.positionCount)
            {
                TargetPoints[i] = target.GetPosition(i);
            }
        }

        if (IsHit(TargetPoints, PoligonPoints) || IsHit(PoligonPoints, TargetPoints))
        {
            Debug.Log("内側にあります");
        }
        else
        {
            Debug.Log("外側にあります");
        }
    }

    private void MoveTarget()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0;
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 1cmの四角形を描画
        target.SetPositions(
            new Vector3[]
            {
                new Vector2(mousePoint.x - 1, mousePoint.y + 1),
                new Vector2(mousePoint.x + 1, mousePoint.y + 1),
                new Vector2(mousePoint.x + 1, mousePoint.y - 1),
                new Vector2(mousePoint.x - 1, mousePoint.y - 1),
                new Vector2(mousePoint.x - 1, mousePoint.y + 1),
            }
        );
    }

    private bool IsHit(Vector3[] points, Vector3[] poligon)
    {
        int vertexCount = poligon.Length - 1;

        foreach (var point in points)
        {
            bool outside = false;
            for (int i = 0; i < vertexCount; i++)
            {
                // poligonの各辺のベクトルAを取得
                var vectorA = poligon[i + 1] - poligon[i];
                // poligonの各点と対象の点のベクトルBを取得
                var vectorB = point - poligon[i];

                // ベクトルAとベクトルBから外積を取得してz軸(垂線)の向きがプラスかマイナスかを判定する
                // マイナスであれば多角形の内側に点がある
                outside = Vector3.Cross(vectorA, vectorB).z > 0;
                if (outside) break;
            }
            // 一つでも内側に存在すれば当たり
            if (!outside) return true;
        }
        return false;
    }
}
