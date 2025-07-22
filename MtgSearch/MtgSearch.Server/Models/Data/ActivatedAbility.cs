using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Data
{
    public class ActivatedAbility
    {
        private static Regex costSplitter = new Regex(" and |,", RegexOptions.Compiled);
        private static Regex splitter = new Regex("^([^:]*): ?", RegexOptions.Compiled);
        private static Regex checkForUnquotedSecondColon = new Regex("^[\"]*:", RegexOptions.Compiled);
        private static Regex[] trimStarts = [
            new Regex("^[^\\(]*\\(", RegexOptions.Compiled),
            new Regex("^[^\"]*\"", RegexOptions.Compiled),
            new Regex("^[^\u2014]*\u2014 ", RegexOptions.Compiled),
        ];
        private static Regex[] trimEnds = [
            new Regex("\\)[^\\)]*$", RegexOptions.Compiled),
            new Regex("\"[^\"]*$", RegexOptions.Compiled)
        ];
        private static Regex NameSanitizer = new Regex(@"([\^\+\*\(\)\{\}\[\]])", RegexOptions.Compiled);
        public ActivatedAbility(string abilityLineWithColon, string parentName)
        {
            var split = splitter.Split(abilityLineWithColon).Skip(1).ToArray();
            if (split.Length != 2)
            {
                throw new ArgumentException($"card {parentName} tried to make activated ability without colon with line\n'{abilityLineWithColon}'");
            }
            if (checkForUnquotedSecondColon.IsMatch(split[1]))
            {
                throw new ArgumentException($"card {parentName} had second colon in activated ability line\n'{abilityLineWithColon}'");
            }


            var costHalf = split[0];
            foreach (var trimmer in trimStarts)
            {
                costHalf = trimmer.Replace(costHalf, "");
            }
            var abilityHalf = split[1];
            foreach (var trimmer in trimEnds)
            {
                abilityHalf = trimmer.Replace(abilityHalf, "");
            }
            var nameReplace = new Regex(
                NameSanitizer.Replace(parentName, "\\$1"),
                RegexOptions.IgnoreCase
            );
            costs = costSplitter.Split(costHalf).Select(cost => {
                cost = nameReplace.Replace(cost, "this card");
                return cost.Trim();
            }).ToArray();
            ability = abilityHalf;
        }
        public string[] costs;
        public string ability;
        public override string ToString() => $"[{string.Join(",", costs)}] => {ability}";

    }
}
