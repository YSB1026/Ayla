public interface ISlowable
{
    // multiplier: 0.5f면 반으로 줄고, 1.0f면 원상복구
    void SetSlowFactor(float multiplier);
}