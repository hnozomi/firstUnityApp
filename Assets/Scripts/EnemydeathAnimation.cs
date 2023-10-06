using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemydeathAnimation : MonoBehaviour
{
    public void OnCompleteAnimation()
    {
        Destroy(this.gameObject);
    }
}
