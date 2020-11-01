using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour, ItemInterface
{
    protected abstract int itemId { set; get; }

    public int GetItemId()
    {
        return itemId;
    }

    public abstract void ObtainItem(PlayerManager player);
}
