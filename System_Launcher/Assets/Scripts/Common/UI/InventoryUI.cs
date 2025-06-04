// GPM UI ���̺귯�� ���
using Gpm.Ui;
// �ڷ�ƾ ���� ���̺귯�� ���
using System.Collections;
// �÷��� ���� ���̺귯�� ���
using System.Collections.Generic;
// TextMeshPro �ؽ�Ʈ ������Ʈ ���
using TMPro;
// Unity �⺻ ���̺귯�� ���
using UnityEngine;

// �κ��丮 ���� Ÿ�� ������ ����
public enum InventorySortType
{
    // ������ ��� ���� ����
    ItemGrade,
    // ������ Ÿ�� ���� ����
    ItemType,
}

// �κ��丮 UI Ŭ���� - BaseUI�� ��ӹ���
public class InventoryUI : BaseUI
{
    // �κ��丮 ��ũ�� ����Ʈ ������Ʈ ����
    public InfiniteScroll InventoryScrollList;
    // ���� ��ư�� �ؽ�Ʈ ������Ʈ ����
    public TextMeshProUGUI SortBtnTxt;

    // ���� �κ��丮 ���� Ÿ�� (�⺻��: ������ ���)
    private InventorySortType m_InventorySortType = InventorySortType.ItemGrade;

    // UI ������ �����ϴ� �޼��� �������̵�
    public override void SetInfo(BaseUIData uiData)
    {
        // �θ� Ŭ������ SetInfo �޼��� ȣ��
        base.SetInfo(uiData);

        // �κ��丮 ������ ����
        SetInventory();
        // �κ��丮 ���� ����
        SortInventory();
    }

    // �κ��丮 �����͸� �����ϴ� �޼���
    private void SetInventory()
    {
        // �κ��丮 ��ũ�� ����Ʈ �ʱ�ȭ
        InventoryScrollList.Clear();

        // ����� �κ��丮 ������ ��������
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        // ����� �κ��丮 �����Ͱ� �����ϴ� ���
        if (userInventoryData != null)
        {
            // �κ��丮 ������ ����Ʈ�� ��ȸ
            foreach (var itemData in userInventoryData.InventoryItemDataList)
            {
                // ���ο� �κ��丮 ������ ���� ������ ����
                var itemSlotData = new InventoryItemSlotData();
                // ������ �ø��� ��ȣ ����
                itemSlotData.SerialNumber = itemData.SerialNumber;
                // ������ ID ����
                itemSlotData.ItemId = itemData.ItemId;
                // ��ũ�� ����Ʈ�� ������ ������ ����
                InventoryScrollList.InsertData(itemSlotData);
            }
        }
    }

    // �κ��丮�� �����ϴ� �޼���
    private void SortInventory()
    {
        // ���� ���� Ÿ�Կ� ���� �б� ó��
        switch (m_InventorySortType)
        {
            // ������ ��� ���� ������ ���
            case InventorySortType.ItemGrade:
                // ���� ��ư �ؽ�Ʈ�� "GRADE"�� ����
                SortBtnTxt.text = "GRADE";

                // ������ ����Ʈ�� ������ ��� �������� ����
                InventoryScrollList.SortDataList((a, b) =>
                {
                    // ������ A�� InventoryItemSlotData�� ĳ����
                    var itemA = a.data as InventoryItemSlotData;
                    // ������ B�� InventoryItemSlotData�� ĳ����
                    var itemB = b.data as InventoryItemSlotData;

                    // ������ ������� ���� (���� ����� ������)
                    int compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);

                    // ���� ������ ����� ���, ������ Ÿ������ ����
                    if (compareResult == 0)
                    {
                        // ������ A�� ID�� ���ڿ��� ��ȯ
                        var itemAIdStr = itemA.ItemId.ToString();
                        // ������ A�� �񱳰� ���� (11001 -> 1001)
                        var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2, 3);

                        // ������ B�� ID�� ���ڿ��� ��ȯ
                        var itemBIdStr = itemB.ItemId.ToString();
                        // ������ B�� �񱳰� ���� (11001 -> 1001)
                        var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);

                        // ������ Ÿ������ ��
                        compareResult = itemAComp.CompareTo(itemBComp);
                    }

                    // �� ��� ��ȯ
                    return compareResult;
                });
                break;
            // ������ Ÿ�� ���� ������ ���
            case InventorySortType.ItemType:
                // ���� ��ư �ؽ�Ʈ�� "TYPE"���� ����
                SortBtnTxt.text = "TYPE";
                // ������ ����Ʈ�� ������ Ÿ�� �������� ����
                InventoryScrollList.SortDataList((a, b) =>
                {
                    // ������ A�� InventoryItemSlotData�� ĳ����
                    var itemA = a.data as InventoryItemSlotData;
                    // ������ B�� InventoryItemSlotData�� ĳ����
                    var itemB = b.data as InventoryItemSlotData;

                    // ������ A�� ID�� ���ڿ��� ��ȯ
                    var itemAIdStr = itemA.ItemId.ToString();
                    // ������ A�� �񱳰� ���� (11001 -> 1001)
                    var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2, 3);

                    // ������ B�� ID�� ���ڿ��� ��ȯ
                    var itemBIdStr = itemB.ItemId.ToString();
                    // ������ B�� �񱳰� ���� (11001 -> 1001)
                    var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);

                    // ������ Ÿ������ ��
                    int compareResult = itemAComp.CompareTo(itemBComp);

                    // ���� ������ Ÿ���� ���, ������ ������� ����
                    if (compareResult == 0)
                    {
                        // ������ ������� ���� (���� ����� ������)
                        compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);
                    }

                    // �� ��� ��ȯ
                    return compareResult;
                });
                break;
            // �⺻ ���̽� (ó�� ����)
            default:
                break;
        }
    }

    // ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnClickSortBtn()
    {
        // ���� ���� Ÿ�Կ� ���� �б� ó��
        switch (m_InventorySortType)
        {
            // ���� ������ ��� ������ ���
            case InventorySortType.ItemGrade:
                // ������ Ÿ�� ���ķ� ����
                m_InventorySortType = InventorySortType.ItemType;
                break;
            // ���� ������ Ÿ�� ������ ���
            case InventorySortType.ItemType:
                // ������ ��� ���ķ� ����
                m_InventorySortType = InventorySortType.ItemGrade;
                break;
            // �⺻ ���̽� (ó�� ����)
            default:
                break;
        }

        // ����� ���� Ÿ������ �κ��丮 ������
        SortInventory();
    }
}
