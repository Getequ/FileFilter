using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace Getequ.FileFilter
{
    [SolutionTreeFilterProvider(GuidList.guidFileFilterCmdSetString, (uint)(PkgCmdIDList.cmdidInterceptorFilter))]
    public sealed class InterceptorFilterProvider : FilterProviderBase
    {
        [ImportingConstructor]
        public InterceptorFilterProvider(SVsServiceProvider serviceProvider, IVsHierarchyItemCollectionProvider hierarchyCollectionProvider)
            : base(serviceProvider, hierarchyCollectionProvider)
        {
            Match(@"\w*Interceptor.cs$");
        }
    }
}