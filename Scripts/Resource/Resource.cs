using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private float _timer;

    public void MoveToLocalParent(float timeToReach)
    {
        if (timeToReach > 0)
        {
            StartCoroutine(MoveTo(transform.parent, timeToReach));
        }
        else
        {
            transform.position = transform.parent.position;
        }
    }

    private IEnumerator MoveTo(Transform target, float timeToReach)
    {
        while (true)
        {
            _timer += Time.fixedDeltaTime / timeToReach;
            transform.position = Vector3.Lerp(transform.position, target.position, _timer);

            if (_timer >= timeToReach)
            {
                transform.position = target.position;
                _timer = 0;
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
