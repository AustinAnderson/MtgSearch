namespace MtgSearch.Server.Models.Logic.Predicates
{
    public interface IParsibleFunction
    {
        string ParseAs { get; }
        string[] Signitures { get; }
        string[] Comments { get; }
        string[] Examples { get; }
        public ISearchPredicate Factory(string[] args, string context);
    }
}
