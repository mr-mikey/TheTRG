using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveTrigger : MonoBehaviour {
    
    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (LevelGenerator.instance.segment_counter != 3)
            {
                LevelGenerator.instance.AddPiece();
                LevelGenerator.instance.RemoveOldestPiece();
                LevelGenerator.instance.SegmentCounter();
            }
            else
            {
                if (LevelGenerator.instance.endrendered <1)
                LevelGenerator.instance.AddEndPiece();
                LevelGenerator.instance.endrendered += 1;
            }
        }

    }
}
