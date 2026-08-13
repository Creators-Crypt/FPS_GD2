public interface IConcentration 
{
    float Current { get; }
    float Ratio { get; }

    
    bool Spend(float cost);

    void Refill();
    float GetDamageMultiplier();
  
}
