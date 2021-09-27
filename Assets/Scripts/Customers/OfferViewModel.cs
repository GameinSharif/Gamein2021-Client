using System;

public class OfferViewModel
{
    public string Company { get; }
    public string Type { get; }
    public int Volume { get; }
    public int CostPerUnit { get; }
    public int TotalCost { get; }
    public string EEA { get; }
    public string LEA { get; }
    public string Deadline { get; }
    public Frequency Frequency { get; }
    public State State { get; }


    public OfferViewModel(
        string company,
        string type,
        int volume,
        int costPerUnit,
        int total,
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

    public OfferViewModel(GetOffersTransitModel transitModel)
    {
        Company = transitModel.teamName;
        Type = transitModel.type;
        Volume = transitModel.volume;
        CostPerUnit = transitModel.costPerUnit;
        TotalCost = CostPerUnit * Volume;
        EEA = transitModel.earliestExpectedArrival.ToString();
        LEA = transitModel.latestExpectedArrival.ToString();
        Deadline = transitModel.offerDeadline.ToString();
        
        //TODO change default value
        Frequency = Frequency.ONCE;
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