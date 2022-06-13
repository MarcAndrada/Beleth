using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject;
    [SerializeField]
    private LayerMask wallMask;

    private Camera mainCamera;
    Material[] materials;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (materials != null)
        {
            for (int j = 0; j < materials.Length; j++)
            {
                materials[j].SetVector("_CutoutPos",new Vector2(0,-10));
            }
        }
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; i++)
        {
            try
            {

                Renderer currentRender;
                hitObjects[i].transform.TryGetComponent<Renderer>(out currentRender);


                if (currentRender != null)
                {

                    materials = currentRender.materials;
                    for (int j = 0; j < materials.Length; j++)
                    {
                        materials[j].SetVector("_CutoutPos", cutoutPos);
                        //materials[j].SetFloat("_CutoutSize", 0.1f);
                        //materials[j].SetFloat("_FalloffSize", 0.05f);

                    }
                }
                
            }
            catch (System.Exception)
            {

                throw;
            }
            

        }

    }
}
