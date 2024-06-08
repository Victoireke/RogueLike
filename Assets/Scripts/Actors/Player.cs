using Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Items.Consumable;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    public Inventory playerInventory; // Inventory for the player

    private bool inventoryIsOpen = false;
    private bool droppingItem = false;
    private bool usingItem = false;

    private void Awake()
    {
        InitializeControls();
    }

    private void Start()
    {
        if (Camera.main != null)
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        }
        else
        {
            Debug.LogError("Main Camera not found.");
        }

        var actor = GetComponent<Actor>();
        if (actor != null)
        {
            GameManager.Get.Player = actor;
        }
        else
        {
            Debug.LogError("Actor component not found on Player.");
        }
    }

    private void OnEnable()
    {
        if (controls == null)
        {
            InitializeControls();
        }

        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.SetCallbacks(null);
            controls.Disable();
        }
    }

    private void InitializeControls()
    {
        controls = new Controls();
        if (controls == null)
        {
            Debug.LogError("Failed to initialize controls.");
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (inventoryIsOpen)
        {
            if (context.performed)
            {
                Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
                if (direction.y > 0)
                {
                    UIManager.Instance.InventoryUI.SelectPreviousItem();
                }
                else if (direction.y < 0)
                {
                    UIManager.Instance.InventoryUI.SelectNextItem();
                }
            }
        }
        else
        {
            if (context.performed)
            {
                MoveOrHit();
            }
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.performed && inventoryIsOpen)
        {
            UIManager.Instance.InventoryUI.Hide();
            inventoryIsOpen = false;
            droppingItem = false;
            usingItem = false;
        }
    }

    private void MoveOrHit()
    {
        if (controls != null)
        {
            Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
            Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
            var actor = GetComponent<Actor>();
            if (actor != null)
            {
                Action.MoveOrHit(actor, roundedDirection);
                if (Camera.main != null)
                {
                    Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
                }
            }
            else
            {
                Debug.LogError("Actor component not found on Player.");
            }
        }
        else
        {
            Debug.LogError("Controls are not initialized.");
        }
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Get the item at the player's location from the GameManager
            Vector3 playerPosition = transform.position;
            Consumable item = GameManager.Get.GetItemAtLocation(playerPosition);

            if (item == null)
            {
                // No item at this location
                Debug.Log("There is no item to grab at this location.");
            }
            else if (!playerInventory.AddItem(item))
            {
                // Inventory is full
                Debug.Log("Your inventory is full.");
            }
            else
            {
                // Item is added to the inventory
                item.gameObject.SetActive(false); // Hide the item
                GameManager.Get.RemoveItem(item); // Remove the item from the GameManager
                Debug.Log($"You have picked up a {item.Type}.");
            }
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                UIManager.Instance.InventoryUI.Show(playerInventory.Items);
                inventoryIsOpen = true;
                droppingItem = true;
            }
        }
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                UIManager.Instance.InventoryUI.Show(playerInventory.Items);
                inventoryIsOpen = true;
                usingItem = true;
            }
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed && inventoryIsOpen)
        {
            Consumable selectedItem = UIManager.Instance.InventoryUI.GetSelectedItem();
            if (selectedItem != null)
            {
                if (droppingItem)
                {
                    playerInventory.RemoveItem(selectedItem);
                    selectedItem.transform.position = transform.position; // Drop the item at the player's location
                    selectedItem.gameObject.SetActive(true); // Show the item in the world
                    GameManager.Get.AddItem(selectedItem);
                    droppingItem = false;
                }
                else if (usingItem)
                {
                    UseItem(selectedItem);
                    playerInventory.RemoveItem(selectedItem);
                    Destroy(selectedItem.gameObject); // Remove the item from the game world after use
                    usingItem = false;
                }

                UIManager.Instance.InventoryUI.Hide();
                inventoryIsOpen = false;
            }
        }
    }

    private void UseItem(Consumable item)
    {
        var actor = GetComponent<Actor>();
        switch (item.Type)
        {
            case ItemType.HealthPotion:
                actor.Heal(item.Value);
                break;
            case ItemType.Fireball:
                List<Actor> nearbyEnemies = GameManager.Get.GetNearbyEnemies(transform.position);
                foreach (var enemy in nearbyEnemies)
                {
                    enemy.DoDamage(item.Value, actor); // Pass the player actor as the attacker
                }
                UIManager.Instance.ShowMessage("You used a Fireball and damaged nearby enemies.", Color.red);
                break;
            case ItemType.ScrollOfConfusion:
                nearbyEnemies = GameManager.Get.GetNearbyEnemies(transform.position);
                foreach (var enemy in nearbyEnemies)
                {
                    var enemyComponent = enemy.GetComponent<Enemy>();
                    if (enemyComponent != null)
                    {
                        enemyComponent.Confuse();
                    }
                }
                UIManager.Instance.ShowMessage("You used a Scroll of Confusion and confused nearby enemies.", Color.blue);
                break;
        }
    }
}
