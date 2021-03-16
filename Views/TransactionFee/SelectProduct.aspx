<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TransactionFeeProductSelectListVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Mid Office Transaction Fees</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(true); %>
       <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
       <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Mid Office Transaction Fee</th> 
		        </tr>                 
                <tr>
                    <td><label for="ProductId">Product Type</label></td>
                   <td><%= Html.DropDownListFor(model => model.ProductId, Model.Products, "Please Select...")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductId)%></td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right"><input type="submit" value="Continue" title="Continue" class="red"/></td>
                </tr>
             </table>
    <% } %>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clientfeegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
 

</asp:Content>