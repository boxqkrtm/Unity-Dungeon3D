using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class InputManager : MonoBehaviour
{
    static private InputManager instance = null;
    public static InputManager Instance
    {
        get
        {
            return instance;
        }
    }

    Vector2 move = Vector2.zero;
    private void Awake()
    {
        instance = this;
    }
    static private PlayerControl ps = null;
    static public PlayerControl Ps { get => ps; }
    private void Start()
    {
        if (ps != null)
        {
            ps.Play.Disable();
            ps = null;
        }
        if (ps == null)
        {
            ps = new PlayerControl();
            ps.Play.Inventory.performed += ctx => InventoryManager.Instance.ToggleInventory();
            ps.Play.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            ps.Play.Move.canceled += ctx => move = Vector2.zero;
            ps.Play.DashDown.performed += ctx =>
            GameManager.Instance.PlayerObj.GetComponent<Player>().DashBoost();
            ps.Play.DashHold.performed += ctx =>
            {
                GameManager.Instance.PlayerObj.GetComponent<Player>().Dash();
            };
            ps.Play.DashHold.canceled += ctx =>
            {
                GameManager.Instance.PlayerObj.GetComponent<Player>().DashOff();
            };
            ps.Play.Attack.performed += ctx =>
            {
                GameManager.Instance.PlayerObj.GetComponent<Player>().Attack();
            };
            ps.Play.AttackOff.performed += ctx =>
            {
                GameManager.Instance.PlayerObj.GetComponent<Player>().AttackOff();
            };
            ps.Play.Skill1.performed += ctx => GameManager.Instance.PlayerObj.GetComponent<Player>().Skill1();

            ps.Play.SkillWindow.performed += ctx => GameManager.Instance.OpenSkillWindow();
        }
        ps.Play.Enable();
    }

    public void InputStop()
    {
        move = Vector2.zero;
        GameManager.Instance.PlayerObj.GetComponent<Player>().DashOff();
        GameManager.Instance.PlayerObj.GetComponent<Player>().AttackOff();
        ps.Play.Disable();
    }

    public void InputStart()
    {
        ps.Play.Enable();
    }

    public Vector2 GetAxisMove()
    {
        return move;
    }
}
