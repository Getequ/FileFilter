using System.ComponentModel.Composition;
using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell;

namespace Getequ.FileFilter
{
    [SolutionTreeFilterProvider(GuidList.guidFileFilterCmdSetString, (uint)(PkgCmdIDList.cmdidEntityFilter))]
    public sealed class EntityFilterProvider : FilterProviderBase
    {
        [ImportingConstructor]
        public EntityFilterProvider(SVsServiceProvider serviceProvider, IVsHierarchyItemCollectionProvider hierarchyCollectionProvider)
            : base(serviceProvider, hierarchyCollectionProvider)
        {
            ShouldIncludeFunc(item =>
            {
                try
                {
                    if (item.Parent == null)
                        return false;

                    if (item.Parent.Text == "Entities")
                        return true;

                    if (item.Parent.Parent == null)
                        return false;

                    if (item.Parent.Parent.Text == "Entities")
                        return true;
                }
                catch(Exception ee)
                {
                    Debug.WriteLine(item.Text + ": " + ee.Message);
                    return false;
                }
                return false;
            });
        }
    }
}