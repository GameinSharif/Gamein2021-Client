using System;

public class Offer
{
    public string Company { get; }
    public string Type { get; }
    public int Volume { get; }
    public double CostPerUnit { get; }
    public double TotalCost { get; }
    public string EEA { get; }
    public string LEA { get; }
    public string Deadline { get; }
    public Frequency Frequency { get; }
    public State State { get; }


    public Offer(
        string company,
        string type,
        int volume,
        double costPerUnit,
        double total,
        string EEA,
        string LEA,
        string deadline,
        Frequency frequency,
        State state)
    {
        Company = company;
        Type = type;
        Volume = volume;
        CostPerUnit = costPerUnit;
        TotalCost = total;
        this.EEA = EEA;
        this.LEA = LEA;
        Deadline = deadline;
        Frequency = frequency;
        State = state;
    }
}

public enum Frequency
{
    ONCE, MONTHLY
}

public enum State
{
    INPROGRESS, DEAL, CLOSED
}