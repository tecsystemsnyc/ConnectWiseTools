namespace TECUserControlLibrary.ViewModels.Interfaces
{
    public interface IComponentSummaryVM
    {
        double TotalTECCost { get; }
        double TotalTECLabor { get; }
        double TotalElecCost { get; }
        double TotalElecLabor { get; }
    }
}
