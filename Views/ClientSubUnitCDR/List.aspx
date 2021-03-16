<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitCDRsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Sub Units - CDR Link</div></div>
    <div id="content">
		<% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm("Create", null, FormMethod.Get, new { id = "form0" })) {%>
            <%= Html.ValidationSummary(true) %>

		   <table width="100%" cellpadding="0" cellspacing="0">
				<tr style="background-color:#ffffff;"> 
					<% if(Model.ClientDefinedReferences.Count == 0 ){%>
						<td width="20%">CDR Values linked to </td> 
						<td width="80%">
							<%=Html.TextBoxFor(model => model.DisplayName, new { placeholder = "Display Name of CDR" })%>
							<%=Html.ValidationMessageFor(model => model.DisplayName)%>
						</td> 
					<%} else { %>
						<td width="100%" colspan="2">CDR Values linked to <%=Model.DisplayName%></td> 
						<%= Html.HiddenFor(model => model.ClientDefinedReferenceItemId) %>
						<%= Html.Hidden("DisplayName", Model.ClientDefinedReferences[0].DisplayName ) %>
					<% } %>
				</tr>
			   <tr style="background-color:#ffffff;">
					<% if(string.IsNullOrEmpty(Model.RelatedToDisplayName)){%>
						<td width="20%">Validate CDR </td> 
						<td width="80%">
							<%=Html.TextBoxFor(model => model.RelatedToDisplayName, new { placeholder = "Display Name of CDR" })%>
							<%=Html.ValidationMessageFor(model => model.RelatedToDisplayName)%>
						</td> 
					<%} else { %>
						<td width="100%" colspan="2">Validate CDR <%=Model.RelatedToDisplayName%></td> 
						<%= Html.HiddenFor(model => model.ClientSubUnitClientDefinedReferenceItemId) %>
						<%= Html.Hidden("RelatedToDisplayName", Model.RelatedToDisplayName) %>
					<% } %>

					<%= Html.Hidden("ClientSubUnitGuid", Model.ClientSubUnit.ClientSubUnitGuid) %>
				</tr>
		   </table>

            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
                    <th width="9%" class="row_header_left"><%= Html.RouteLink("CDR Value", "ListMain", new { page = 1, sortField = "Value", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id= Model.ClientSubUnit.ClientSubUnitGuid})%></th> 
                    <th width="18%"><%= Html.RouteLink("Account Name", "ListMain", new { page = 1, sortField = "ClientAccountName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id= Model.ClientSubUnit.ClientSubUnitGuid })%></th> 
					<th width="9%"><%= Html.RouteLink("Account", "ListMain", new { page = 1, sortField = "ClientAccountNumber", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id= Model.ClientSubUnit.ClientSubUnitGuid })%></th> 
                    <th width="14%"><%= Html.RouteLink("Credit Card", "ListMain", new { page = 1, sortField = "MaskedCreditCardNumber", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id= Model.ClientSubUnit.ClientSubUnitGuid })%></th> 
                    <th width="14%"><%= Html.RouteLink("Card Holder", "ListMain", new { page = 1, sortField = "CreditCardHolderName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id= Model.ClientSubUnit.ClientSubUnitGuid })%></th> 
                    <th width="8%"><%= Html.RouteLink("Valid To", "ListMain", new { page = 1, sortField = "CreditCardValidTo", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id= Model.ClientSubUnit.ClientSubUnitGuid })%></th> 
				    <th width="8%"><%= Html.RouteLink("Validate Values?", "ListMain", new { page = 1, sortField = "ValidateValuesFlag", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], id= Model.ClientSubUnit.ClientSubUnitGuid })%></th> 
					<th width="5%">&nbsp;</th> 
					<th width="5%">&nbsp;</th> 
                    <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 	
		        </tr> 
                <%
                    foreach (var item in Model.ClientDefinedReferences)
                    { 
                %>
                <tr>
                    <td><%= Html.Encode(item.Value)%></td>
                    <td><%= Html.Encode(item.ClientAccountName)%></td>
					<td><%= Html.Encode(item.ClientAccountNumber)%></td>
                    <td><%= Html.Encode(item.MaskedCreditCardNumber)%></td>
					<td><%= Html.Encode(item.CreditCardHolderName)%></td>
                    <td><%if(item.CreditCardValidTo.HasValue){%><%  =Html.Encode(item.CreditCardValidTo.Value.ToString("MMM yyyy"))%><%}%></td>
					<td data-validate-values="<%= Html.Encode(item.ValidateValues)%>"><%if(Int32.Parse(item.ValidateValues.Value.ToString()) != 0) { %>Yes<%}else{%>No<%}%></td>
					<td>
						<%= Html.RouteLink(
							"Validate Values", 
							"Main", 
							new {
								controller="ClientSubUnitCDRItem", 
								action = "List", 
								id = item.ClientSubUnitClientDefinedReferenceId, 
								csu = Model.ClientSubUnit.ClientSubUnitGuid,
								relatedToDisplayName = Model.RelatedToDisplayName
							}, new { 
								@class="ValidateLinkActive", 
								title = "Validate Values",
								data_controller="ClientSubUnitCDRItem", 
								data_action = "List", 
								data_id = item.ClientSubUnitClientDefinedReferenceId, 
								data_csu = Model.ClientSubUnit.ClientSubUnitGuid
							}
						)%>
						<% if(string.IsNullOrEmpty(Model.RelatedToDisplayName)){%><span class="ValidateLinkInactive">Validate Values</span><%} %>
					</td>
                    <td><%= Html.RouteLink("View", "Main", new { action = "View", id = item.ClientSubUnitClientDefinedReferenceId }, new { title = "View" })%></td>
                    <td>
						<%if (Model.HasWriteAccess){ %>
							<%=  Html.RouteLink("Edit", "Main", new { action = "Edit", id = item.ClientSubUnitClientDefinedReferenceId }, new { title = "Edit" })%>
						<%} %>
					</td>
					<td>
						<%if (Model.HasWriteAccess){ %>
							<%=  Html.RouteLink("Delete", "Main", new { action = "Delete", id = item.ClientSubUnitClientDefinedReferenceId }, new { title = "Delete" })%>
						<%} %>
					</td>     
                </tr>        
                <% 
                } 
                %>
                 <tr>
                <td colspan="11" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                          <% if (Model.ClientDefinedReferences.HasPreviousPage)
                             { %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid, page = (Model.ClientDefinedReferences.PageIndex - 1), sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                         <% if (Model.ClientDefinedReferences.HasNextPage)
                            { %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid, page = (Model.ClientDefinedReferences.PageIndex + 1), sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"] }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.ClientDefinedReferences.TotalPages > 0)
                                                     { %>Page <%=Model.ClientDefinedReferences.PageIndex%> of <%=Model.ClientDefinedReferences.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="10"><%if (Model.HasCDRLinkImportAccess)
                                                            { %><input type="submit" name="btnSubmit" value="Import" class="red" title="Import"/>&nbsp;<%}%><%if (Model.HasWriteAccess)
                                                            { %><input type="submit" name="btnSubmit" value="Create CDR Link" class="red" title="Create CDR Link"/><%}%></td> 
		        </tr> 
            </table>
            <% } %>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();

        //Search
        $('#frmSearch').submit(function () {
            if ($('#filter').val() == "") {
                alert("No Search Text Entered");
                return false;
            }
            $('<input>').attr({ type: 'hidden', id: 'id', name: 'id', value: '<%=Model.ClientSubUnit.ClientSubUnitGuid%>' }).appendTo('#frmSearch');
            $("#frmSearch").attr("action", "/ClientSubUnitCDR.mvc/List");

        });

    	$('#btnSearch').click(function () {
            $("#frmSearch").submit();

    	});

    	//RelatedToDisplayName

    	if ($('#RelatedToDisplayName').val() == '') {
    		$('.ValidateLinkActive').hide();
    		$('.ValidateLinkInactive').show();
    	}

    	$('#RelatedToDisplayName').live('input propertychange paste', function() {
    		var value = $(this).val().replace(/^\s+|\s+$/gm, '');
    		if (value != '') {
    			$('.ValidateLinkActive').show();
    			$('.ValidateLinkInactive').hide();
    			$('.ValidateLinkActive').each(function() {
    				$(this).attr('href', '/ClientSubUnitCDRItem.mvc/List?id=' + $(this).attr('data-id') + '&csu=' + $(this).attr('data-csu') + '&relatedToDisplayName=' + value);
    			});
    		} else {
    			$('.ValidateLinkActive').hide();
    			$('.ValidateLinkInactive').show();
    			$('.ValidateLinkActive').each(function () {
    				$(this).attr('href', '/ClientSubUnitCDRItem.mvc/List?id=' + $(this).attr('data-id') + '&csu=' + $(this).attr('data-csu'));
    			});
    		}
    	});
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
CDR Links
</asp:Content>
