using System;

[Serializable]
public class GetOffersTransitModel
{
    public string teamName;
    public string type;
    public int volume;
    public int costPerUnit;
    public CustomDateTime earliestExpectedArrival;
    public CustomDateTime latestExpectedArrival;
    public CustomDateTime offerDeadline;
}