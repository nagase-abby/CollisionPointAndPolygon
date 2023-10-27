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

    private void Update()
    {
        MoveTarget();

        if (IsHit(new Vector3[] { target.GetPosition(0), target.GetPosition(1), target.GetPosition(2), target.GetPosition(3) }))
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

    private bool IsHit(Vector3[] points)
    {
        int vertexCount = poligon.positionCount - 1;

        foreach (var point in points)
        {
            bool inside = true;
            for (int i = 0; i < vertexCount; i++)
            {
                // poligonの各辺のベクトルAを取得
                var vectorA = poligon.GetPosition(i + 1) - poligon.GetPosition(i);
                // poligonの各点と対象の点のベクトルBを取得
                var vectorB = point - poligon.GetPosition(i);

                // ベクトルAとベクトルBから外積を取得してz軸(垂線)の向きがプラスかマイナスかを判定する
                // マイナスであれば多角形の内側に点がある
                if (Vector3.Cross(vectorA, vectorB).z > 0)
                {
                    inside = false;
                    break;
                }
            }
            if (inside) return true;
        }
        return false;
    }
}
