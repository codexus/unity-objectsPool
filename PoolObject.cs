using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    Pool pool;

    public void Init(Pool pool) { this.pool = pool; }

    public void ReturnToPool()
    {
        pool?.ReturnToPool(gameObject);
    }
}
