using System;

namespace Composition.UI
{
    public class PageItem
    {
        public string Name { get; internal set; }
        public Type Page { get; internal set; }
    }
}