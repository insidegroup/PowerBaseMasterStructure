<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.LocationWizardVM>" %>
<div id="divTeam">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm("LocationDetailsScreen", "LocationWizard", FormMethod.Post, new { id = "frmLocationDetails", autocomplete = "off" }))
           {%>
            <%= Html.ValidationSummary(true)%>
          <div style="float:right"><span id="locationDetailsBackButton"><small><< Back</small></span>
              &nbsp;<span id="locationDetailsNextButton"><small>Next >></small></span></div>
            <table cellpadding="0" cellspacing="0" width="100%" class="tablesorter_other" id='locationEditTable'> 
            	<thead>
		        <tr> 
			        <th colspan="3"><%if (Model.Location.LocationId == 0)
                             {%>Create Location<%}else{ %>Edit Location<%} %></th> 
		        </tr> 
                </thead>
                <tbody>
                <tr>
                    <td width="40%"><label for="Location_LocationName">Location Name</label></td>
                    <td width="30%"><%= Html.TextBoxFor(model => model.Location.LocationName, new { maxlength = "50", autocomplete = "off" })%><span class="error"> *</span></td>
                    <td width="20%"><%= Html.ValidationMessageFor(model => model.Location.LocationName)%></td>
                </tr> 
                <tr>
                    <td><label for="Location_CountryName">Country</label></td>
                    <td><%= Html.TextBoxFor(model => model.Location.CountryName, new { maxlength = "100", autocomplete = "off"  })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.Location.CountryName)%>
                        <%= Html.HiddenFor(model => model.Location.CountryCode)%>
                        
                        <label id="lblLocation_CountryNameMsg"/>
                    </td>
                </tr> 
                <tr>
                    <td><label for="Location_CountryRegionId">CountryRegion</label></td>
                    <td><%= Html.DropDownList("Location_CountryRegionId", new List<SelectListItem>())%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.Location.CountryRegionId)%>
                        <%= Html.Hidden("CountryRegionId", Model.Location.CountryRegionId)%>
                    </td>
                </tr>  
               <tr>
                    <td><label for="Address_FirstAddressLine">First Address Line</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.FirstAddressLine, new { maxlength = "80", autocomplete = "off"  })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.FirstAddressLine)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_SecondAddressLine">Second Address Line</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.SecondAddressLine, new { maxlength = "80", autocomplete = "off"  })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.SecondAddressLine)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_CityName">City</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.CityName, new { maxlength = "40", autocomplete = "off"  })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.CityName)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_CountyName">County Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.CountyName, new { maxlength = "40", autocomplete = "off"  })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.CountyName)%></td>
                </tr>
                <tr>
                    <td><label for="Address_StateProvinceName">State/Province</label></td>
                    <td><%= Html.DropDownListFor(model => model.Address.StateProvinceCode, Model.StateProvinces, "Please Select...")%> <span class="stateProvinceCodeError error"> *</span></td>
                    <td>
					    <%= Html.ValidationMessageFor(model => model.Address.StateProvinceCode)%>
					    <label id="lblStateProvinceCodeMsg"/>
                    </td>
                </tr>
                <tr>
                    <td><label for="Address_LatitudeDecimal">Latitude</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.LatitudeDecimal, new { maxlength = "10", autocomplete = "off"  })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.LatitudeDecimal)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_LongitudeDecimal">Longitude</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.LongitudeDecimal, new { maxlength = "10", autocomplete = "off"  })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.LongitudeDecimal)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_MappingQualityCode">Mapping Quality Code</label></td>
                    <td><%= Html.DropDownList("Address_MappingQualityCode", Model.MappingQualityCodes, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.MappingQualityCode)%></td>
                </tr>    
                
                <tr>
                    <td><label for="Address_PostalCode">PostalCode</label></td>
                    <td><%= Html.TextBoxFor(model => model.Address.PostalCode, new { maxlength = "30", autocomplete = "off"  })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.PostalCode)%></td>
                </tr> 
                <tr>
                    <td><label for="Address_ReplicatedFromClientMaintenanceFlag">ReplicatedFromClientMaintenanceFlag</label></td>
                    <td><%= Html.CheckBoxFor(model => model.Address.ReplicatedFromClientMaintenanceFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.Address.ReplicatedFromClientMaintenanceFlag)%></td>
                </tr> 
              
                </tbody>
            </table>
            
               
              
               <span id="deleteLocation" style="float:right"><small>Delete this location</small></span>
               
            <%= Html.HiddenFor(model => model.Location.LocationId)%>
            <%if (Model.Address !=null){%>
                    <%= Html.HiddenFor(model => model.Address.AddressId)%>
                     <%= Html.HiddenFor(model => model.Address.VersionNumber)%>
            <%}%>
            <%= Html.HiddenFor(model => model.Location.VersionNumber)%>
           
            <%= Html.HiddenFor(model => model.SystemUserCount)%>
        <% } %>


        </div>