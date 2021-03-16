<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderColumnNameVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Column Names</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Column Name</div></div>
        <div id="content">
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Column Name</th> 
		        </tr> 
				<tr>
					<td><label for="PolicyOtherGroupHeaderColumnName_Label">Table Name</label></td>
					<td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeader.TableName)%></td>
				</tr>	
				<tr>
                    <td><label for="PolicyOtherGroupHeaderColumnName_ColumnName">Column Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeaderColumnName.ColumnName)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
					<td class="row_footer_blank_left" colspan="2">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
						<a href="javascript:window.print();" class="red" title="Print">Print</a>
					</td>                    
                    <td class="row_footer_blank_right">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Delete Column Name" title="Delete Column Name" class="red"/>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeaderColumnName.VersionNumber)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId)%>
							<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId)%>
						<%}%>
                    </td>
                </tr>
           </table>
        </div>
    </div>
   <script type="text/javascript">
   	$(document).ready(function () {
   		$("tr:odd").addClass("row_odd");
   		$("tr:even").addClass("row_even");
   		$('#menu_admin').click();
   		$('#search').hide();
   		$('#breadcrumb').css('width', 'auto');
   	});
	</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Other Group Header", "Main", new { controller = "PolicyOtherGroupHeader", action = "List", id = Model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId }, new { title = "Policy Other Group Header" })%> &gt;
<%=Html.Encode(Model.PolicyOtherGroupHeader.Label) %> &gt;
<%=Html.Encode(Model.PolicyOtherGroupHeader.TableName) %> &gt;
Delete
</asp:Content>