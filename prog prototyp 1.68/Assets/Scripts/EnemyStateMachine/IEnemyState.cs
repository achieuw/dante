using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState {

    void UpdateState();

    void OnTrigger(Collider2D other);

    void OnCollision(Collision2D other);

    void ToIdleState();

    void ToChaseState();

    void ToAttackState();
}
