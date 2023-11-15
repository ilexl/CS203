#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform BoardParentTransform;
    [Space(10)]
    [SerializeField] Vector3 startOffset;
    [SerializeField] GameObject BoardGridPiecePrefab;
    public int BoardSize;
    [SerializeField] float spacing;
    
    // creates a reusable and flexable board of any dimesions
    public void CreateBoard()
    {
        float step = 1f;
        int untilRight = 1;
        Vector3 lastPosition = startOffset;
        Vector3 lasteulerAngles = Vector3.zero;
        GameObject pivot = null;
        GameObject lastPiece = null; // bottom right

        // spawn board in spiral
        for (int i = 0; i < (BoardSize * BoardSize * 4); i++)
        {
            GameObject piece = Instantiate(BoardGridPiecePrefab, BoardParentTransform);
            piece.transform.localPosition = lastPosition;
            piece.transform.eulerAngles = lasteulerAngles;
            if (untilRight == 0 && i != 0)
            {
                piece.transform.eulerAngles += new Vector3(0, 0, 90);

                untilRight += (int)step;
                step += 0.5f;
            }
            untilRight--;

            piece.transform.Translate(new Vector3(0, spacing, 0));
            lastPosition = piece.transform.localPosition;
            lasteulerAngles = piece.transform.eulerAngles;

            lastPiece = piece;
        }

        // number pieces from spiral and find top left
        int name = 0;
        foreach (Transform b in BoardParentTransform)
        {
            if (name != 0)
            {
                float sqrt = Mathf.Sqrt(name);
                while (sqrt > 2) { sqrt -= 2; }
                float t = (sqrt % 2) / 2;
                if (t == 0.5f)
                {
                    pivot = b.gameObject;
                }
            }
            b.name = name++.ToString();
        }

        Vector3 offset = ((pivot.transform.position - lastPiece.transform.position) / spacing);
        offset.x *= -1;
        Debug.Log(offset);

        // give each board piece their real position
        // i.e top left is 0,0 and down/right is 1,1
        foreach (Transform b in BoardParentTransform)
        {
            Vector3 realPos = new(((b.position - lastPiece.transform.position) / spacing).x + offset.x,
                (offset.y - ((b.position - lastPiece.transform.position) / spacing).y),
                0);
            b.name += " - " + realPos.ToString(); // sendName them just in case
            b.GetComponent<BoardPart>().realPos = realPos;
        }
    }
}
#if UNITY_EDITOR

[CustomEditor(typeof(Board))]
public class EDITOR_Board : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Board"))
        {
            ((Board)target).CreateBoard();
        }
    }
}
#endif