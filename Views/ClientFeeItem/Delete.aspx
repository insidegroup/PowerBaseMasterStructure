<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeItemVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">Client Fee Items</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			     <th class="row_header" colspan="3">Delete Client Fee Item</th> 
		    </tr> 
             <tr>
                <td>Fee Type</td>
                <td><%= Html.Encode(Model.FeeType.FeeTypeDescription)%></td>
                <td></td>
            </tr>
            <tr>
                <td><%= Html.Encode(Model.FeeType.FeeTypeDescription)%></td>
                <td><%= Html.Encode(Model.ClientFeeItem.ClientFee.ClientFeeDescription)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Value Amount</td>
                <td><%= Html.Encode(Model.ClientFeeItem.ValuePercentage)%></td>
                <td></td>
            </tr>       
            <tr>
                <td>Value Percentage</td>
                <td><%= Html.Encode(Model.ClientFeeItem.ValuePercentage)%></td>
                <td></td>
            </tr>   
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>                    
                    <td class="row_footer_blank_right">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.ClientFeeItem.VersionNumber)%>
                        <%= Html.HiddenFor(model => model.ClientFeeItem.ClientFeeItemId)%>
                    <%}%>
                    </td>  
                </tr>
        </table>
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
  <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink(Model.FeeType.FeeTypeDescription + " Groups", "Main", new { controller="ClientFeeGroup", action = "ListUnDeleted", ft = Model.FeeType.FeeTypeId }, new { title = Model.FeeType.FeeTypeDescription })%> &gt;
<%=Html.RouteLink(Model.ClientFeeItem.ClientFeeGroup.ClientFeeGroupName, "Main", new { controller = "ClientFeeGroup", action = "View", id = Model.ClientFeeItem.ClientFeeGroup.ClientFeeGroupId }, new { title = Model.ClientFeeItem.ClientFeeGroup.ClientFeeGroupName })%> &gt;
Client Fee Items
</asp:Content>





