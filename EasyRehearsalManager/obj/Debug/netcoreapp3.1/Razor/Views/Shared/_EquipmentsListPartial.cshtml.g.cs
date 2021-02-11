#pragma checksum "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\_EquipmentsListPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0fc017ad75c5a08d0331e600ace4fd4afbb3c293"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__EquipmentsListPartial), @"mvc.1.0.view", @"/Views/Shared/_EquipmentsListPartial.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 4 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\_ViewImports.cshtml"
using EasyRehearsalManager.Model;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\_ViewImports.cshtml"
using EasyRehearsalManager.Web.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\_ViewImports.cshtml"
using Syncfusion.JavaScript;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0fc017ad75c5a08d0331e600ace4fd4afbb3c293", @"/Views/Shared/_EquipmentsListPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"21786c7fdc9c93112993988f4bd372f43d166063", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__EquipmentsListPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<RehearsalRoom>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<h1>Terem Nr. ");
#nullable restore
#line 3 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\_EquipmentsListPartial.cshtml"
         Write(Model.Number);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n\r\n<h4>Bérelhető eszközök</h4>\r\n");
#nullable restore
#line 6 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\_EquipmentsListPartial.cshtml"
 if (Model.Studio.Equipments == null || !Model.Studio.Equipments.Any())
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p>Nincsenek bérelhető eszközök ehhez a próbahelyhez regisztrálva.</p>\r\n");
#nullable restore
#line 9 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\_EquipmentsListPartial.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <table class=\"table table-borderless w-auto\">\r\n");
#nullable restore
#line 13 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\_EquipmentsListPartial.cshtml"
         foreach (Equipment eq in Model.Studio.Equipments)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <th>");
#nullable restore
#line 16 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\_EquipmentsListPartial.cshtml"
               Write(eq.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</th>\r\n                <td>");
#nullable restore
#line 17 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\_EquipmentsListPartial.cshtml"
               Write(eq.QuantityAvailable);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Db</td>\r\n            </tr>\r\n");
#nullable restore
#line 19 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\_EquipmentsListPartial.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </table>\r\n");
#nullable restore
#line 21 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\_EquipmentsListPartial.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<RehearsalRoom> Html { get; private set; }
    }
}
#pragma warning restore 1591