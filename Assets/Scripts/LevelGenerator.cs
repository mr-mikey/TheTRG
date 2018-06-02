using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    public Transform levelStartPoint;
    public List<LevelPieceBasic> levelPrefabs = new List<LevelPieceBasic>();
    public List<LevelPieceBasic> pieces = new List<LevelPieceBasic>();
    public LevelPieceBasic startPlatformPrefab;    public LevelPieceBasic endPlatformPrefab;    public int segment_counter;
    public int endrendered = 0;
    private float lastprefab;
    public List<LevelPieceWithProbability> levelPrefabsWithProbability = new List<LevelPieceWithProbability>();
    // Use this for initialization
    void Start()
    {
        instance = this;
        ShowPiece((LevelPieceBasic)Instantiate(startPlatformPrefab));
        AddPiece();
        AddPiece();
        AddPiece();
        AddPiece();

    }
    [System.Serializable]
    public class LevelPieceWithProbability
    {
        public LevelPieceBasic levelpiece;
        public float probability = 0.0f;
    }    public void ShowPiece(LevelPieceBasic piece)
    {
        piece.transform.SetParent(this.transform, false);
        if (pieces.Count < 1)
            piece.transform.position = new Vector2(levelStartPoint.position.x - piece.startPoint.localPosition.x,levelStartPoint.position.y - piece.exitPoint.localPosition.y);
        else
            piece.transform.position = new Vector2(pieces[pieces.Count - 1].exitPoint.position.x - pieces[pieces.Count - 1].startPoint.localPosition.x,pieces[pieces.Count - 1].exitPoint.position.y - pieces[pieces.Count - 1].exitPoint.localPosition.y);
        pieces.Add(piece);
    }
    public void AddPiece()
    {
        float random = Random.Range(0.0f, 1.0f);
        int randomIndex = levelPrefabsWithProbability.Count - 1;
        for (int i=0; i < levelPrefabsWithProbability.Count -1; i++)
        {
            if (random < levelPrefabsWithProbability[ i ].probability)
            {
                if (lastprefab != i)
                {
                    randomIndex = i;
                    break;
                }
                else
                {
                    continue;
                    
                }
                
            }
        }
        LevelPieceBasic piece = (LevelPieceBasic)Instantiate(levelPrefabsWithProbability[randomIndex].levelpiece);
        lastprefab = randomIndex;
        ShowPiece(piece);
    }
    public void AddEndPiece()
    {
        
        ShowPiece((LevelPieceBasic)Instantiate(endPlatformPrefab));
    }
    public void RemoveOldestPiece()
    {
        if (pieces.Count > 1)
        {
            LevelPieceBasic oldestPiece = pieces[0];
            pieces.RemoveAt(0);
            Destroy(oldestPiece.gameObject);
        }
    }
    public void SegmentCounter()
    {
        segment_counter += 1;
    }
}