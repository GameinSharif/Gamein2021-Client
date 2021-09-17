using System;

public class Provider
{
    public string Company { get; }
    public string Type { get; }
    public int MaxMonthlyCap { get; }
    public double ProviderAverageCost { get; }
    public double ProviderMinOnRecord { get; }
    public double ProviderMaxOnRecord { get; }


    public Provider(
        string company,
        string type,
        int maxMonthlyCap,
        double providerAverageCost,
        double providerMinOnRecord,
        double providerMaxOnRecord)
    {
        Company = company;
        Type = type;
        MaxMonthlyCap = maxMonthlyCap;
        ProviderAverageCost = providerAverageCost;
        ProviderMinOnRecord = providerMinOnRecord;
        ProviderMaxOnRecord = providerMaxOnRecord;
    }
}
