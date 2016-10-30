using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Getequ.FileFilter
{
    [SolutionTreeFilterProvider(GuidList.guidFileFilterCmdSetString, (uint)(PkgCmdIDList.cmdidHelperFilter))]
    public sealed class HelperFilterProvider : FilterProviderBase
    {
        [ImportingConstructor]
        public HelperFilterProvider(SVsServiceProvider serviceProvider, IVsHierarchyItemCollectionProvider hierarchyCollectionProvider)
            : base(serviceProvider, hierarchyCollectionProvider)
        {
            Match(@"\w+Helper.cs$");
        }
    }
}