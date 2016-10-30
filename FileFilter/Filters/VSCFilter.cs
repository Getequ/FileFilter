using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Getequ.FileFilter
{
    [SolutionTreeFilterProvider(GuidList.guidFileFilterCmdSetString, (uint)(PkgCmdIDList.cmdidVSCFilter))]
    public sealed class VSCFilterProvider : FilterProviderBase
    {
        [ImportingConstructor]
        public VSCFilterProvider(SVsServiceProvider serviceProvider, IVsHierarchyItemCollectionProvider hierarchyCollectionProvider)
            : base(serviceProvider, hierarchyCollectionProvider)
        {
            Match(@"\w+ViewModel.cs$");
            Match(@"\w+Service.cs$");
            Match(@"\w+Controller.cs$");

            NotMatch(@"\w+DomainService.cs$");

            MatchPolicy = FileFilter.MatchPolicy.Any;
        }
    }
}