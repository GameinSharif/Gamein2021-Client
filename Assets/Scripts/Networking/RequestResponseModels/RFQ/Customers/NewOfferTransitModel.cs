using System;

[Serializable]
public class NewOfferTransitModel
{
    public string type;
    public int volume;
    public int costPerUnit;
    public CustomDateTime earliestExpectedArrival;
    public CustomDateTime latestExpectedArrival;
    public CustomDateTime offerDeadline;

    public NewOfferTransitModel(string type, int volume, int costPerUnit, CustomDateTime earliestExpectedArrival, CustomDateTime latestExpectedArrival, CustomDateTime offerDeadline)
    {
        this.type = type;
        this.volume = volume;
        this.costPerUnit = costPerUnit;
        this.earliestExpectedArrival = earliestExpectedArrival;
        this.latestExpectedArrival = latestExpectedArrival;
        this.offerDeadline = offerDeadline;
    }
}