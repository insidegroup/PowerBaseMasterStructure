<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderColumnNameVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Column Names</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text">Column Names</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <% using(Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Column Name</th> 
		        </tr> 
				<tr>
                    <td><label for="PolicyOtherGroupHeaderColumnName_Label">Table Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.PolicyOtherGroupHeaderTableName.TableName)%></td>
                </tr>	
				<tr>
                    <td><label for="PolicyOtherGroupHeaderColumnName_ColumnName">Column Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.PolicyOtherGroupHeaderColumnName.ColumnName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeaderColumnName.ColumnName)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Column Name" class="red" title="Create Column Name"/></td>
                </tr>
            </table>
			<%= Html.HiddenFor(model => model.PolicyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId)%>
			<%= Html.HiddenFor(model => model.PolicyOtherGroupHeaderTableName.PolicyOtherGroupHeaderTableNameId)%>
			<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId)%>
			<%= Html.HiddenFor(model => model.PolicyOtherGroupHeader.Label)%>
		<% } %>
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
Create
</asp:Content>