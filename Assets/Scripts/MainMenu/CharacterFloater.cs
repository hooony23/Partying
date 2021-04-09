using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFloater : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // 소환하고 날려버릴 캐릭터 프리팹 저장
    [SerializeField] private List<Sprite> sprites; // 캐릭터들을 담을 배열

    private float timer = 2f; // 소환하는 사이의 간격
    private float distance = 11f; // 중심으로부터 소환되는 거리

    private void Start()
    {
        // 생성할 캐릭터 중심으로부터의 거리 랜덤
        SpawnFloatingCrew(Random.Range(0f, distance));
    }

    void Update()
    {
        // 일정시간마다 호출
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnFloatingCrew(distance);
            timer = 1f;
        }
    }

    // 캐릭터 소환
    public void SpawnFloatingCrew(float dist)
    {
        // 중심을 기준으로 소환할 캐릭터의 각도
        float angle = Random.Range(0f, 360f);

        // 중심으로 부터 원형의 방향을 돌아가며 가리키는 벡터
        Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * distance;

        // 날아가는 방향, 속도, 회전속도
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        float floatingSpeed = Random.Range(1f, 4f);
        float rotateSpeed = Random.Range(-1f, 1f);

        // 캐릭터 소환
        var character = Instantiate(prefab, spawnPos, Quaternion.identity).GetComponent<FloatingCharacters>();
        character.SetFloatingCharacter(sprites[Random.Range(0, sprites.Count)],
            direction, floatingSpeed, rotateSpeed, Random.Range(2f, 6f));
    }

    
    // 캐릭터 화면 밖으로 나가면 사라짐
    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<FloatingCharacters>();
        if (character != null)
        {
            Destroy(character.gameObject);
        }
    }
}
