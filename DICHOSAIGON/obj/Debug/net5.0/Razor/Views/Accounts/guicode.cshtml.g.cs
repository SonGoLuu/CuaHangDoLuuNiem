#pragma checksum "D:\ASP.NET CORE LEARNING\DICHOSAIGON\DICHOSAIGON\Views\Accounts\guicode.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c19776182fbf26a2df5d802d2b17bc7dec1cc1df"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Accounts_guicode), @"mvc.1.0.view", @"/Views/Accounts/guicode.cshtml")]
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
#line 1 "D:\ASP.NET CORE LEARNING\DICHOSAIGON\DICHOSAIGON\Views\_ViewImports.cshtml"
using DICHOSAIGON;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\ASP.NET CORE LEARNING\DICHOSAIGON\DICHOSAIGON\Views\_ViewImports.cshtml"
using DICHOSAIGON.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c19776182fbf26a2df5d802d2b17bc7dec1cc1df", @"/Views/Accounts/guicode.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"77de991fb4e3f5cbdb6fd7d9a1bf723ed3bf9c44", @"/Views/_ViewImports.cshtml")]
    public class Views_Accounts_guicode : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\ASP.NET CORE LEARNING\DICHOSAIGON\DICHOSAIGON\Views\Accounts\guicode.cshtml"
  
    ViewData["Title"] = "guicode";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<div class=""login-register-area section-space-y-axis-100"">
    <div class=""container"">
        <div class=""row"">
            <div class=""col-lg-12 pt-10 pt-lg-0"">
                Chúng tôi đã gửi mã kích hoạt tài khoản về số điện thoại của bạn, vui lòng nhập mã xác nhận.
                <br />
                <br />
                Nhập mã xác nhận:
                <input type=""text"" id=""myText""");
            BeginWriteAttribute("value", " value=\"", 496, "\"", 504, 0);
            EndWriteAttribute();
            WriteLiteral(@">
                <button onclick=""myFunction()"">Xác nhận</button>
                <br />
                <p id=""demo""></p>

            </div>
        </div>
    </div>
</div>
<script>
    function myFunction() {
      var x = document.getElementById(""myText"").value;
      if(x == ");
#nullable restore
#line 25 "D:\ASP.NET CORE LEARNING\DICHOSAIGON\DICHOSAIGON\Views\Accounts\guicode.cshtml"
         Write(ViewBag.Code);

#line default
#line hidden
#nullable disable
            WriteLiteral(")\r\n      {\r\n          window.location=\"/tai-khoan-cua-toi.html\";\r\n      }\r\n      else\r\n      {\r\n          document.getElementById(\"demo\").innerHTML = \"Bạn nhập sai mã xác nhận, vui lòng thử lại.\";\r\n      }\r\n    }\r\n</script>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591