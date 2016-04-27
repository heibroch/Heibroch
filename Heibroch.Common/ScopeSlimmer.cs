using System;
using System.Collections.Generic;
using System.Linq;

namespace Heibroch.Common.Wpf
{
    public class ScopeSlimmer<T>
    {
        public Func<T, string> MatchProperty { get; set; }
        public IEnumerable<T> CurrentScope { get; set; }
        public IEnumerable<T> InitialScope { get; set; }
        
        public ScopeSlimmer(IEnumerable<T> initialScope, Func<T, string> matchProperty)
        {
            MatchProperty = matchProperty;
            InitialScope = initialScope;
            CurrentScope = initialScope;
        }

        public void Search(string query)
        {
            if(CurrentScope == null)
                
            CurrentScope = CurrentScope.Where(x => MatchProperty(x).Contains(query));
        }
    }
}
