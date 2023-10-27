using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] SpriteRenderer shadow;
    Transform shadowTr;

    [Space(10)]
    [SerializeField] bool isInfinite;
    [SerializeField] float maxDist;
    bool canShowShadow;
    RaycastHit hitShadow;

    [Header("")]
    [SerializeField] Vector2 scaleRange = new Vector2(0.5f, 1f);


    
    void Awake()
    {
        shadowTr = shadow.transform;
    }

    void Update()
    {
        shadowTr.gameObject.SetActive(canShowShadow);    //Mostra l'ombra solo se ha colpito qualcosa

        shadowTr.rotation = Quaternion.LookRotation(Vector3.up);    //Ruota sempre l'ombra verso l'alto
    


        //Cambia la dimensione dell'ombra rispetto alla distanza
        float shadowScale = 1 - (hitShadow.distance / maxDist);

        shadowScale = Mathf.Clamp(shadowScale, scaleRange.x, scaleRange.y);    //Limita la dimensione

        shadowTr.localScale = Vector3.one * shadowScale;
    }

    private void FixedUpdate()
    {
        float castdist = isInfinite
                           ? Mathf.Infinity
                           : maxDist;

        //Calcolo se colpisce a terra l'ombra
        //(non colpisce i Trigger e "~0" significa che collide con tutti i layer)
        canShowShadow = Physics.Raycast(transform.position,
                                        Vector3.down,
                                        out hitShadow,
                                        castdist,
                                        ~0,
                                        QueryTriggerInteraction.Ignore);


    
        //Posiziona l'ombra dove si colpisce il raycast
        if (canShowShadow)
        {
            shadowTr.position = hitShadow.point + Vector3.up * 0.01f;
        }
    }



    #region EXTRA - Cambiare l'Inspector

    private void OnValidate()
    {
        //Limita il range dello scale dell'ombra
        //(tra 0 e 1)
        scaleRange.x = Mathf.Clamp(scaleRange.x, 0, scaleRange.y);
        scaleRange.y = Mathf.Clamp(scaleRange.y, scaleRange.x, 1);
    }

    #endregion


    #region EXTRA - Gizmo

    private void OnDrawGizmosSelected()
    {
        //Disegna un cubetto verde che indica dove ha colpito il muro
        Gizmos.color = Color.green;
        if (canShowShadow)
        {
            Gizmos.DrawLine(startPoint.position, hitShadow.point);
            Gizmos.DrawCube(hitShadow.point, Vector3.one * 0.1f);
        }
    }

    #endregion
}
