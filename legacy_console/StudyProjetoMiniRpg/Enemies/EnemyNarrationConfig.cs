using System.Collections.Generic;

public class DualStateLines
{
    public List <string> NoRevel {get; set;} = new();
    public List <string> Revel {get; set;} = new();

}

public class EnemyNarrationSet
{
    public DualStateLines OnAppear { get; set; } = new();
    public DualStateLines OnAttack { get; set; } = new();

    public List<string> OnHurt { get; set; } = new();
    public List<string> OnDeath { get; set; } = new();

}