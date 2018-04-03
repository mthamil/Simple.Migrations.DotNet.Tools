using Microsoft.Extensions.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Migrations.Tools.DotNet.Utilities.Localization
{
    public class CommandDescriptionProvider<TResources> : IReadOnlyDictionary<string, string>
    {
        private readonly IStringLocalizer _localizer;
        private readonly Lazy<IEnumerable<LocalizedString>> _allStrings;

        public CommandDescriptionProvider(IStringLocalizerFactory stringLocalizerFactory)
        {
            _localizer = stringLocalizerFactory.Create(typeof(TResources));
            _allStrings = new Lazy<IEnumerable<LocalizedString>>(_localizer.GetAllStrings);
        }

        public string this[string key] => _localizer[key];

        public bool ContainsKey(string key) => _localizer[key] != null;

        public IEnumerable<string> Keys => _allStrings.Value.Select(s => s.Name);

        public IEnumerable<string> Values => _allStrings.Value.Select(s => s.Value);

        public bool TryGetValue(string key, out string value) => (value = _localizer[key]) != null;

        public int Count => _allStrings.Value.Count();

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() =>
            _allStrings.Value
                       .Select(s => new KeyValuePair<string, string>(s.Name, s.Value))
                       .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
