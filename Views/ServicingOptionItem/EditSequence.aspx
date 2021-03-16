<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ServicingOptionItemSequencesVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Servicing Option Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Servicing Option Items - Ordering</div></div>
        <div id="content">
         <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
            <table width="100%" cellpadding="0" cellspacing="0" id="sortable">
                <tr> 
                	
			        <th width="10%" class="row_header_left">Order</th> 
			        <th width="50%">Value</th> 
			        <th width="35%">GDS</th> 
                    <th width="5%" class="row_header_right">&nbsp;</th>
		        </tr> 
		       
                  <tbody class="content">  
		               
                <%
                var iCounter = 0;
                foreach (var item in Model.ServicingOptionItemSequences)
                {
                    iCounter++;
                   
                %>
                <tr width="100%" id="<%= Html.Encode(item.ServicingOptionItemId)%>_<%= Html.Encode(item.VersionNumber) %>">
                     <td width="10%"><%:item.ServicingOptionItemSequence%></td>
                    <td width="50%"><%= Html.Encode(item.ServicingOptionItemValue)%></td>
                    <td width="35%"><%= Html.Encode(item.GDSName)%></td>
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
                         <% if (Model.ServicingOptionItemSequences.HasPreviousPage)
                            { %>
                            <%= Html.RouteLink("<<Previous Page", "Default", new { page = (Model.ServicingOptionItemSequences.PageIndex - 1), groupid = Model.ServicingOptionGroup.ServicingOptionGroupId, id = Model.ServicingOptionId }, new { onclick = "return checkSaved();" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.ServicingOptionItemSequences.HasNextPage)
                           {  %>
                            <%= Html.RouteLink("Next Page>>>", "Default", new { page = (Model.ServicingOptionItemSequences.PageIndex + 1), groupid = Model.ServicingOptionGroup.ServicingOptionGroupId, id = Model.ServicingOptionId }, new { onclick = "return checkSaved();" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.ServicingOptionItemSequences.TotalPages > 0)
                                                     { %>Page <%=Model.ServicingOptionItemSequences.PageIndex%> of <%=Model.ServicingOptionItemSequences.TotalPages%><%} %></div>
                    </div>
                    </td>
                </tr>
         
		        <tr> 
                 <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                 <td class="row_footer_blank_right" colspan="3"><%= Html.RouteLink("Cancel", "Default", new { action="List", id = Model.ServicingOptionGroup.ServicingOptionGroupId }, new { @class = "red" })%>&nbsp;<input type="submit" value="Save" title="Save" class="red"/></td>
		        </tr> 
		        
            </table>
            <input type="hidden" name="SequenceOriginal" id="SequenceOriginal" />
            <input type="hidden" name="Sequence" id="Sequence" />
            <input type="hidden" name="id" id="id" value="<%=Model.ServicingOptionId%>" />
            <input type="hidden" name="groupid" id="groupid" value="<%=Model.ServicingOptionGroup.ServicingOptionGroupId%>" />
            <input type="hidden" name="page" id="page" value="<%=ViewData["Page"] %>" />
             <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/Sequencing/ServicingOption.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Servicing Option Groups", "Main", new { controller = "ServicingOptionGroup", action = "ListUnDeleted", }, new { title = "Servicing Option Groups" })%> &gt;
<%=Html.RouteLink(Model.ServicingOptionGroup.ServicingOptionGroupName, "Default", new { controller = "ServicingOptionGroup", action = "View", id = Model.ServicingOptionGroup.ServicingOptionGroupId }, new { title = Model.ServicingOptionGroup.ServicingOptionGroupName })%> &gt;
<%=Html.RouteLink("Items", "Default", new { controller = "ServicingOptionItem", action = "List", id = Model.ServicingOptionGroup.ServicingOptionGroupId }, new { title = "Servicing Option Items" })%> &gt;
<%= Html.Encode(Model.ServicingOption.ServicingOptionName)%>
</asp:Content>