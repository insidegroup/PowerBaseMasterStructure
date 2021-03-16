	<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.ClientWizardVM>" %>

<% Html.EnableClientValidation(); %>
<% using (Html.BeginForm()){%>
    <%= Html.ValidationSummary(true)%>
    <div style="float:left"><span id="clientDetailsCancelButton"><small>Cancel</small></span>&nbsp;<span id="clientDetailsSaveButton"><small>Save</small></span></div>
    <div style="float:right"><span id="clientDetailsBackButton"><small><< Back</small></span>&nbsp;<span id="clientDetailsNextButton"><small>Next >></small></span></div>
    
    <br />
    <br />

    <table cellpadding="0" cellspacing="0" width="100%" class="tablesorter_other2" id="clientDetailsTable"> 
        <tr>
            <td width="20%">TopUnit Name</td>
            <td width="40%" <%= Html.Raw((Model.ClientTopUnit.ExpiryDate != null && Model.ClientTopUnit.ExpiryDate.Value <= DateTime.Now) ? " class=\"expired\"" : String.Empty) %>>
                <%= Html.Encode(Model.ClientTopUnit.ClientTopUnitName)%>
            </td>
            <td width="20%"></td>
            <td width="20%"></td>
        </tr>
        <tr>
            <td>TopUnit GUID</td>
            <td><%= Html.Encode(Model.ClientTopUnit.ClientTopUnitGuid)%></td>
            <td></td>
            <td width="20%"></td>
        </tr>
        <tr>
            <td>TopUnit Portrait Status</td>
            <td><%= Html.Encode(Model.ClientTopUnit.PortraitStatus.PortraitStatusDescription)%></td>
            <td></td>
            <td width="20%"></td>
            </tr>
            <tr>
            <td>SubUnit Name</td>
            <td <%= Html.Raw((Model.ClientSubUnit.ExpiryDate != null && Model.ClientSubUnit.ExpiryDate.Value <= DateTime.Now) || (Model.ClientTopUnit.ExpiryDate != null && Model.ClientTopUnit.ExpiryDate.Value <= DateTime.Now) ? " class=\"expired\"" : String.Empty) %>>
                <%= Html.Encode(Model.ClientSubUnit.ClientSubUnitName)%>
            </td>
            <td></td>
            <td width="20%"></td>
            </tr>
            <tr>
            <td>SubUnit GUID</td>
            <td><%= Html.Encode(Model.ClientSubUnit.ClientSubUnitGuid)%></td>
            <td></td>
            <td width="20%"></td>
            </tr>
            <tr>
            <td>Display Name</td>
            <td><%= Html.TextBoxFor(model => model.ClientSubUnit.ClientSubUnitDisplayName, new { maxlength = "50" })%><span id="ClientSubUnit_ClientSubUnitDisplayName_Error" class="error"> *</span></td>
            <td><%= Html.ValidationMessageFor(model => model.ClientSubUnit.ClientSubUnitDisplayName)%></td>
            <td width="20%"></td>
        </tr>
        <tr>
            <td>Client Portrait Status</td>
            <td><%= Html.DropDownList("ClientSubUnit_PortraitStatusId", Model.ClientSubUnitPortraitStatuses, new List<SelectListItem>())%><span class="error"> *</span></td>
            <td></td>
            <td width="20%"></td>
        </tr>
        <tr>
            <td>Portrait Status Description</td>
            <td><%= Html.TextBoxFor(model => model.ClientSubUnit.PortraitStatusDescription, new { maxlength = "100" })%></td>
            <td><%= Html.ValidationMessageFor(model => model.ClientSubUnit.PortraitStatusDescription)%></td>
            <td width="20%"></td>
        </tr>
        <tr>
            <td>Line of Business</td>
            <td><%= Html.DropDownList("ClientSubUnit_LineOfBusinessId", Model.ClientSubUnitLineOfBusinesses, "Please Select")%><span class="error"> *</span></td>
            <td><%= Html.ValidationMessageFor(model => model.ClientSubUnit.LineOfBusinessId)%></td>
            <td width="20%"></td>
        </tr>
		<tr>
            <td>Restricted Client</td>
            <td colspan="3">
				<% if(Model.RestrictedClientAccess == true) { %>
					<%= Html.CheckBox("ClientSubUnit_RestrictedClient", Model.ClientSubUnit.RestrictedClient)%> 
				<% } else { %>
					<%= Html.CheckBox("ClientSubUnit_RestrictedClient", Model.ClientSubUnit.RestrictedClient, new { disabled = true})%> 
				<% }  %>	
				<label for="ClientSubUnit_RestrictedClient">Check this box if you want to restrict counselor's access to this client's information in Power Library. 
					If unchecked, all counselors will be able to view the client's information.</label>
            </td>
        </tr>
		<tr>
            <td>Private Client</td>
            <td colspan="3">
				<% if(Model.RestrictedClientAccess == true) { %>
					<%= Html.CheckBox("ClientSubUnit_PrivateClient", Model.ClientSubUnit.PrivateClient)%> 
				<% } else { %>
					<%= Html.CheckBox("ClientSubUnit_PrivateClient", Model.ClientSubUnit.PrivateClient, new { disabled = true})%> 
				<% }  %>	
				<label for="ClientSubUnit_PrivateClient">Check this box if you want this client to not be visible in Power Library. 
					If unchecked, all counselors will be able to view the client's information.</label>
            </td>
        </tr>
		<tr>
            <td>Cuba Booking Allowed</td>
            <td colspan="3">
				<% if(Model.ComplianceAdministratorAccess == true) { %>
					<%= Html.CheckBox("ClientSubUnit_CubaBookingAllowed", Model.ClientSubUnit.CubaBookingAllowed)%> 
				<% } else { %>
					<%= Html.CheckBox("ClientSubUnit_CubaBookingAllowed", Model.ClientSubUnit.CubaBookingAllowed, new { disabled = true})%> 
				<% }  %>	
				<label for="ClientSubUnit_CubaBookingAllowed">Check this box if this subunit has signed an agreement to book travel to Cuba.</label>
            </td>
        </tr>
		<tr>
            <td>In Country Service Only</td>
            <td colspan="3">
				<%= Html.CheckBox("ClientSubUnit_InCountryServiceOnly", Model.ClientSubUnit.InCountryServiceOnly)%> 
				<label for="ClientSubUnit_InCountryServiceOnly">Check this box if this subunit can only be serviced within their country.</label>
            </td>
        </tr>
		<tr>
            <td>Branch Contact Number</td>
            <td><%= Html.TextBoxFor(model => model.ClientSubUnit.BranchContactNumber, new { maxlength = "20" })%></td>
            <td><%= Html.ValidationMessageFor(model => model.ClientSubUnit.BranchContactNumber)%></td>
            <td width="20%"></td>
        </tr>
		<tr>
            <td>Branch Fax Number</td>
            <td><%= Html.TextBoxFor(model => model.ClientSubUnit.BranchFaxNumber, new { maxlength = "20" })%></td>
            <td><%= Html.ValidationMessageFor(model => model.ClientSubUnit.BranchFaxNumber)%></td>
            <td width="20%"></td>
        </tr>
		<tr>
            <td>Branch Email</td>
            <td><%= Html.TextBoxFor(model => model.ClientSubUnit.BranchEmail, new { maxlength = "50" })%></td>
            <td></td>
            <td width="20%"></td>
        </tr>
		<tr>
            <td>Client IATA</td>
            <td><%= Html.TextBoxFor(model => model.ClientSubUnit.ClientIATA, new { minlength = "8", maxlength = "8" })%></td>
            <td><%= Html.ValidationMessageFor(model => model.ClientSubUnit.ClientIATA)%></td>
            <td width="20%"></td>
        </tr>
		<tr>
            <td>24HSC Dialled Number</td>
            <td><%= Html.TextBoxFor(model => model.ClientSubUnit.DialledNumber24HSC, new { maxlength = "20" })%></td>
            <td><%= Html.ValidationMessageFor(model => model.ClientSubUnit.DialledNumber24HSC)%></td>
            <td width="20%"></td>
        </tr>
        <%if (Model.ClientSubUnitTelephonies != null){%>
        <tr>
            <td colspan="4">
                <table id="currentTelephoniesTable" cellspacing="0" class="tablesorter_other2" width="100%" border="0">
                <tbody></tbody>
                <%
                foreach (var item in Model.ClientSubUnitTelephonies)
                {
                %>
                    <tr id="<%:item.DialedNumber%>_row1" diallednumber="<%= item.DialedNumber%>" versionnumber="<%= item.VersionNumber%>">
                        <td width="20%">Dialled Number</td>
                        <td width="40%"><%= item.DialedNumber%></td>
                        <td width="20%"></td>
                        <td width="20%"></td>
                    </tr> 
                    <tr id="<%:item.DialedNumber%>_row2" diallednumber="<%= item.DialedNumber%>" versionnumber="<%= item.VersionNumber%>">
                        <td width="20%">Identifier</td>
                        <td width="40%"><%= item.CallerEnteredDigitDefinitionType.CallerEnteredDigitDefinitionTypeDescription%></td>
                        <td width="20%"><a href="javascript:removeTelephony('<%:item.DialedNumber%>', '<%:item.VersionNumber%>')"><img src="../../images/remove.png" border="0" /></a></td>
                        <td width="20%"></td>
                    </tr> 
                <%
                }
                %>
                </table>
            </td>
        </tr>
        <%} %>
        <tr>
            <td><label for="DialedNumber">Dialled Number</label></td>
            <td><%= Html.TextBox("DialledNumber","", new { maxlength="15"})%></td>
            <td><span class="field-validation-valid error" id="ClientSubUnit_DialledNumber_validationMessage" style="display: none;"></span></td>
           <td><label id="lblDialedNumberMsg"/></td>
            
        </tr> 
            <tr>
            <td><label for="Identifier">Identifier</label></td>
            <td><%= Html.DropDownList("Identifier", Model.CallerEnteredDigitDefinitionTypes, "Please Select...")%></td>
	        <td><a href="javascript:addTelephony();"><img src="../../images/add.png" border="0" /></a></td>
            <td><label id="lblIdentifierMsg"/></td>


        </tr>
        <tr class="ClientBusinessDescription">
            <td>Client Business Description</td>
            <td colspan="2"><%= Html.TextAreaFor(model => model.ClientSubUnit.ClientBusinessDescription, new { maxlength = "750", @rows = "20" })%></td>
            <td width="20%"><%= Html.ValidationMessageFor(model => model.ClientSubUnit.ClientBusinessDescription)%></td>
        </tr>
    </table>
    <input type="hidden" id="subUnitName" value="<%: Html.Encode(Model.ClientSubUnit.ClientSubUnitName) %>"  />
    <input type="hidden" id="topUnitName" value="<%: Html.Encode(Model.ClientTopUnit.ClientTopUnitName) %>"  />
    <%= Html.Hidden("TopUnitGuid", Model.ClientTopUnit.ClientTopUnitGuid)%>
    <%= Html.HiddenFor(model => model.ClientSubUnit.VersionNumber)%>
    <%= Html.HiddenFor(model => model.ClientTopUnit.VersionNumber)%>
    <%= Html.HiddenFor(model => model.PolicyGroup.PolicyGroupId)%>
<% } %>
<script type="text/javascript">
    var originalForm = $('#form0').serialize();

    function removeTelephony(dialledNumber, versionNumber) {
    
        //Remove From Visible List
        $('#currentTelephoniesTable tbody tr').each(function () {
            if ($(this).attr("diallednumber") == dialledNumber) {
                $(this).remove();
            }
        });

        //Add to Removed list
        $('#hiddenRemovedTelephoniesTable').append("<tr dialledNumber='" + dialledNumber + "' versionNumber='" + versionNumber + "'></tr>")

        //Remove From Added List (if exists)
        $('#hiddenAddedTelephoniesTable tbody tr').each(function () {
            if ($(this).attr("diallednumber") == dialledNumber) {
                $(this).remove();
            }
        });
    }
    
    
    function addTelephony() {

        //STEP 1.validate
        var valid = true;
        if ($("#DialledNumber").val() == "" || isNaN($("#DialledNumber").val())) {
            $("#lblDialedNumberMsg").text("Must be a number");
            valid = false;
        }  
        if ($("#Identifier").val() == "") {
            $("#lblIdentifierMsg").text("Please select an Identifier");
            valid = false;
        }
        //Check if already exists in another field
        $('#currentTelephoniesTable tbody tr').each(function () {
            if ($(this).attr("diallednumber") == $("#DialledNumber").val()) {
                $("#lblDialedNumberMsg").text("Must differ from existing");
                valid = false;
            }
        });
        if (!valid) {
            return;
        }

        //Add to Visible List
        $('#currentTelephoniesTable  > tbody:last').append("<tr id='" + $("#DialledNumber").val() + "_row1' dialledNumber='" + $("#DialledNumber").val() + "' versionnumber='1'><td width='20%'>Dialled Number</td><td width='40%'>" + $("#DialledNumber").val() + "</td><td width='20%'></td><td width='20%'></td></tr> <tr id='" + $("#DialledNumber").val() + "_row2' dialledNumber='" + $("#DialledNumber").val() + "' versionnumber='1'><td width='20%'>Identifier</td><td width='40%'>" + $("#Identifier :selected").text() + "</td><td width='20%'><a href='javascript:removeTelephony(\"" + $("#DialledNumber").val() + "\",\"1\")'><img src='images/remove.png' border='0' /></a></td><td width='20%'></td></tr>");

        //Add to Added List
        $('#hiddenAddedTelephoniesTable').append("<tr dialledNumber='" + $("#DialledNumber").val() + "' CallerEnteredDigitDefinitionTypeId='" + $("#Identifier").val() + "' versionNumber='1'></tr>")

        //Remove From Removed List (if exists)
        $('#hiddenRmovedTelephoniesTable tbody tr').each(function () {
            if ($(this).attr("diallednumber") == dialledNumber) {
                $(this).remove();
            }
        });

        //Clear fields
        $("#DialledNumber").val("");
        $("#Identifier").val("");
        $("#lblDialedNumberMsg").text("");
        $("#lblIdentifierMsg").text("");
    }

    function saveit() {



       

    }
</script>
            
            