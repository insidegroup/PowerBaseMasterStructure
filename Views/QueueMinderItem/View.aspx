<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.QueueMinderItem>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Queues</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Follow Up Queue Items</div></div>
    <div id="content">
        <table cellpadding="0" cellspacing="0" width="100%"> 
		    <tr> 
			    <th class="row_header" colspan="3">View Follow Up Queue Item</th> 
		    </tr> 
            <tr>
                <td>Description</td>
                <td><%= Html.Encode(Model.QueueMinderItemDescription)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Type</td>
                <td><%= Html.Encode(Model.QueueMinderType.QueueMinderTypeDescription)%></td>
                <td></td>
            </tr>  
             <tr>
                <td>Pseudo City Or Office Id</td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeId)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Queue Number</td>
                <td><%= Html.Encode(Model.QueueNumber)%></td>
                <td></td>
            </tr> 
             <tr>
                <td>Queue Category</td>
                <td><%= Html.Encode(Model.QueueCategory)%></td>
                <td></td>
            </tr>   
             <tr>
                <td>Queue PrefactoryCode</td>
                <td><%= Html.Encode(Model.PrefactoryCode)%></td>
                <td></td>
            </tr>   
             <tr>
                <td>GDS</td>
                <td><%= Html.Encode(Model.GDS.GDSName)%></td>
                <td></td>
            </tr>   
             <tr>
                <td>Context</td>
                <td><%= Html.Encode(Model.Context.ContextName)%></td>
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
        $('#menu_ticketqueuegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Follow Up Queue Groups", "Main", new { controller = "FollowUpQueueGroup", action = "ListUnDeleted", }, new { title = "Follow Up Queue Groups" })%> &gt;
<%=Html.RouteLink(Model.QueueMinderGroup.QueueMinderGroupName, "Default", new { controller = "FollowUpQueueGroup", action = "View", id = Model.QueueMinderGroup.QueueMinderGroupId }, new { title = Model.QueueMinderGroup.QueueMinderGroupName })%> &gt;
<%=Html.RouteLink("Queue Items", "Default", new { controller = "QueueMinderItem", action = "List", id = Model.QueueMinderGroupId }, new { title = "Queue Items" })%>
</asp:Content>
