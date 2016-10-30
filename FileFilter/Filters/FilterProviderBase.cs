using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

namespace Getequ.FileFilter
{
    public class FilterProviderBase : HierarchyTreeFilterProvider
    {
        private SVsServiceProvider _svcProvider;
        private IVsHierarchyItemCollectionProvider _hierarchyCollectionProvider;

        private IList<string> matchPatterns = new List<string>();
        private IList<string> notMatchPatterns = new List<string>();
        private Func<IVsHierarchyItem, bool> _shouldInclude = null;

        protected MatchPolicy MatchPolicy = MatchPolicy.All;
        protected MatchPolicy NotMatchPolicy = MatchPolicy.All;

        public FilterProviderBase(SVsServiceProvider serviceProvider, IVsHierarchyItemCollectionProvider hierarchyCollectionProvider)
        {
            _svcProvider = serviceProvider;
            _hierarchyCollectionProvider = hierarchyCollectionProvider;
        }

        // Returns an instance of Create filter class.
        protected override HierarchyTreeFilter CreateFilter()
        {
            if (_shouldInclude == null)
                return new FileNameFilter(_svcProvider, _hierarchyCollectionProvider,
                                          matchPatterns, notMatchPatterns, MatchPolicy, NotMatchPolicy);
            else
                return new FileNameFilter(_svcProvider, _hierarchyCollectionProvider, _shouldInclude);
        }

        protected void Match(string matchPattern)
        {
            matchPatterns.Add(matchPattern);
        }

        protected void NotMatch(string notMatchPattern)
        {
            notMatchPatterns.Add(notMatchPattern);
        }

        protected void ShouldIncludeFunc(Func<IVsHierarchyItem, bool> func)
        {
            _shouldInclude = func;
        }

        // Implementation of file filtering
        private sealed class FileNameFilter : HierarchyTreeFilter
        {
            private readonly IServiceProvider _svcProvider;
            private readonly IVsHierarchyItemCollectionProvider _hierarchyCollectionProvider;

            private IList<Regex> matchRegex = new List<Regex>();
            private IList<Regex> notMatchRegex = new List<Regex>();
            private Func<IVsHierarchyItem, bool> _shouldInclude = null;

            MatchPolicy _matchPolicy;
            MatchPolicy _notMatchPolicy;

            public FileNameFilter(
                IServiceProvider serviceProvider,
                IVsHierarchyItemCollectionProvider hierarchyCollectionProvider,
                IEnumerable<string> matchPatterns, IEnumerable<string> notMatchPatterns,
                MatchPolicy matchPolicy, MatchPolicy notMatchPolicy)
            {
                _svcProvider = serviceProvider;
                _hierarchyCollectionProvider = hierarchyCollectionProvider;

                foreach (var matchPattern in matchPatterns)
                {
                    matchRegex.Add(new Regex(matchPattern));
                }

                foreach (var notMatchPattern in notMatchPatterns)
                {
                    notMatchRegex.Add(new Regex(notMatchPattern));
                }

                _matchPolicy = matchPolicy;
                _notMatchPolicy = notMatchPolicy;
            }

            public FileNameFilter(
                IServiceProvider serviceProvider,
                IVsHierarchyItemCollectionProvider hierarchyCollectionProvider,
                Func<IVsHierarchyItem, bool> shouldInclude)
            {
                _svcProvider = serviceProvider;
                _hierarchyCollectionProvider = hierarchyCollectionProvider;

                _shouldInclude = shouldInclude;
            }

            // Gets the items to be included from this filter provider. 
            // rootItems is a collection that contains the root of your solution
            // Returns a collection of items to be included as part of the filter
            protected override async Task<IReadOnlyObservableSet> GetIncludedItemsAsync(IEnumerable<IVsHierarchyItem> rootItems)
            {
                IVsHierarchyItem root = HierarchyUtilities.FindCommonAncestor(rootItems);
                IReadOnlyObservableSet<IVsHierarchyItem> sourceItems;
                sourceItems = await _hierarchyCollectionProvider.GetDescendantsAsync(
                                    root.HierarchyIdentity.NestedHierarchy,
                                    CancellationToken);

                IFilteredHierarchyItemSet includedItems = await _hierarchyCollectionProvider.GetFilteredHierarchyItemsAsync(
                    sourceItems,
                    _shouldInclude == null ? (Predicate<IVsHierarchyItem>)ShouldIncludeInFilterByRegex : (Predicate<IVsHierarchyItem>)ShouldIncludeInFilterByFunc,
                    CancellationToken);
                return includedItems;
            }

            // Returns true if filters hierarchy item name for given filter; otherwise, false</returns>
            private bool ShouldIncludeInFilterByRegex(IVsHierarchyItem hierarchyItem)
            {
                if (hierarchyItem == null)
                {
                    return false;
                }
                var text = hierarchyItem.Text;

                return (_matchPolicy == MatchPolicy.All ? matchRegex.All(r => r.IsMatch(text)) : matchRegex.Any(r => r.IsMatch(text)))
                    && (_notMatchPolicy == MatchPolicy.All ? notMatchRegex.All(r => !r.IsMatch(text)) : notMatchRegex.Any(r => !r.IsMatch(text)));
            }

            private bool ShouldIncludeInFilterByFunc(IVsHierarchyItem hierarchyItem)
            {
                if (hierarchyItem == null)
                {
                    return false;
                }
                return _shouldInclude(hierarchyItem);
            }
        }
    }

    public enum MatchPolicy
    { 
        All,
        Any
    }
}