using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Getequ.FileFilter
{
    [SolutionTreeFilterProvider(GuidList.guidFileFilterCmdSetString, (uint)(PkgCmdIDList.cmdidServiceFilter))]
    public sealed class ServiceFilterProvider : FilterProviderBase
    {
        [ImportingConstructor]
        public ServiceFilterProvider(SVsServiceProvider serviceProvider, IVsHierarchyItemCollectionProvider hierarchyCollectionProvider)
            : base(serviceProvider, hierarchyCollectionProvider)
        {
            Match(@"\w+Service.cs$");
            NotMatch(@"\w+DomainService.cs$|\w+ReportService.cs$");
        }
    }
}