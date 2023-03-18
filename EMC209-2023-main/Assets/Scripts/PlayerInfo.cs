using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private string _equipment;
    public float maxHP = 100;

    private bool _isFiring;
    private GameObject _fire;
    public HealthBarBehaviour healthBar;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isFiring);
            stream.SendNext(_currentHealth);
            stream.SendNext(_equipment);
        } else
        {
            this._currentHealth = (float)stream.ReceiveNext();
            this._equipment = (string)stream.ReceiveNext();
            this._isFiring = (bool)stream.ReceiveNext();
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            _fire.SetActive(_isFiring);
        } else
        {
            var input = Input.GetButtonDown("Fire1");
            _isFiring = input;
            _fire.SetActive(input);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var pv = collision.gameObject.GetComponent<PhotonView>();
        
        if (!pv.IsMine)
        {
            var selfpv = GetComponent<PhotonView>();
            selfpv.RPC(nameof(AdjustHealth), pv.Owner, -2);
        }
    }

    [PunRPC]
    private void AdjustHealth(float adjustment)
    {
        _currentHealth -= adjustment;
        healthBar.SetHealth(_currentHealth, maxHP);
    }
}
