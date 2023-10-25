using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotScript : MonoBehaviour
{
    [SerializeField] OptionsSO_Script options_SO;

    [Space(10)]
    [SerializeField] Transform cameraMasterPivot;
    [SerializeField] Transform cameraPivotTilt;
    [SerializeField] Camera playerCam;
    Transform playerCam_Tr;
    Vector3 dirCamPlayer;

    [Space(10)]
    [SerializeField] Transform playerToFollow;
    [SerializeField] float camMovingVel = 6.5f;

    [Space(10)]
    [SerializeField] float rotVel = 6.5f;
    [SerializeField] Vector2 vertRotRange = new Vector2(-15, 52.5f);
    [Space(5)]
    [SerializeField] Vector2 camDistRange = new Vector2(1, 5);
    RaycastHit hitWall;
    bool hasCamHitWall;

    float camDist = 3;
    float xRot = 0f;

    bool centerMouse = true;



    void Awake()
    {
        playerCam_Tr = playerCam.transform;


        //Imposta il mouse al centro dello schermo
        SetCenterMouse(centerMouse);
    }

    void Update()
    {
        #region Rotazione telecamera

        //Prende la rotazione
        InputAction inputRotation = GameManager.inst.inputManager.Giocatore.RotazioneVista;


        //Prende le (X,Y) dell'input
        float mouseX = inputRotation.ReadValue<Vector2>().x * rotVel * Time.deltaTime;
        float mouseY = inputRotation.ReadValue<Vector2>().y * rotVel * Time.deltaTime;


        //Movimento mouse * sensibilita' (dalle impost.)
        mouseX *= options_SO.GetSensitivity();
        mouseY *= options_SO.GetSensitivity();


        #region Gamepad

        if (Gamepad.all.Count > 0)   //Se c'e' almeno un Gamepad
        {
            string inpuName = inputRotation.activeControl.name,
                   gamepadNameCameraMov = Gamepad.current.rightStick.name;

            //Aumenta la sensibilita' se si usa un controller/gamepad
            if (inpuName == gamepadNameCameraMov)
            {
                mouseX *= 10;
                mouseY *= 10;
            }
        }
        #endregion


        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, vertRotRange.x, vertRotRange.y);         //Restrige la rotazione nei limiti

        cameraPivotTilt.localRotation = Quaternion.Euler(xRot, 0, 0f);    //La X come rotazione Y del pivot orizz
        cameraMasterPivot.Rotate(Vector3.up * mouseX);                      //...e la Y come rotazione X del pivot vert

        #endregion



        #region Limita la distanza della cam per non farla entrare nei muri

        //Calcolo della direzione della telecamera
        dirCamPlayer = playerCam_Tr.position - cameraMasterPivot.position;


        //Calcolo se la telecamera ha colpito un muro
        //(non colpisce i Trigger e "~0" significa che collide con tutti i layer)
        hasCamHitWall = Physics.Raycast(playerToFollow.position,
                                     dirCamPlayer,
                                     out hitWall,
                                     camDistRange.y - (playerCam.nearClipPlane + 0.1f),
                                     ~0,
                                     QueryTriggerInteraction.Ignore);


        //Se ha colpito il muro avvicina la telecamera,
        //se no la mette alla massima distanza
        camDist = hasCamHitWall
                    ? hitWall.distance
                    : camDistRange.y;

        //Limita la distanza nel range
        camDist = Mathf.Clamp(camDist, camDistRange.x, camDistRange.y);


        //Calcola la nuova posizione
        Vector3 _camPosDist = playerCam_Tr.localPosition;
        _camPosDist.z = -camDist;
        playerCam_Tr.localPosition = _camPosDist;

        #endregion
    }


    private void FixedUpdate()
    {
        //Muove la telecamera verso il giocatore
        Vector3 playerPos = Vector3.Lerp(cameraMasterPivot.position,
                                         playerToFollow.position,
                                         Time.deltaTime * camMovingVel);
        float dist = Vector3.Distance(cameraMasterPivot.position, playerToFollow.position);

        if (dist <= 0.01f)
            playerPos = playerToFollow.position;

        cameraMasterPivot.localPosition = playerPos;
    }


    public void SetCenterMouse(bool value)
    {
        centerMouse = value;


        //Blocca (o no) + nasconde (o meno)
        //il mouse al centro
        Cursor.visible = !centerMouse;
        Cursor.lockState = centerMouse
                           ? CursorLockMode.Locked
                           : CursorLockMode.None;
    }



    #region EXTRA - Cambiare l'Inspector

    private void OnValidate()
    {
        //Limita il range della rotazione verticale della telecamera
        //(con un min di -90° e un max di 90°)
        vertRotRange.x = Mathf.Clamp(vertRotRange.x, -90, vertRotRange.y);
        vertRotRange.y = Mathf.Clamp(vertRotRange.y, vertRotRange.x, 90);

        //Limita il range della distanza tra la telecamera e il giocatore
        //(sempre positivo)
        camDistRange.x = Mathf.Clamp(camDistRange.x, 0, camDistRange.y);
        camDistRange.y = Mathf.Clamp(camDistRange.y, camDistRange.x, camDistRange.y);
    }

    #endregion


    #region EXTRA - Gizmo

    private void OnDrawGizmos()
    {
        //Disegna una linea grigia che collega la telecamera al giocatore
        Vector3 camPos = playerCam.transform.position;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(cameraMasterPivot.position, camPos);


        //Disegna due sfere vuote grigie;
        //una quanto il nearClipPlane della telecamera,
        //l'altra per quanto può avvicinarsi al giocatore
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(playerCam.transform.position, playerCam.nearClipPlane + 0.1f);
        Gizmos.DrawWireSphere(playerToFollow.position, camDistRange.x);
    }

    private void OnDrawGizmosSelected()
    {
        //Disegna un cubetto verde che indica dove ha colpito il muro
        Gizmos.color = Color.green;
        if (hasCamHitWall)
        {
            Gizmos.DrawLine(playerToFollow.position, hitWall.point);
            Gizmos.DrawCube(hitWall.point, Vector3.one * 0.1f);
        }


        //Disegna due linee blu nei limiti della rotazione della telecamera
        Vector3 camPos = playerCam.transform.position;

        Vector3 camPos2D = new Vector3(camPos.x,
                                       playerToFollow.position.y,
                                       camPos.z),
                dir = (camPos2D - transform.position).normalized;

        Quaternion minRot = Quaternion.AngleAxis(vertRotRange.x, cameraMasterPivot.right),
                   maxRot = Quaternion.AngleAxis(vertRotRange.y, cameraMasterPivot.right);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, minRot * dir * 1.5f);
        Gizmos.DrawRay(transform.position, maxRot * dir * 1.5f);
    }

    #endregion
}
