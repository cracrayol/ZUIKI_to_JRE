using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace ZUIKI_to_JRE.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
        if (ZUIKI_to_JRE.Properties.Resources.resourceMan == null)
          ZUIKI_to_JRE.Properties.Resources.resourceMan = new ResourceManager("ZUIKI_to_JRE.Properties.Resources", typeof (ZUIKI_to_JRE.Properties.Resources).Assembly);
        return ZUIKI_to_JRE.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => ZUIKI_to_JRE.Properties.Resources.resourceCulture;
      set => ZUIKI_to_JRE.Properties.Resources.resourceCulture = value;
    }
  }
}
