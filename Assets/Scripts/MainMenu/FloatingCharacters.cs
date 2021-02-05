using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCharacters : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;      // 스프라이트와 색상을 정함
    private Vector3 direction;                  // 캐릭터가 날아다니는 방향을 나타냄
    private float floatingSpeed;                // 캐릭터가 날아다니는 속도
    private float rotateSpeed;                  // 캐릭터 날아다니면서 회전하는 속도


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CharacterFloating();

    }

    // size : 캐릭터의 크기
    public void SetFloatingCharacter(Sprite sprite, Vector3 direction, float floatingSpeed,
        float rotateSpeed, float size)
    {
        this.direction = direction;
        this.floatingSpeed = floatingSpeed;
        this.rotateSpeed = rotateSpeed;

        spriteRenderer.sprite = sprite;

        transform.localScale = new Vector3(size, size, size);          // 캐릭터의 크기 조정
        spriteRenderer.sortingOrder = (int)Mathf.Lerp(1, 32767, size); // 큰 크루원이 작은 크루원보다 앞서도록 함
    }

    // 캐릭터를 회전시키고 이동시킴
    private void CharacterFloating()
    {
        transform.position += direction * floatingSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 0f, rotateSpeed));
    }
}
