using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Groupplaner.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Groupplaner.Properties.Resources.resourceMan == null)
          Groupplaner.Properties.Resources.resourceMan = new ResourceManager("Groupplaner.Properties.Resources", typeof (Groupplaner.Properties.Resources).Assembly);
        return Groupplaner.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Groupplaner.Properties.Resources.resourceCulture;
      set => Groupplaner.Properties.Resources.resourceCulture = value;
    }
  }
}
