using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell;

namespace Getequ.FileFilter
{
    internal static class Utilities
    {
        // Finds the first common ancestor for items
        public static IVsHierarchyItem FindCommonAncestor(IEnumerable<IVsHierarchyItem> items)
        {
            IVsHierarchyItem commonParent = null;
            if (items != null)
            {
                foreach (IVsHierarchyItem item in items)
                {
                    commonParent = commonParent == null ? item : FindCommonAncestor(commonParent, item, h => h.Parent);
                    // If we don't find a common parent, stop iterating
                    if (commonParent == null)
                    {
                        break;
                    }
                }
            }
            return commonParent;
        }

        // Finds the common ancestor of obj1 and obj2 using the given function to evaluate parent.
        private static IVsHierarchyItem FindCommonAncestor(
            IVsHierarchyItem obj1,
            IVsHierarchyItem obj2,
            Func<IVsHierarchyItem, IVsHierarchyItem> parentEvaluator)
        {
            if (obj1 == null || obj2 == null)
            {
                return null;
            }

            HashSet<IVsHierarchyItem> map = new HashSet<IVsHierarchyItem>();
            while (obj1 != null)
            {
                map.Add(obj1);
                obj1 = parentEvaluator(obj1);
            }

            while (obj2 != null)
            {
                if (map.Contains(obj2))
                {
                    return obj2;
                }

                obj2 = parentEvaluator(obj2);
            }

            return null;
        }
    }
}