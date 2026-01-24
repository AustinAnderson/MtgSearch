using MtgSearch.Server.Models.Logic.Predicates;
using MtgSearch.Server.Models.Logic.Predicates.Functions;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Logic.Parsing
{
    public class FunctionList
    {
        public static IReadOnlyDictionary<string, IFunctionInfo> ByName => _functionsByName;
        private readonly static Dictionary<string, IFunctionInfo> _functionsByName = new IFunctionInfo[] {
            TextSearchPredicate.FunctionInfo,
            ActivatedAbilitySearchPredicate.FunctionInfo,
            CanBeCommanderPredicate.FunctionInfo,
            HasAltFacePredicate.FunctionInfo,
            IsMultiColoredPredicate.FunctionInfo,
            IsPreReleasePredicate.FunctionInfo,
            NameLikePredicate.FunctionInfo,
            ManaSymbolLikeSearch.FunctionInfo,
            InSetPredicate.FunctionInfo,
            HasKeyWordPredicate.FunctionInfo,
            new HasSuperTypeFunctionInfo(),
            new AllSuperTypesFunctionInfo(),
            new AnySuperTypesFunctionInfo(),
            new HasTypeFunctionInfo(),
            new AllTypesFunctionInfo(),
            new AnyTypesFunctionInfo(),
            new HasSubTypeFunctionInfo(),
            new AllSubTypesFunctionInfo(),
            new AnySubTypesFunctionInfo(),
        }.ToDictionary(x => x.ParseAs);
    }
}
