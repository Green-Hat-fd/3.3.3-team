using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TiltingPlatform : MonoBehaviour
{
    [SerializeField] HingeJoint joint;
    JointSpring _jointSpring;

    #region Tooltip()
    [Tooltip("La rotazione [-45, 45] per attivare la piattaforma")]
    #endregion
    [Space(10), Range(-45, 45)]
    [SerializeField] float activationRate = 7.5f;
    [SerializeField] bool isMinLimit = true;

    [Space(15)]
    [SerializeField] UnityEvent onActivated,
                                onDeactivated;

    [Header("—— Feedback ——")]
    [SerializeField] AudioSource activated_sfx;
    [SerializeField] AudioSource deactivated_sfx;

    float doOnce_sfx = 0;
    bool activated = false;



    private void Awake()
    {
        //Prende il componente Joint se esso non è stato assegnato
        //(da sé stesso o dai figli)
        if (joint == null)
        {
            if (GetComponent<HingeJoint>())
                joint = GetComponent<HingeJoint>();
            else
                if (GetComponentInChildren<HingeJoint>())
                joint = GetComponentInChildren<HingeJoint>();
        }

        _jointSpring = joint.spring;
    }

    void Update()
    {
        //Controlla se il bottone è stato premuto
        activated = joint.angle <= activationRate;

        //Lo manda sempre verso il limite minimo
        _jointSpring.targetPosition = isMinLimit
                                        ? joint.limits.min
                                        : joint.limits.max;


        if (activated)
        {
            //Attiva ogni oggetto di conseguenza
            onActivated.Invoke();

            SFX_Activate();        //Feedback sonoro
        }
        else
        {
            //Lo disattiva se viene rilasciato
            onDeactivated.Invoke();

            SFX_Deactivate();     //Feedback sonoro
        }

        joint.spring = _jointSpring;
    }


    void SFX_Activate()
    {
        //Controlla se DoOnce è a 0
        if (doOnce_sfx <= 0)
        {
            //Riproduce il suono del pulsante attivato
            activated_sfx.PlayOneShot(activated_sfx.clip);

            doOnce_sfx = 1;       //Porta il DoOnce a 1 -> per il suono quando Rilascia
        }
    }
    void SFX_Deactivate()
    {
        //Controlla se DoOnce è a 0
        if (doOnce_sfx >= 1)
        {
            //Riproduce il suono del pulsante disattivato
            deactivated_sfx.PlayOneShot(deactivated_sfx.clip);

            doOnce_sfx = 0;       //Porta il DoOnce a 0 -> per il suono quando Preme
        }
    }

    public void DeactivatePlatform()
    {
        Vector3 angles = joint.transform.localEulerAngles;

        angles.z = 0;
        joint.transform.localEulerAngles = angles;
    }
}
