#pragma checksum "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dd883b5c1abd897f88d502ff66acf65619556505"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Reservations_Details), @"mvc.1.0.view", @"/Views/Reservations/Details.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dd883b5c1abd897f88d502ff66acf65619556505", @"/Views/Reservations/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"21786c7fdc9c93112993988f4bd372f43d166063", @"/Views/_ViewImports.cshtml")]
    public class Views_Reservations_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Reservation>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-primary align-baseline"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-secondary align-baseline"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
  
    ViewData["Title"] = "Foglalás részletei";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1 class=\"display-4\">Foglalás részletei</h1>\r\n\r\n<table class=\"table table-striped w-auto\">\r\n    <tr>\r\n        <th>Foglalás kezdete:</th>\r\n        <td>");
#nullable restore
#line 12 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
       Write(Model.Start);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n    </tr>\r\n    <tr>\r\n        <th>Foglalás vége:</th>\r\n        <td>");
#nullable restore
#line 16 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
       Write(Model.End);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n    </tr>\r\n    <tr>\r\n        <th>Zenekar neve:</th>\r\n        <td>");
#nullable restore
#line 20 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
       Write(Model.BandName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n    </tr>\r\n    <tr>\r\n        <th>Foglaló neve:</th>\r\n        <td>");
#nullable restore
#line 24 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
       Write(Model.User.UserOwnName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n    </tr>\r\n    <tr>\r\n        <th>Próbahely neve:</th>\r\n        <td>");
#nullable restore
#line 28 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
       Write(Model.RehearsalRoom.Studio.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n    </tr>\r\n    <tr>\r\n        <th>Terem száma:</th>\r\n        <td>");
#nullable restore
#line 32 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
       Write(Model.RehearsalRoom.Number);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n    </tr>\r\n    <tr>\r\n        <th>Bérelt eszközök:</th>\r\n");
#nullable restore
#line 36 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
           
            if (ViewBag.Equipments == null || ViewBag.Equipments.Count == 0)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <td>Ön nem bérelt semmit a foglaláshoz.</td>\r\n");
#nullable restore
#line 40 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
            }
            else
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <td>\r\n");
#nullable restore
#line 44 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
                 foreach(var e in ViewBag.Equipments)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div>");
#nullable restore
#line 46 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
                    Write(e);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 47 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </td>\r\n");
#nullable restore
#line 49 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
            }
        

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </tr>\r\n</table>\r\n\r\n<div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "dd883b5c1abd897f88d502ff66acf656195565059173", async() => {
                WriteLiteral("Szerkesztés");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-reservationId", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 56 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Reservations\Details.cshtml"
                                      WriteLiteral(Model.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["reservationId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-reservationId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["reservationId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "dd883b5c1abd897f88d502ff66acf6561955650511495", async() => {
                WriteLiteral("Vissza a foglalásokhoz");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Reservation> Html { get; private set; }
    }
}
#pragma warning restore 1591
