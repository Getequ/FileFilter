using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Getequ.FileFilter
{
    [SolutionTreeFilterProvider(GuidList.guidFileFilterCmdSetString, (uint)(PkgCmdIDList.cmdidReportFilter))]
    public sealed class ReportFilterProvider : FilterProviderBase
    {
        [ImportingConstructor]
        public ReportFilterProvider(SVsServiceProvider serviceProvider, IVsHierarchyItemCollectionProvider hierarchyCollectionProvider)
            : base(serviceProvider, hierarchyCollectionProvider)
        {
            Match(@"\w+Report.cs$|\w+PrintForm.cs$");
        }
    }
}