using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public Image[] slotImages; // All 20 slots
    public Sprite[] itemIcons;
    public Color activeColor = Color.yellow;
    public Color inactiveColor = Color.white;

    public GameObject expandedInventoryUI;
    public Button openInventoryButton;
    public Button closeInventoryButton;

    public int expandedColumns = 4; 
    private int currentSlot = 0;
    private bool isExpanded = false;

    void Start()
    {
        UpdateSlotUI();

        if (openInventoryButton != null)
            openInventoryButton.onClick.AddListener(OpenExpandedInventory);

        if (closeInventoryButton != null)
            closeInventoryButton.onClick.AddListener(CloseExpandedInventory);
    }

    void Update()
    {
        if (!isExpanded)
        {
            HandlePrimaryInventoryInput();
        }
        else
        {
            HandleExpandedInventoryInput();
        }
    }

    void HandlePrimaryInventoryInput()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                currentSlot = i;
                UpdateSlotUI();
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            currentSlot = (currentSlot + 1) % 5;
            UpdateSlotUI();
        }
        else if (scroll < 0f)
        {
            currentSlot = (currentSlot - 1 + 5) % 5;
            UpdateSlotUI();
        }
    }

    void HandleExpandedInventoryInput()
    {
        int rowCount = Mathf.CeilToInt(slotImages.Length / (float)expandedColumns);
        int col = currentSlot % expandedColumns;
        int row = currentSlot / expandedColumns;

        if (Input.GetKeyDown(KeyCode.RightArrow) && col < expandedColumns - 1)
        {
            currentSlot = Mathf.Min(currentSlot + 1, slotImages.Length - 1);
            UpdateSlotUI();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && col > 0)
        {
            currentSlot = Mathf.Max(currentSlot - 1, 0);
            UpdateSlotUI();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && row < rowCount - 1)
        {
            int next = currentSlot + expandedColumns;
            if (next < slotImages.Length)
            {
                currentSlot = next;
                UpdateSlotUI();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && row > 0)
        {
            currentSlot = currentSlot - expandedColumns;
            UpdateSlotUI();
        }
    }

    public void OpenExpandedInventory()
    {
        isExpanded = true;
        expandedInventoryUI.SetActive(true);
        Time.timeScale = 0f;

       // Cursor.visible = true;
        

        UpdateSlotUI();
    }

    public void CloseExpandedInventory()
    {
        isExpanded = false;
        expandedInventoryUI.SetActive(false);
        Time.timeScale = 1f;

       

        UpdateSlotUI();
    }

    void UpdateSlotUI()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].color = (i == currentSlot) ? activeColor : inactiveColor;
            if (i < itemIcons.Length && itemIcons[i] != null)
                slotImages[i].sprite = itemIcons[i];
        }
    }

    public int GetSelectedSlotIndex()
    {
        return currentSlot;
    }
}
