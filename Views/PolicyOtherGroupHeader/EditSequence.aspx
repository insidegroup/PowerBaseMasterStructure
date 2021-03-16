<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderSequenceVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Other Group Headers
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Other Group Headers - Edit Order</div></div>
        <div id="content">
         <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
            <table width="100%" cellpadding="0" cellspacing="0" id="sortable">
                <tr> 
                	
			        <th width="10%" class="row_header_left">Order</th> 
			        <th width="20%">Service Type</th> 
			        <th width="20%">Product</th> 
					<th width="20%">Sub Product</th> 
					<th width="25%">Label</th> 
                    <th width="5%" class="row_header_right">&nbsp;</th>
		        </tr> 
		       
                  <tbody class="content">  
		               
                <%
                var iCounter = 0;
                foreach (var item in Model.PolicyOtherGroupHeaderSequences)
                {
                    iCounter++;
                   
                %>
                <tr width="100%" id="<%= Html.Encode(item.PolicyOtherGroupHeaderId)%>_<%= Html.Encode(item.VersionNumber) %>">
                    <td><%:item.DisplayOrder%></td>
                    <td><%:item.PolicyOtherGroupHeaderServiceTypeDescription%></td>
					<td><%= Html.Encode(item.ProductName)%></td>
                    <td><%= Html.Encode(item.SubProductName)%></td>
					<td><%= Html.Encode(item.Label)%></td>
                    <td><img src="../../images/arrow.png" alt=""/></td>
                </tr>
				<% 
                } 
				
                %>

                
       </tbody>
                <tr>
                    <td colspan="6" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                         <% if (Model.PolicyOtherGroupHeaderSequences.HasPreviousPage)
                            { %>
                            <%= Html.RouteLink("<<Previous Page", "Default", new { page = (Model.PolicyOtherGroupHeaderSequences.PageIndex - 1), PolicyOtherGroupHeaderServiceTypeId = Model.PolicyOtherGroupHeaderServiceTypeId, ProductId = Model.ProductId, SubProductId = Model.SubProductId }, new { onclick = "return checkSaved();" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                        <% if (Model.PolicyOtherGroupHeaderSequences.HasNextPage)
                           {  %>
                            <%= Html.RouteLink("Next Page>>>", "Default", new { page = (Model.PolicyOtherGroupHeaderSequences.PageIndex + 1), PolicyOtherGroupHeaderServiceTypeId = Model.PolicyOtherGroupHeaderServiceTypeId, ProductId = Model.ProductId, SubProductId = Model.SubProductId }, new { onclick = "return checkSaved();" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.PolicyOtherGroupHeaderSequences.TotalPages > 0)
                                                     { %>Page <%=Model.PolicyOtherGroupHeaderSequences.PageIndex%> of <%=Model.PolicyOtherGroupHeaderSequences.TotalPages%><%} %></div>
                    </div>
                    </td>
                </tr>
         
		        <tr> 
                 <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                 <td class="row_footer_blank_right" colspan="5">
					 <%= Html.RouteLink("Cancel", new { controller = "PolicyOtherGroupHeader", action = "List" }, new { @class = "red" })%>&nbsp;
					 <input type="submit" value="Save" title="Save" class="red"/></td>
		        </tr> 
		        
            </table>
            <input type="hidden" name="SequenceOriginal" id="SequenceOriginal" />
            <input type="hidden" name="Sequence" id="Sequence" />
            <input type="hidden" name="page" id="page" value="<%=ViewData["Page"] %>" />
             <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/Sequencing/PolicyOtherGroupHeader.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
Policy Other Group Headers &gt;
<%=Html.Encode(Model.PolicyOtherGroupHeaderServiceType.PolicyOtherGroupHeaderServiceTypeDescription) %> <%=Html.Encode(Model.Product.ProductName)%> <%=Html.Encode(Model.SubProduct != null ? Model.SubProduct.SubProductName : "") %>
</asp:Content>