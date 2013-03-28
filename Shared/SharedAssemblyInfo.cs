using System.Reflection;
using System.Resources;

[assembly: AssemblyCompany("Randy Ridge")]
#if CODE_ANALYSIS
[assembly: AssemblyConfiguration("ANALYSIS")]
#elif DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif
[assembly: AssemblyCopyright("Copyright Randy Ridge © 2013.  All rights reserved.")]
[assembly: AssemblyTrademark("Randy Ridge")]
[assembly: NeutralResourcesLanguage("en-US")]
