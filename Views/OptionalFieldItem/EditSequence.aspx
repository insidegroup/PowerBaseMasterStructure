<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.OptionalFieldItemSequencesVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Optional Field Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Optional Field Items - Ordering</div></div>
        <div id="content">
         <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
            <table width="100%" cellpadding="0" cellspacing="0" id="sortable">
                <tr> 
			        <th width="10%" class="row_header_left">Order</th> 
			        <th width="50%">Optional Field</th> 
			        <th width="35%">Supplier</th> 
                    <th width="5%" class="row_header_right">&nbsp;</th>
		        </tr> 
		       
                  <tbody class="content">  
		               
                <%
                var iCounter = 0;
                foreach (var item in Model.OptionalFieldItemSequences)
                {
                    iCounter++;
                   
                %>
                <tr width="100%" id="<%= Html.Encode(item.OptionalFieldItemId)%>_<%= Html.Encode(item.VersionNumber) %>">
                    <td width="10%"><%:item.OptionalFieldDisplayOrder%></td>
                    <td width="50%"><%= Html.Encode(item.OptionalFieldName)%></td>
                    <td width="35%"><%= Html.Encode(item.SupplierName)%></td>
                    <td width="5%"><img src="../../images/arrow.png" alt=""/></td>
                </tr>
				<% 
                } 
				
                %>

                
       </tbody>
                <tr>
                    <td colspan="4" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.OptionalFieldItemSequences.HasPreviousPage)
                            { %>
                            <%= Html.RouteLink("<<Previous Page", "Default", new { page = (Model.OptionalFieldItemSequences.PageIndex - 1), groupid = Model.OptionalFieldGroup.OptionalFieldGroupId, id = Model.OptionalFieldId }, new { onclick = "return checkSaved();" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.OptionalFieldItemSequences.HasNextPage)
                           {  %>
                            <%= Html.RouteLink("Next Page>>>", "Default", new { page = (Model.OptionalFieldItemSequences.PageIndex + 1), groupid = Model.OptionalFieldGroup.OptionalFieldGroupId, id = Model.OptionalFieldId }, new { onclick = "return checkSaved();" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.OptionalFieldItemSequences.TotalPages > 0)
                                                     { %>Page <%=Model.OptionalFieldItemSequences.PageIndex%> of <%=Model.OptionalFieldItemSequences.TotalPages%><%} %></div>
                    </div>
                    </td>
                </tr>
         
		        <tr> 
                 <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                 <td class="row_footer_blank_right" colspan="3"><%= Html.RouteLink("Cancel", "Default", new { action="List", id = Model.OptionalFieldGroup.OptionalFieldGroupId }, new { @class = "red" })%>&nbsp;<input type="submit" value="Save" title="Save" class="red"/></td>
		        </tr> 
		        
            </table>
            <input type="hidden" name="SequenceOriginal" id="SequenceOriginal" />
            <input type="hidden" name="Sequence" id="Sequence" />
            <input type="hidden" name="id" id="id" value="<%=Model.OptionalFieldId%>" />
            <input type="hidden" name="groupid" id="groupid" value="<%=Model.OptionalFieldGroup.OptionalFieldGroupId%>" />
            <input type="hidden" name="page" id="page" value="<%=ViewData["Page"] %>" />
             <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/Sequencing/OptionalFieldItem.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Optional Field Groups", "Main", new { controller = "OptionalFieldGroup", action = "ListUnDeleted", }, new { title = "Optional Field Groups" })%> &gt;
<%=Html.RouteLink(Model.OptionalFieldGroup.OptionalFieldGroupName, "Default", new { controller = "OptionalFieldGroup", action = "View", id = Model.OptionalFieldGroup.OptionalFieldGroupId }, new { title = Model.OptionalFieldGroup.OptionalFieldGroupName })%> &gt;
<%=Html.RouteLink("Items", "Main", new { controller = "OptionalFieldItem", action = "List", id = Model.OptionalFieldGroup.OptionalFieldGroupId })%> &gt;
Ordering
</asp:Content>