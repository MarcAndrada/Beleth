using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiecesController : MonoBehaviour
{
    [SerializeField] GameObject VFX;
    [SerializeField] Transform socket;

    [SerializeField]
    GameObject BrokePiece;
    [SerializeField]
    public GameObject existingBrokenPiece;

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

            Instantiate(VFX, socket.position, socket.rotation);

            Destroy(gameObject);
        }

    }
}
