using UnityEngine;

// Tanga: mashina tegsa ball beradi va boshqa joyga ko'chadi
public class Coin : MonoBehaviour
{
    public int value = 10;
    public float areaSize = 35f; // tanga paydo bo'ladigan maydon o'lchami

    void Update()
    {
        transform.Rotate(0f, 0f, 150f * Time.deltaTime); // aylanib turadi
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(value);
            MoveToRandomPlace();
        }
    }

    void MoveToRandomPlace()
    {
        float x = Random.Range(-areaSize, areaSize);
        float z = Random.Range(-areaSize, areaSize);
        transform.position = new Vector3(x, 1f, z);
    }
}
