namespace Spreads.Slang.FParsec

open System.Reflection
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

[<assembly: ComVisible(false)>]

#if LOW_TRUST
    [<assembly: System.Security.AllowPartiallyTrustedCallers>]
    [<assembly: System.Security.SecurityTransparent>]
#endif
[<assembly: InternalsVisibleTo(CommonAssemblyInfo.TestAssemblyName + CommonAssemblyInfo.StrongNamePublicKey)>]
do ()