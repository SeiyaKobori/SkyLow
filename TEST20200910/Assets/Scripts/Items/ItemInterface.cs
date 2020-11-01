using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ItemInterface
{
    void ObtainItem(PlayerManager player);
    int GetItemId();
}
