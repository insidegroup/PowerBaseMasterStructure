<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.ClientSubUnitReasonCodesVM>" %>


<div></div>
<% using (Html.BeginForm())
   {%>


<div style="float: left;"><span id="clientReasonCodesSaveButton"><small>Save</small></span>&nbsp;<span id="clientReasonCodesCancelButton"><small>Cancel</small> </span></div>
<div style="float: right;"><span id="clientReasonCodesBackButton"><small><< Back</small></span>&nbsp;<span id="clientReasonCodesNextButton"><small>Next >></small> </span></div>

        
 <div id="divSearch">
     	<table class="" cellspacing="0" width="100%">
        <thead>
			<tr>
				<th>&nbsp;</th>
                </tr>
                </thead>
        </table>
     </div>
     <% } %>
<table class="tablesorter_other" cellspacing="0" width="100%">
        <tr>
            <td>
			<% if (Model.ClientSubUnitReasonCodeProductGroup != null) {
				foreach (var reasonCodeGroup in Model.ClientSubUnitReasonCodeProductGroup) {
					if (reasonCodeGroup.Product != null && reasonCodeGroup.ReasonCodeType != null) { %>
						<table id="currentReasonCode<%: reasonCodeGroup.Product.ProductName.Replace(" ", "") %><%: reasonCodeGroup.ReasonCodeType.ReasonCodeTypeDescription.Replace(" ", "") %>ItemsTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
                <thead>
                     <tr>
									<th colspan="5"><% =Html.Raw(!string.IsNullOrEmpty(reasonCodeGroup.ReasonCodeType.ReasonCodeTypeName) ? 
													reasonCodeGroup.ReasonCodeType.ReasonCodeTypeName : 
													reasonCodeGroup.Product.ProductName + " - " + reasonCodeGroup.ReasonCodeType.ReasonCodeTypeDescription) %></th>
									<th align="right">
										<%if (Model.ReasonCodeGroupWriteAccess) {

                var rcGroupId = "";
                if(reasonCodeGroup.ReasonCodeProductGroups !=null){
                    if (reasonCodeGroup.ReasonCodeProductGroups.Count > 0)
                    {
                        rcGroupId = reasonCodeGroup.ReasonCodeProductGroups[0].ReasonCodeGroupId.ToString();
                    }
                }
										//groupId, productId, reasonCodeTypeId, product, reasonCodeType, source%>
										<a href="javascript:AddProductReasonCodeType(
															'<%: rcGroupId %>', 
															'<%: reasonCodeGroup.Product.ProductId %>', 
															'<%: reasonCodeGroup.ReasonCodeType.ReasonCodeTypeId %>', 
															'<%: reasonCodeGroup.Product.ProductName %>',
															'<%: reasonCodeGroup.ReasonCodeType.ReasonCodeTypeDescription %>',
																		'')">
											<img src="<%=Url.Content("~/images/Common/addButton.jpg")%>" style="float:right" />
										</a>
										<%}%>
									</th>
                    </tr>
                   <tr>
									<td width="100">Display Order</td>
									<td width="103">Reason Code</td>
									<td width="273">Reason Code Description</td>
									<td width="60">Source</td>
									<td width="211">Reason Code Group Name</td>
									<td width="131">Actions</td>
                                                
                   </tr>
                  </thead>
                  <tbody>
								<% if (reasonCodeGroup.ReasonCodeProductGroups != null) {
               var i = 1;
									foreach (var item in reasonCodeGroup.ReasonCodeProductGroups) { %>
									<tr id="<%: item.ReasonCodeItemId %>" class="reason_code_item">
										<td><%: item.DisplayOrder %></td>
										<td class="reason_code"><%: item.ReasonCode %></td>
										<%if (item.ReasonCodeAlternativeDescription != "" && item.ReasonCodeAlternativeDescription != null) { %>
											<td><%: item.ReasonCodeAlternativeDescription %></td>
										<%} else{ %>
											<td><%: item.ReasonCodeProductTypeDescription %></td>
										<%}%>
										<td><%: item.Source %>
										<% if (item.Source == "Country") {
                                            Response.Write(" : " + item.SourceName);
										}%>
										</td>
										<td><%: item.ReasonCodeGroupName %></td>
										<td>
										<%if (item.Source != "Custom"){ %>
											Inherited 
                                            <%if (Model.ReasonCodeGroupWriteAccess && item.Source != "Custom : Shared") {

											//groupId, productId, reasonCodeTypeId, product, reasonCodeType, source, groupCount, reasonCode, reasonCodeItemId, reasonCodeDescription %>
											<a href="javascript:RemoveProductReasonCodeType(
															'<%: item.ReasonCodeGroupId %>', 
															'<%: reasonCodeGroup.Product.ProductId %>', 
															'<%: reasonCodeGroup.ReasonCodeType.ReasonCodeTypeId %>', 
															'<%: item.ReasonCode %>',
															'<%: item.VersionNumber %>',
                                                            '<%: item.ReasonCodeItemId %>'
                                                            )">
												<img src="<%=Url.Content("~/images/Common/removeButton.jpg")%>" />
											</a>
                                            <%} %>
										<%} else {%>
											<%if (Model.ReasonCodeGroupWriteAccess && item.Source != "Custom : Shared"){
											//groupId, productId, reasonCodeTypeId, product, reasonCodeType, source, groupCount, reasonCode, reasonCodeItemId%>
											<a href="javascript:EditProductReasonCodeType(
															'<%: item.ReasonCodeGroupId %>',
															'<%: reasonCodeGroup.Product.ProductId %>', 
															'<%: reasonCodeGroup.ReasonCodeType.ReasonCodeTypeId %>', 
															'<%: reasonCodeGroup.Product.ProductName %>',
															'<%: reasonCodeGroup.ReasonCodeType.ReasonCodeTypeDescription %>',
															'<%: item.Source %>',
															'',
															'<%: item.ReasonCode %>',
															'<%: item.ReasonCodeItemId %>')">
												<img src="<%=Url.Content("~/images/Common/editButton.jpg")%>" />
											</a>
											<% //groupId, productId, reasonCodeTypeId, product, reasonCodeType, source, groupCount, reasonCode, reasonCodeItemId, reasonCodeDescription %>
											<a href="javascript:RemoveProductReasonCodeType(
															'<%: item.ReasonCodeGroupId %>', 
															'<%: reasonCodeGroup.Product.ProductId %>', 
															'<%: reasonCodeGroup.ReasonCodeType.ReasonCodeTypeId %>', 
															'<%: item.ReasonCode %>',
															'<%: item.VersionNumber %>',
                                                            '<%: item.ReasonCodeItemId %>'
															)">
												<img src="<%=Url.Content("~/images/Common/removeButton.jpg")%>" />
											</a>
                                        <%}else{ %>
												Read Only	
											<% } %>
                                <%}%>
										</td>
                            </tr>
								<% 
                               
                                    } %>	
							<%  i = i + 1;
           
           } %>	
                 </tbody>   
                </table>
					<% } %>
				<% } %>
			<% } %>
            </td>

        </tr>
         </table>
 <input type="hidden" id="PolicyGroupWriteAccess" value="<%=Model.PolicyGroupWriteAccess%>" />
