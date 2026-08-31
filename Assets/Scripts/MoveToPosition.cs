using UnityEngine;
using System.Collections;
using Const;

public class MoveToPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public IEnumerator MoveToPositionCoroutine(char direction_, float duration_)
    {
        // 現在の場所をスタート地点にする
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos;

        // 方向を判定して「移動先（targetPos）」を計算する
        if (direction_ == (char)symbolDirections.left) targetPos.x += (float)directions.left * Const.CONST.MOVEDISTANCE;
        if (direction_ == (char)symbolDirections.right) targetPos.x += (float)directions.right * Const.CONST.MOVEDISTANCE;
        if (direction_ == (char)symbolDirections.up) targetPos.y -= (float)directions.up * Const.CONST.MOVEDISTANCE;
        if (direction_ == (char)symbolDirections.down) targetPos.y -= (float)directions.down * Const.CONST.MOVEDISTANCE;

        float elapsed = 0f;
        while (elapsed < duration_)
        {
            // Vector3.Lerp を使い、startPos.z (元のZ) を維持したまま移動
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration_);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos; // 最後にズレを補正
        //transform.position = new Vector3(transform.position.x, transform.position.y);
    }
}
