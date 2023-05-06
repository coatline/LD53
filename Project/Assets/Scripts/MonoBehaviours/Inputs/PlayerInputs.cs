using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInputs : Inputs
{
    [SerializeField] TopDownMover mover;
    [SerializeField] ItemHolder itemHolder;
    [SerializeField] ItemGrabber grabber;
    [SerializeField] ItemUser itemUser;
    [SerializeField] Dasher dasher;
    GameObject pressEText;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        cam.transform.position = transform.position - new Vector3(0, 0, 10);
        cam.GetComponent<CameraFollow>().SetFollow(transform.GetChild(0));

        Canvas[] canvasas = FindObjectsOfType<Canvas>();
        Canvas c = null;

        foreach (Canvas canvas in canvasas)
        {
            if (canvas.name == "WorldSpaceCanvas")
            {
                c = canvas;
                break;
            }
        }

        pressEText = c.transform.Find("Press E Text").gameObject;
        grabber.OverItem += OverItem;
        grabber.LeftItem += LeftItem;

        if (Game.I.PlayerData == null)
            itemHolder.ChangeItem(new GunStack(DataLibrary.I.Guns.GetRandom(), 1));
    }

    void OverItem(LooseItem i)
    {
        pressEText.transform.position = transform.position + new Vector3(0, 2, 0);
        pressEText.SetActive(true);
    }
    void LeftItem(LooseItem i) => pressEText.SetActive(false);

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
