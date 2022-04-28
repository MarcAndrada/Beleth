using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiecesController : MonoBehaviour
{
    [SerializeField]
    GameObject BrokePiece;
    [SerializeField]
    GameObject existingBrokenPiece;

    bool can;

    public void Break()
    {
        if (!can)
        {
            if (existingBrokenPiece != null)
            {
                existingBrokenPiece.SetActive(true);
            }
            else
            {
                Instantiate(BrokePiece);
            }            
            can = true;
            Destroy(gameObject);
        }
    }
}
