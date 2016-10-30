using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Getequ.FileFilter
{
    [SolutionTreeFilterProvider(GuidList.guidFileFilterCmdSetString, (uint)(PkgCmdIDList.cmdidMapFilter))]
    public sealed class MapFilterProvider : FilterProviderBase
    {
        [ImportingConstructor]
        public MapFilterProvider(SVsServiceProvider serviceProvider, IVsHierarchyItemCollectionProvider hierarchyCollectionProvider)
            : base(serviceProvider, hierarchyCollectionProvider)
        {
            Match(@"\w+Map.cs$");
            NotMatch(@"\w+LogMap.cs$");
        }
    }
}