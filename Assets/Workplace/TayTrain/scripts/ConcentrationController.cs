using UnityEngine;

public class ConcentrationController : MonoBehaviour //,IConcentration
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private float currentConcentration;
    public float Current => currentConcentration;
    public float Ratio => stats ? currentConcentration / stats.maxConcentration : 0f;
    private void Awake()
    {
        currentConcentration = stats.maxConcentration;
    }
    public void Spend(float cost)
    {
        currentConcentration = Mathf.Clamp(currentConcentration -  cost, 0f, stats.maxConcentration);
        //currentConcentration -= Spell.concentrationCost;
    }
    public float GetDamageMultipler()
    {
        if (currentConcentration >= 400f)
            return 2f;
        if (currentConcentration >= 300f)
            return 1.75f;
        if (currentConcentration >= 200f)
            return 1.5f;
        if (currentConcentration >= 100f)
            return 1.25f;

        return 1f;
    }
    public void refill()
    {
        currentConcentration = stats.maxConcentration;
    }
    //public void spendConcentration(cost)
    //{
    //    stats.currentConcentration -= cost;
    //    if (currentConcentration > 400f)
    //    {
            
    //    }
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
      //if(Input.GetKeyDown(KeyCode.V))
      //  {
      //      Spend(50f);

      //      Debug.Log("Concentration " + currentConcentration +
      //          " | Damage Multiplier: " + GetDamageMultipler());
      //  }
      //if(Input.GetKeyDown(KeyCode.B))
      //  {
      //      refill();
      //      Debug.Log("Concentration Refilled: " + currentConcentration);
      //  }
    }
}
