#pragma checksum "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "79c17cc848f3bf90578cce679d648458b72fb414"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_RehearsalStudios_Delete), @"mvc.1.0.view", @"/Views/RehearsalStudios/Delete.cshtml")]
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
#line 4 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\_ViewImports.cshtml"
using EasyRehearsalManager.Model;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\_ViewImports.cshtml"
using EasyRehearsalManager.Web.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\_ViewImports.cshtml"
using Syncfusion.JavaScript;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"79c17cc848f3bf90578cce679d648458b72fb414", @"/Views/RehearsalStudios/Delete.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"21786c7fdc9c93112993988f4bd372f43d166063", @"/Views/_ViewImports.cshtml")]
    public class Views_RehearsalStudios_Delete : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<RehearsalStudio>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
  
    ViewData["Title"] = "Próbahely törlése";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Törlés</h1>\r\n\r\n<h3>Biztosan törölni akarja?</h3>\r\n<div>\r\n    <h4>RehearsalStudio</h4>\r\n    <hr />\r\n    <dl class=\"row\">\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 15 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 18 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 21 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.Address));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 24 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.Address));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 27 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.LocationX));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 30 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.LocationX));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 33 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.LocationY));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 36 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.LocationY));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 39 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.District));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 42 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.District));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 45 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.Phone));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 48 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.Phone));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 51 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.Email));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 54 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.Email));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 57 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.Web));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 60 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.Web));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 63 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.NumberOfRooms));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 66 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.NumberOfRooms));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 69 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.Description));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 72 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.Description));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 75 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.OpeningHourMonday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 78 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.OpeningHourMonday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 81 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ClosingHourMonday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 84 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ClosingHourMonday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 87 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.OpeningHourTuesday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 90 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.OpeningHourTuesday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 93 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ClosingHourTuesday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 96 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ClosingHourTuesday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 99 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.OpeningHourWednesday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 102 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.OpeningHourWednesday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 105 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ClosingHourWednesday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 108 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ClosingHourWednesday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 111 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.OpeningHourThursday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 114 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.OpeningHourThursday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 117 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ClosingHourThursday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 120 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ClosingHourThursday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 123 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.OpeningHourFriday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 126 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.OpeningHourFriday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 129 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ClosingHourFriday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 132 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ClosingHourFriday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 135 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.OpeningHourSaturday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 138 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.OpeningHourSaturday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 141 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ClosingHourSaturday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 144 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ClosingHourSaturday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 147 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.OpeningHourSunday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 150 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.OpeningHourSunday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class = \"col-sm-2\">\r\n            ");
#nullable restore
#line 153 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayNameFor(model => model.ClosingHourSunday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class = \"col-sm-10\">\r\n            ");
#nullable restore
#line 156 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
       Write(Html.DisplayFor(model => model.ClosingHourSunday));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n    </dl>\r\n    \r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "79c17cc848f3bf90578cce679d648458b72fb41421918", async() => {
                WriteLiteral("\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "79c17cc848f3bf90578cce679d648458b72fb41422185", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 161 "C:\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\RehearsalStudios\Delete.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.Id);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n        <input type=\"submit\" value=\"Törlés\" class=\"btn btn-danger\" /> |\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "79c17cc848f3bf90578cce679d648458b72fb41423995", async() => {
                    WriteLiteral("Vissza a próbahelyek listájához");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<RehearsalStudio> Html { get; private set; }
    }
}
#pragma warning restore 1591