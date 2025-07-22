using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace MtgSearch.Server.Models.Data
{
    public class ColorIdentity
    {
        private readonly static Regex Validate = new Regex("b?g?r?u?w?");
        public ColorIdentity(string colors)
        {
            this.colors = colors.Select(x => (""+x).ToLower()).OrderBy(x => x).ToArray();
            if (!Validate.IsMatch(string.Join("", this.colors)))
            {
                throw new ArgumentException($"only up to one of each of `wubrg` allowed, got `{colors}`");
            }
            Id = "{c}";
            if (this.colors.Length > 0)
            {
                Id = $"{{{string.Join("}{", this.colors)}}}";
            }
        }
        private string[] colors;
        public string[] Colors => colors;
        public string Id { get; }
        public static bool operator ==(ColorIdentity left, ColorIdentity right)
            => left?.Id == right?.Id;
        public static bool operator !=(ColorIdentity left, ColorIdentity right)
            => left?.Id != right?.Id;
        public override int GetHashCode() => Id.GetHashCode();
        public override bool Equals(object? obj) 
            => obj != null && obj is ColorIdentity other && other == this;

        //TODO: pre-sorted on startup, could do a diff walk algo for speed up if network is not the bottle-neck
        public bool IncludedIn(ColorIdentity other)
        {
            if (other.colors.Length == 0) return true;
            foreach (var color in colors)
            {
                if (!other.colors.Contains(color)) return false;
            }
            return true;
        }
}
