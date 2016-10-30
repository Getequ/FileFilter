using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Getequ.FileFilter
{
    [SolutionTreeFilterProvider(GuidList.guidFileFilterCmdSetString, (uint)(PkgCmdIDList.cmdidInterfaceFilter))]
    public sealed class InterfaceFilterProvider : FilterProviderBase
    {
        [ImportingConstructor]
        public InterfaceFilterProvider(SVsServiceProvider serviceProvider, IVsHierarchyItemCollectionProvider hierarchyCollectionProvider)
            : base(serviceProvider, hierarchyCollectionProvider)
        {
            Match(@"^([I]{1})([A-Z]{1})(\w+)(.cs)$");
        }
    }
}