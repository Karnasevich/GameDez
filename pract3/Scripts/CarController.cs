using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Налаштування швидкості")]
    public float motorForce = 5000f; // Сила прискорення
    public float maxSpeed = 50f;     // Максимальна швидкість

    [Header("Налаштування керування")]
    public float steerForce = 60f;  // Швидкість повороту керма

    [Range(0f, 1f)]
    public float grip = 0.9f;        // Зчеплення: 1 - машина їде як по рельсах, 0 - як по льоду

    private float moveInput;
    private float turnInput;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Знижуємо центр мас, щоб уникнути перекидання
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void Update()
    {
        // Отримуємо введення від гравця
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        // 1. Прискорення з урахуванням ліміту швидкості
        // Додаємо силу тільки якщо поточна швидкість менша за максимальну
        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddRelativeForce(Vector3.forward * moveInput * motorForce);
        }

        // 2. Поворот (працює, якщо машина хоч трохи котиться)
        float currentSpeed = rb.linearVelocity.magnitude;
        if (currentSpeed > 0.5f)
        {
            // Визначаємо, куди їде машина (вперед чи назад), щоб правильно інвертувати кермо при задньому ході
            float direction = Vector3.Dot(rb.linearVelocity, transform.forward) > 0 ? 1f : -1f;

            // Застосовуємо обертання
            Quaternion turnRotation = Quaternion.Euler(0f, turnInput * steerForce * direction * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        // 3. Фізика зчеплення (гасіння бокового ковзання)
        ApplyArcadeGrip();
    }

    void ApplyArcadeGrip()
    {
        // Переводимо глобальну швидкість у локальну (відносно самої машини)
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);

        // Зменшуємо швидкість по осі X (бокове ковзання) залежно від параметра grip
        localVelocity.x *= (1f - grip);

        // Повертаємо локальну швидкість назад у глобальну і застосовуємо до Rigidbody
        rb.linearVelocity = transform.TransformDirection(localVelocity);
    }
}