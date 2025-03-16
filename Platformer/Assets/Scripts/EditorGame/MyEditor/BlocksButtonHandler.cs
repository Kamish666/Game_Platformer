using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksButtonHandler : MonoBehaviour
{
    [SerializeField] private BlockCreator _blockCreator;

    private void Awake()
    {
        _blockCreator = BlockCreator.GetInstance();
    }

    public void ButtonClicked(BuildingBlockBase item)
    {
        Debug.Log("Button was clicked: " + item.name);
        _blockCreator.ObjectSelected(item);
    }
}
