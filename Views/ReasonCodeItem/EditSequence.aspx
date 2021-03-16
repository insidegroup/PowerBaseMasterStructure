<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ReasonCodeGroupReasonCodeTypeSequencingVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Reason Codes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Reason Code Item Ordering</div></div>
        <div id="content">
         <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table width="100%" cellpadding="0" cellspacing="0" id="sortable">
                <tr> 
                	
			        <th width="10%" class="row_header_left">Display Order</th> 
			        <th width="17%">Reason Code</th> 
			        <th width="63%">Reason Code Type</th> 
                    <th width="10%" class="row_header_right">&nbsp;</th>
		        </tr> 
		       
                  <tbody class="content">  
                <%
                var iCounter = 0;
                foreach (var item in Model.SequenceItems) {
                    iCounter++;
                   
                %>
                  <tr width="100%" id="<%= Html.Encode(item.ReasonCodeItemId)%>_<%= Html.Encode(item.VersionNumber) %>">
                    <td width="15%"><%= Html.Encode(item.DisplayOrder) %></td>
                    <td width="17%"><%= Html.Encode(item.ReasonCode) %></td>
                    <td width="58%"><%= Html.Encode(item.ReasonCodeTypeDescription) %></td>
                    <td width="10%"><img src="../../images/arrow.png" alt=""/></td>
                </tr>
				<% 
                } 
                %>
                </tbody>
                <tr>
                    <td colspan="4" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.SequenceItems.HasPreviousPage){ %>
                            <%= Html.RouteLink("<<Previous Page", "Default", new { page = (Model.SequenceItems.PageIndex - 1), id = Model.ReasonCodeGroup.ReasonCodeGroupId, reasonCodeTypeId = Model.ReasonCodeTypeId }, new { onclick = "return checkSaved();" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.SequenceItems.HasNextPage){  %>
                            <%= Html.RouteLink("Next Page>>>", "Default", new { page = (Model.SequenceItems.PageIndex + 1), id = Model.ReasonCodeGroup.ReasonCodeGroupId, reasonCodeTypeId = Model.ReasonCodeTypeId }, new { onclick = "return checkSaved();" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.SequenceItems.TotalPages > 0){ %>Page <%=Model.SequenceItems.PageIndex%> of <%=Model.SequenceItems.TotalPages%><%} %></div>
                    </div>
                    </td>
                </tr>
         
		        <tr> 
                 <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                 <td class="row_footer_blank_right" colspan="2"><%= Html.ActionLink("Reset", "EditSequence", new { id = ViewData["PolicyGroupId"] }, new { @class = "red" })%>&nbsp;<input type="submit" value="Save" title="Save" class="red"/></td>
		        </tr> 
		        
            </table>
           <input type="hidden" name="RCSequenceOriginal" id="RCSequenceOriginal" />
            <input type="hidden" name="RCSequence" id="RCSequence" />
            <input type="hidden" name="id" id="id" value="<%=Model.ReasonCodeGroup.ReasonCodeGroupId %>" />
            <%= Html.HiddenFor(model => model.ReasonCodeGroup.ReasonCodeGroupId) %>
            <input type="hidden" name="reasonCodeTypeId" id="reasonCodeTypeId" value="<%=Model.ReasonCodeTypeId %>" />
            <input type="hidden" name="page" id="page" value="<%=Model.Page%>" />
             <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/Sequencing/ReasonCodeItem.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Reason Code Groups", "Main", new { controller = "ReasonCodeGroup", action = "ListUnDeleted", }, new { title = "Reason Code Groups" })%> &gt;
<%=Html.RouteLink(Model.ReasonCodeGroup.ReasonCodeGroupName, "Default", new { controller = "ReasonCodeGroup", action = "View", id = Model.ReasonCodeGroup.ReasonCodeGroupId }, new { title = Model.ReasonCodeGroup.ReasonCodeGroupName})%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "ReasonCodeItem", action = "List", id = Model.ReasonCodeGroup.ReasonCodeGroupId }, new { title = "Reason Code Items"})%> &gt;
Ordering
</asp:Content>