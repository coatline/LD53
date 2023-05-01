using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : Inputs
{
    [SerializeField] TopDownMover mover;
    [SerializeField] ItemHolder itemHolder;
    [SerializeField] ItemGrabber grabber;
    [SerializeField] ItemUser itemUser;
    [SerializeField] Dasher dasher;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        cam.transform.position = transform.position - new Vector3(0, 0, 10);
        cam.GetComponent<CameraFollow>().SetFollow(transform.GetChild(0));

        if (Game.I.PlayerData == null)
            itemHolder.ChangeItem(new GunStack(DataLibrary.I.Guns.GetRandom(), 1));
    }

    void Update()
    {
        mover.RawInput(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

        if (Input.GetMouseButton(0))
            itemUser.TryUseItem();
        else if (Input.GetMouseButtonUp(0))
            ReleasedUseItemInput();

        if (Input.GetKey(KeyCode.Space))
            dasher.HoldDash();
        else if (Input.GetKeyUp(KeyCode.Space))
            dasher.ReleaseDash();

        if (Input.GetKeyDown(KeyCode.E))
            grabber.TryPickupItem();

        itemHolder.Aim(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    }
}
