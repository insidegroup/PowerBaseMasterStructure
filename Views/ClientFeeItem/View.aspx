<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientFeeItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
<div id="banner"><div id="banner_text">Client Fee Item</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View Client Fee Item</th> 
		    </tr> 
            <tr>
                <td>Fee Type</td>
                <td><%=ViewData["FeeTypeDescription"]%></td>
                <td></td>
            </tr>
            <tr>
                <td><%=ViewData["FeeTypeDescription"]%></td>
                <td><%= Html.Encode(Model.ClientFee.ClientFeeDescription)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Value Amount</td>
                <td><%= Html.Encode(Model.ValueAmount)%></td>
                <td></td>
            </tr>       
            <tr>
                <td>Value Percentage</td>
                <td><%= Html.Encode(Model.ValuePercentage)%></td>
                <td></td>
            </tr>   
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                <td class="row_footer_blank_right"></td>
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
<%=Html.RouteLink(ViewData["FeeTypeDescription"] + " Groups", "Main", new { controller = "ClientFeeGroup", action = "ListUnDeleted", ft = Model.ClientFee.FeeTypeId }, new { title = ViewData["FeeTypeDescription"] })%> &gt;
<%=Html.RouteLink(ViewData["ClientFeeGroupName"].ToString(), "Main", new { controller = "ClientFeeGroup", action = "View", id = ViewData["ClientFeeGroupId"].ToString() }, new { title = ViewData["ClientFeeGroupName"].ToString() })%> &gt;
Client Fee Items
</asp:Content>

