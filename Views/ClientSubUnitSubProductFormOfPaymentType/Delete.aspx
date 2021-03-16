<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitSubProductFormOfPaymentTypeVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - Client Detail SubProduct FormOfPaymentTypes</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		          <tr> 
			        <th class="row_header" colspan="3">Delete SubProduct FormOfPaymentType</th> 
		        </tr> 
                 <tr>
                    <td>Sub Product</td>
                    <td><%= Html.Encode(Model.ClientDetailSubProductFormOfPaymentType.SubProductName)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Form Of Payment Type</td>
                    <td><%= Html.Encode(Model.ClientDetailSubProductFormOfPaymentType.FormOfPaymentTypeDescription)%></td>
                    <td></td>
                 </tr>
                
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.ClientDetailSubProductFormOfPaymentType.SubProductId) %>
                        <%= Html.HiddenFor(model => model.ClientDetailSubProductFormOfPaymentType.VersionNumber)%>
                    <%}%>
                    </td>                
               </tr>
            </table>       
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");


    })
 </script>
</asp:Content>

