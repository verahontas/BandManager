#pragma checksum "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "708260af63eb3e05f9b82b1c8c328b84ba75d8d3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_ReservationsTable_Default), @"mvc.1.0.view", @"/Views/Shared/Components/ReservationsTable/Default.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"708260af63eb3e05f9b82b1c8c328b84ba75d8d3", @"/Views/Shared/Components/ReservationsTable/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"21786c7fdc9c93112993988f4bd372f43d166063", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_ReservationsTable_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ReservationTableViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "RehearsalStudios", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ReservationsTable", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Reservations", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "CreateFromTable", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            WriteLiteral("\r\n<table>\r\n    <tr>\r\n        <td>\r\n");
#nullable restore
#line 13 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
             if (Model.Index >= 0)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "708260af63eb3e05f9b82b1c8c328b84ba75d8d35186", async() => {
                WriteLiteral("Előző nap");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-studioId", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 15 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                            WriteLiteral(Model.Studio.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["studioId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-studioId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["studioId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 15 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                                                                WriteLiteral(false);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["isNext"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-isNext", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["isNext"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 16 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </td>\r\n        <td>\r\n            <div>\r\n                <table class=\"table table-bordered table-dark\">\r\n                    <tr>\r\n                        <th></th>\r\n");
#nullable restore
#line 23 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                         for (int hour = Model.OpeningHour; hour < Model.ClosingHour; ++hour)
                        {
                            var end = hour + 1;

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <th scope=\"col\">");
#nullable restore
#line 26 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                       Write(hour);

#line default
#line hidden
#nullable disable
            WriteLiteral(" - ");
#nullable restore
#line 26 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                               Write(end);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n");
#nullable restore
#line 27 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </tr>\r\n");
#nullable restore
#line 29 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                     for (int i = 0; i < Model.NumberOfAvailableRooms; ++i)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr>\r\n                            <td>\r\n                                ");
#nullable restore
#line 33 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                           Write(Model.Rooms[i].Number);

#line default
#line hidden
#nullable disable
            WriteLiteral(" . Terem\r\n                            </td>\r\n");
#nullable restore
#line 35 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                             for (int hour = Model.OpeningHour; hour < Model.ClosingHour; ++hour)
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <td>\r\n");
#nullable restore
#line 38 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                       bool oldCell = false; 

#line default
#line hidden
#nullable disable
#nullable restore
#line 39 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                     if (Model.Index == 0) //ha a mai napot nézzük akkor lehetnek már nem foglalható időpontok
                                    {
                                        

#line default
#line hidden
#nullable disable
#nullable restore
#line 41 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                         if (hour < DateTime.Now.Hour || (hour == DateTime.Now.Hour && DateTime.Now.Minute > 0))
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <a class=\"disabled\">Foglalás</a>\r\n");
#nullable restore
#line 44 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                            oldCell = true;
                                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 47 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                         if (!oldCell && !Model.Reservations.Any())
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "708260af63eb3e05f9b82b1c8c328b84ba75d8d313609", async() => {
                WriteLiteral("Foglalás");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-roomId", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 49 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                                                WriteLiteral(Model.Rooms[i].Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["roomId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-roomId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["roomId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 49 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                                                                                    WriteLiteral(hour);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["hour"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-hour", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["hour"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 50 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                        }
                                        else
                                        {
                                            bool isConflict = false;
                                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 54 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                             foreach (var reservation in Model.Reservations)
                                            {
                                                DateTime dateNow = DateTime.Now;
                                                DateTime dateToCompare = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, hour, 0, 0);
                                                if (!oldCell && reservation.RehearsalRoomId == Model.Rooms[i].Id && reservation.IsConflicting(dateToCompare.AddMinutes(1), dateToCompare.AddMinutes(59)))
                                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                    <p>Foglalt</p>\r\n");
#nullable restore
#line 61 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                    isConflict = true;
                                                    break;
                                                }
                                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 65 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                             if (!oldCell && !isConflict)
                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "708260af63eb3e05f9b82b1c8c328b84ba75d8d319278", async() => {
                WriteLiteral("Foglalás");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-roomId", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 67 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                                                    WriteLiteral(Model.Rooms[i].Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["roomId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-roomId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["roomId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 67 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                                                                                        WriteLiteral(hour);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["hour"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-hour", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["hour"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 68 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 68 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                             
                                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 69 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                         
                                    }
                                    else //ha nem máról van szó
                                    {
                                        

#line default
#line hidden
#nullable disable
#nullable restore
#line 73 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                         if (!Model.Reservations.Any())
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "708260af63eb3e05f9b82b1c8c328b84ba75d8d324112", async() => {
                WriteLiteral("Foglalás");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-roomId", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 75 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                                                WriteLiteral(Model.Rooms[i].Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["roomId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-roomId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["roomId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 75 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                                                                                    WriteLiteral(hour);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["hour"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-hour", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["hour"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 76 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                        }
                                        else
                                        {
                                            bool isConflict = false;
                                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 80 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                             foreach (var reservation in Model.Reservations)
                                            {
                                                DateTime dateOfCurrentDay = DateTime.Now.AddDays(Model.Index);
                                                DateTime dateToCompare = new DateTime(dateOfCurrentDay.Year, dateOfCurrentDay.Month, dateOfCurrentDay.Day, hour, 0, 0);
                                                if (reservation.RehearsalRoomId == Model.Rooms[i].Id && reservation.IsConflicting(dateToCompare.AddMinutes(1), dateToCompare.AddMinutes(59)))
                                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                    <p>Foglalt</p>\r\n");
#nullable restore
#line 87 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                    isConflict = true;
                                                    break;
                                                }
                                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 91 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                             if (!isConflict)
                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "708260af63eb3e05f9b82b1c8c328b84ba75d8d329814", async() => {
                WriteLiteral("Foglalás");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-roomId", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 93 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                                                    WriteLiteral(Model.Rooms[i].Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["roomId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-roomId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["roomId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 93 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                                                                                        WriteLiteral(hour);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["hour"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-hour", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["hour"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 94 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 94 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                             
                                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 95 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                         
                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                </td>\r\n");
#nullable restore
#line 98 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </tr>\r\n");
#nullable restore
#line 100 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </table>\r\n            </div>\r\n        </td>\r\n        <td>\r\n");
#nullable restore
#line 105 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
             if (Model.Index < 10) //csak a következő 10 napra előre lehet foglalni
            {
                ReservationTableViewModel vm = new ReservationTableViewModel()
                {
                    ClosingHour = Model.ClosingHour,
                    OpeningHour = Model.OpeningHour,
                    Studio = Model.Studio,
                    NumberOfAvailableRooms = Model.NumberOfAvailableRooms,
                    Index = Model.Index,
                    Reservations = Model.Reservations,
                    Rooms = Model.Rooms
                };

#line default
#line hidden
#nullable disable
            WriteLiteral("                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "708260af63eb3e05f9b82b1c8c328b84ba75d8d335692", async() => {
                WriteLiteral("Következő nap");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-studioId", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 117 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                            WriteLiteral(Model.Studio.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["studioId"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-studioId", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["studioId"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 117 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
                                                                                                                                WriteLiteral(true);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["isNext"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-isNext", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["isNext"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 118 "D:\Documents\EGYETEM\szakdolgozat\bootstrap-proba\BandManager\EasyRehearsalManager\Views\Shared\Components\ReservationsTable\Default.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </td>\r\n    </tr>\r\n</table>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ReservationTableViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
