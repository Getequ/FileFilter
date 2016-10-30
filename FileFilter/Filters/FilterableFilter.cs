using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Getequ.FileFilter
{
    [SolutionTreeFilterProvider(GuidList.guidFileFilterCmdSetString, (uint)(PkgCmdIDList.cmdidFilterableFilter))]
    public sealed class FilterableFilterProvider : FilterProviderBase
    {
        [ImportingConstructor]
        public FilterableFilterProvider(SVsServiceProvider serviceProvider, IVsHierarchyItemCollectionProvider hierarchyCollectionProvider)
            : base(serviceProvider, hierarchyCollectionProvider)
        {
            Match(@"Filterable\w+.cs$");
        }
    }
}