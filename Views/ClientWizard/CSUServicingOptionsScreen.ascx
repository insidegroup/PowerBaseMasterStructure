<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.ClientServicingOptionItemsVM>" %>

   <% using (Html.BeginForm()) {%>
        
 <div id="divSearch">
     <div style="float:right;"><span id="clientServicingOptionsBackButton"><small><< Back</small></span>&nbsp;<span id="clientServicingOptionsNextButton"><small>Next >></small> </span></div>
     <div style="float:left;"><span id="clientServicingOptionsSaveButton"><small>Save</small></span>&nbsp;<span id="clientServicingOptionsCancelButton"><small>Cancel</small> </span></div>
	     
     <table class="" cellspacing="0" width="100%" style="padding-top:20px;">
        <thead>
        	<tr> <th><%if(Model.ServicingOptionWriteAccess){ %><span id="clientServicingOptionsAddButton"><small>Add new >></small></span><%}%></th>
                </tr>
                </thead>
        </table>
 
     <% } %>
     </div>

<table class="tablesorter_other" cellspacing="0">
    <tr>
        <td><div id="ServicingOptionsSearchResults"></div></td>
        <th>
        <table id="currentServicingOptions" cellspacing="0" class="tablesorter_other2" border="1">
            <thead>
                <tr>
                    <td colspan="9">Current Servicing Options</td>
                </tr>
                <tr>
<%--                    <th>Display In App?</th>--%>
                    <th>Name</th>
                    <th>Value</th>
                    <th>GDS</th>
                    <th>Order</th>
                    <th>Parameters</th>
                    <th><b>Source</b></th>
                    <th><b>Service Option Group Name</b></th>
<%--                    <th>Instruction</th>--%>
					
                    <th>Actions</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                    <% 
							
						foreach (var item in Model.ServicingOptionVMs)
						{
							%>
							<tr serviceItemID="<%: item.ServicingOptionItem.ServicingOptionItemId %>" 
								serviceOptionID="<%: item.ServicingOptionItem.ServicingOptionId %>" 
								servicingoptionname="<%: item.ServicingOptionItem.ServicingOptionName %>" 
								optionStatus="Current" 
								versionNumber="<%: item.ServicingOptionItem.VersionNumber %>" 
								departureTimeWindowMinutes="<%: item.ServicingOptionItem.DepartureTimeWindowMinutes %>" 
								arrivalTimeWindowMinutes="<%: item.ServicingOptionItem.ArrivalTimeWindowMinutes %>"
								maximumStops="<%: item.ServicingOptionItem.MaximumStops %>"
								maximumConnectionTimeMinutes="<%: item.ServicingOptionItem.MaximumConnectionTimeMinutes %>"
								useAlternateAirportFlag="<%: item.ServicingOptionItem.UseAlternateAirportFlag %>" 
								noPenaltyFlag="<%: item.ServicingOptionItem.NoPenaltyFlag %>"
								noRestrictionsFlag="<%: item.ServicingOptionItem.NoRestrictionsFlag %>"
							>						  
							    <td><input type="hidden" id="itemName" value="<%=item.ServicingOptionItem.ServicingOptionName%>" /><%=item.ServicingOptionItem.ServicingOptionName%></td>

                             
							<%
                            //If we are using drop down for Value
                           
                            if(item.ServicingOptionItemValues.Count > 0){%>
                                <%if(Model.ServicingOptionWriteAccess){ %>
							        <td>
                                        <% if (item.ServicingOptionItem.Source == "Custom") {%>
                                        <select id="soItemValSelect_<%=item.ServicingOptionItem.ServicingOptionItemId%>" name="ValSelect">
								        <%foreach(var item2 in item.ServicingOptionItemValues){
                      
										%><option<%if(item2.ServicingOptionItemValue1 == item.ServicingOptionItem.ServicingOptionItemValue){%> selected<%}%> value="<%=item2.ServicingOptionItemValue1 %>"><%=item2.ServicingOptionItemValue1 %></option><%
									    } %>
                                        </select>
                                        <%} else {%>
                                           <input type="text" value="<%=item.ServicingOptionItem.ServicingOptionItemValue%>" maxlength="50" id="soItemValSelect_<%=item.ServicingOptionItem.ServicingOptionItemId%>" name="ValSelect" readonly style="width:120px; background-color: #e3e1da;"/>
                                        <% } %>
                                    </td>
                                    <td> 
                                        <!--GDS never used for drop down values-->
                                    </td>
                                    <td><%if(item.ServicingOptionItem.ServicingOptionItemSequence!=0){%><%=Html.Encode(item.ServicingOptionItem.ServicingOptionItemSequence)%><%}%></td>
<%--							        <td><input type="text" value="<%=item.ServicingOptionItem.ServicingOptionItemInstruction%>" maxlength="100" id="soInstruction_<%: item.ServicingOptionItem.ServicingOptionItemId %>" style="width:250px;"/></td>--%>
                                   <td></td>
                                <td><%: item.ServicingOptionItem.Source %>
										<% if (item.ServicingOptionItem.Source == "Country") {
                                            Response.Write(" : " + item.ServicingOptionItem.SourceName);
										}%>
										</td>
                                    <td><%=item.ServicingOptionItem.ServicingOptionGroupName%></td>
							        <td>
										<% if(item.ShowEditParameterButton == true && item.ServicingOptionItem.Source == "Custom") { %>
											<img src="<%=Url.Content("~/Images/Common/editButton.jpg")%>" alt="edit" class="edit"/>
                                        <%} else {%>
                                        <% if(item.ShowEditParameterButton == true && item.ServicingOptionItem.Source != "Custom : Shared") { %>
											
                                        <%} else {%>
                                            
                                        <% } %>
                                        <% } %>
							        </td>
									<td>
                                        <% if(item.ServicingOptionItem.Source == "Custom") { %><img src="<%=Url.Content("~/Images/remove.png")%>" alt="remove" class="remove"/>
                                        <%} else {%>
                                         
                                           Inherited 
                                        <% } %>
                                    
									</td>
                                <%}else{%>
                                    <td><%=Html.Encode(item.ServicingOptionItem.ServicingOptionItemValue)%></td>
                                    <td><%=Html.Encode(item.ServicingOptionItem.GDSName)%></td>
                                     <td><%if(item.ServicingOptionItem.ServicingOptionItemSequence!=0){%><%=Html.Encode(item.ServicingOptionItem.ServicingOptionItemSequence)%><%}%></td>
                                    <td></td>
                                <td><%: item.ServicingOptionItem.Source %>
										<% if (item.ServicingOptionItem.Source == "Country") {
                                            Response.Write(" : " + item.ServicingOptionItem.SourceName);
										}%>
										</td>
                                    <td><%=Html.Encode(item.ServicingOptionItem.ServicingOptionGroupName)%></td>

<%--                                    <td><%=Html.Encode(item.ServicingOptionItem.ServicingOptionItemInstruction)%></td>--%>
							        <td>
										<% if(item.ShowEditParameterButton == true && item.ServicingOptionItem.Source == "Custom") { %>
											<img src="<%=Url.Content("~/Images/Common/editButton.jpg")%>" alt="edit" class="edit"/>
                                        <%} else {%>
                                        <% if(item.ShowEditParameterButton == true && item.ServicingOptionItem.Source != "Custom : Shared") { %>
											
                                        <%} else {%>
                                            
                                        <% } %>
                                        <% } %>
							        </td>
									<td></td>
                                <%}%>

						    <%
							}else{
                            //If we are using free text for Value
							%>
                                <%if(Model.ServicingOptionWriteAccess){ 
                                 %>
								    <td>
                                        <% if (item.ServicingOptionItem.Source == "Custom") {%>
                                            <input type="text" value="<%=item.ServicingOptionItem.ServicingOptionItemValue%>" maxlength="50" id="soItemVal_<%: item.ServicingOptionItem.ServicingOptionItemId %>" style="width:120px;"/>
                                         <%} else {%>
                                           <input type="text" value="<%=item.ServicingOptionItem.ServicingOptionItemValue%>" maxlength="50" id="soItemVal_<%: item.ServicingOptionItem.ServicingOptionItemId %>" style="width:120px; background-color: #e3e1da;" readonly/>
                                        <% } %>
                                    </td>
                                    <td>
                                    
                                    <%--<%if (item.ServicingOptionItem.ServicingOptionId == 1 || item.ServicingOptionItem.ServicingOptionId == 2 || item.ServicingOptionItem.ServicingOptionId == 90){  %>--%>
									<% if (item.ServicingOptionItem.GDSRequiredFlag == true){ %>
										 <% if (item.ServicingOptionItem.Source == "Custom") {%>
                                        <select id="gdsCode_<%=item.ServicingOptionItem.ServicingOptionItemId%>"><%
											  foreach (var item3 in Model.GDSs)
											  {
											  %><option<%if(item3.Value == item.ServicingOptionItem.GDSCode){%> selected<%}%> value="<%:item3.Value %>"><%:item3.Text %></option><%
											  }
                            
										 %></select>
                                         <%} else {%>
                                            <input type="text" value="<%=item.ServicingOptionItem.GDSCode%>" maxlength="50" id="gdsCode_<%=item.ServicingOptionItem.ServicingOptionItemId%>" style="width:120px; background-color: #e3e1da;" readonly/>
                                         <% } %>
                                    <% } %>
                                    
                                    
                                    
                                    </td>
                                    <td><%if(item.ServicingOptionItem.ServicingOptionItemSequence!=0){%><%=Html.Encode(item.ServicingOptionItem.ServicingOptionItemSequence)%><%}%></td>
<%--                                    <td><input type="text" value="<%=item.ServicingOptionItem.ServicingOptionItemInstruction%>" maxlength="100" id="soInstruction_<%: item.ServicingOptionItem.ServicingOptionItemId %>" style="width:250px;"/></td>--%>
							        <td></td>
                                <td><%: item.ServicingOptionItem.Source %>
										<% if (item.ServicingOptionItem.Source == "Country") {
                                            Response.Write(" : " + item.ServicingOptionItem.SourceName);
										}%></td>
                                    <td><%=Html.Encode(item.ServicingOptionItem.ServicingOptionGroupName)%></td>
                                    <td>
										<% if(item.ShowEditParameterButton == true && item.ServicingOptionItem.Source == "Custom") { %>
											<img src="<%=Url.Content("~/Images/Common/editButton.jpg")%>" alt="edit" class="edit"/>
                                        <%} else {%>
                                        <% if(item.ShowEditParameterButton == true && item.ServicingOptionItem.Source != "Custom : Shared") { %>
											
                                        <%} else {%>
                                            
                                        <% } %>
                                        <% } %>
							        </td>
									<td>
                                         <% if(item.ShowEditParameterButton == true && item.ServicingOptionItem.Source == "Custom") { %><img src="<%=Url.Content("~/Images/remove.png")%>" alt="remove" class="remove"/>
                                        <%} else {%>
                                        <% if(item.ShowEditParameterButton == false && item.ServicingOptionItem.Source == "Custom") { %>
											
                                        <%} else {%>
                                           Inherited 
                                        <% } %>
                                        <% } %>
									</td>
                                <%}else{%>
                                    <td><%=Html.Encode(item.ServicingOptionItem.ServicingOptionItemValue)%></td>
                                    <td><%=Html.Encode(item.ServicingOptionItem.GDSName)%></td>
                                    <td><%if(item.ServicingOptionItem.ServicingOptionItemSequence!=0){%><%=Html.Encode(item.ServicingOptionItem.ServicingOptionItemSequence)%><%}%></td>
<%--                                    <td><%=Html.Encode(item.ServicingOptionItem.ServicingOptionItemInstruction)%></td>--%>
							        <td></td>
                                    <td><%: item.ServicingOptionItem.Source %>
										<% if (item.ServicingOptionItem.Source == "Country") {
                                            Response.Write(" : " + item.ServicingOptionItem.SourceName);
										}%>
										</td>
                                    <td><%=Html.Encode(item.ServicingOptionItem.ServicingOptionGroupName)%></td>
                                    <td>
										<% if(item.ShowEditParameterButton == true && item.ServicingOptionItem.Source == "Custom") { %>
											<img src="<%=Url.Content("~/Images/Common/editButton.jpg")%>" alt="edit" class="edit"/>
                                        <%} else {%>
                                        <% if(item.ShowEditParameterButton == true && item.ServicingOptionItem.Source != "Custom : Shared") { %>
											
                                        <%} else {%>
                                            
                                        <% } %>
                                        <% } %>
							        </td>
									<td></td>
                                <%}%>
                                <%                       
							  } %>
						   
							
							</tr>
							<%
						
						
						}
					
					%>
                 </tbody>   
                </table>
            

        </tr>
         </table>
      

